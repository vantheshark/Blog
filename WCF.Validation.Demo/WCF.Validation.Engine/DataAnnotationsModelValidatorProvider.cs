using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace WCF.Validation.Engine
{
    // A factory for validators based on ValidationAttribute
    public delegate ModelValidator DataAnnotationsModelValidationFactory(ModelMetadata metadata, ValidationAttribute attribute);

    // A factory for validators based on IValidatableObject
    public delegate ModelValidator DataAnnotationsValidatableObjectAdapterFactory(ModelMetadata metadata);

    /// <summary>
    /// An implementation of <see cref="ModelValidatorProvider"/> which providers validators
    /// for attributes which derive from <see cref="ValidationAttribute"/>. It also provides
    /// a validator for types which implement <see cref="IValidatableObject"/>. To support
    /// client side validation, you can either register adapters through the static methods
    /// on this class, or by having your validation attributes implement
    /// <see cref="IClientValidatable"/>. The logic to support IClientValidatable
    /// is implemented in <see cref="DataAnnotationsModelValidator"/>.
    /// </summary>
    public class DataAnnotationsModelValidatorProvider : AssociatedValidatorProvider
    {
        private static bool _addImplicitRequiredAttributeForValueTypes;
        private static ReaderWriterLockSlim _adaptersLock = new ReaderWriterLockSlim();

        // Factories for validation attributes

        internal static DataAnnotationsModelValidationFactory DefaultAttributeFactory = (metadata,  attribute) => new DataAnnotationsModelValidator(metadata, attribute);

        internal static Dictionary<Type, DataAnnotationsModelValidationFactory> AttributeFactories = new Dictionary<Type, DataAnnotationsModelValidationFactory> 
        {
            {
                typeof(RangeAttribute),
                (metadata, attribute) => new DataAnnotationsModelValidator(metadata, attribute) //new RangeAttributeAdapter(metadata, (RangeAttribute)attribute)
            },

            {
                typeof(RegularExpressionAttribute),
                (metadata, attribute) => new DataAnnotationsModelValidator(metadata, attribute)//new RegularExpressionAttributeAdapter(metadata, (RegularExpressionAttribute)attribute)
            },

            {
                typeof(RequiredAttribute),
                (metadata, attribute) => new DataAnnotationsModelValidator(metadata, attribute)//new RequiredAttributeAdapter(metadata, (RequiredAttribute)attribute)
            },

            {
                typeof(StringLengthAttribute),
                (metadata, attribute) => new DataAnnotationsModelValidator(metadata, attribute)//new StringLengthAttributeAdapter(metadata, (StringLengthAttribute)attribute)
            },
        };

        // Factories for IValidatableObject models

        internal static DataAnnotationsValidatableObjectAdapterFactory DefaultValidatableFactory = metadata => new ValidatableObjectAdapter(metadata);

        internal static Dictionary<Type, DataAnnotationsValidatableObjectAdapterFactory> ValidatableFactories = new Dictionary<Type, DataAnnotationsValidatableObjectAdapterFactory>();

        public static bool AddImplicitRequiredAttributeForValueTypes
        {
            get
            {
                return _addImplicitRequiredAttributeForValueTypes;
            }
            set
            {
                _addImplicitRequiredAttributeForValueTypes = value;
            }
        }

        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, IEnumerable<Attribute> attributes)
        {
            _adaptersLock.EnterReadLock();

            try
            {
                List<ModelValidator> results = new List<ModelValidator>();

                // Add an implied [Required] attribute for any non-nullable value type,
                // unless they've configured us not to do that.
                if (AddImplicitRequiredAttributeForValueTypes /*&& metadata.IsRequired */ && !attributes.Any(a => a is RequiredAttribute))
                {
                    attributes = attributes.Concat(new[] { new RequiredAttribute() });
                }

                // Produce a validator for each validation attribute we find
                foreach (ValidationAttribute attribute in attributes.OfType<ValidationAttribute>())
                {
                    DataAnnotationsModelValidationFactory factory;
                    if (!AttributeFactories.TryGetValue(attribute.GetType(), out factory))
                    {
                        factory = DefaultAttributeFactory;
                    }
                    results.Add(factory(metadata, attribute));
                }

                // Produce a validator if the type supports IValidatableObject
                if (typeof(IValidatableObject).IsAssignableFrom(metadata.ModelType))
                {
                    DataAnnotationsValidatableObjectAdapterFactory factory;
                    if (!ValidatableFactories.TryGetValue(metadata.ModelType, out factory))
                    {
                        factory = DefaultValidatableFactory;
                    }
                    results.Add(factory(metadata));
                }

                return results;
            }
            finally
            {
                _adaptersLock.ExitReadLock();
            }
        }

        #region Validation attribute adapter registration

        public static void RegisterAdapter(Type attributeType, Type adapterType)
        {
            ValidateAttributeType(attributeType);
            ValidateAttributeAdapterType(adapterType);
            ConstructorInfo constructor = GetAttributeAdapterConstructor(attributeType, adapterType);

            _adaptersLock.EnterWriteLock();

            try
            {
                AttributeFactories[attributeType] = (metadata, attribute) => (ModelValidator)constructor.Invoke(new object[] { metadata, attribute });
            }
            finally
            {
                _adaptersLock.ExitWriteLock();
            }
        }

        public static void RegisterAdapterFactory(Type attributeType, DataAnnotationsModelValidationFactory factory)
        {
            ValidateAttributeType(attributeType);
            ValidateAttributeFactory(factory);

            _adaptersLock.EnterWriteLock();

            try
            {
                AttributeFactories[attributeType] = factory;
            }
            finally
            {
                _adaptersLock.ExitWriteLock();
            }
        }

        public static void RegisterDefaultAdapter(Type adapterType)
        {
            ValidateAttributeAdapterType(adapterType);
            ConstructorInfo constructor = GetAttributeAdapterConstructor(typeof(ValidationAttribute), adapterType);

            DefaultAttributeFactory = (metadata, attribute) => (ModelValidator)constructor.Invoke(new object[] { metadata, attribute });
        }

        public static void RegisterDefaultAdapterFactory(DataAnnotationsModelValidationFactory factory)
        {
            ValidateAttributeFactory(factory);

            DefaultAttributeFactory = factory;
        }

        // Helpers 

        private static ConstructorInfo GetAttributeAdapterConstructor(Type attributeType, Type adapterType)
        {
            ConstructorInfo constructor = adapterType.GetConstructor(new[] { typeof(ModelMetadata), attributeType });
            if (constructor == null)
            {
                throw new ArgumentException("adapterType");
            }

            return constructor;
        }

        private static void ValidateAttributeAdapterType(Type adapterType)
        {
            if (adapterType == null)
            {
                throw new ArgumentNullException("adapterType");
            }
            if (!typeof(ModelValidator).IsAssignableFrom(adapterType))
            {
                throw new ArgumentException("adapterType");
            }
        }

        private static void ValidateAttributeType(Type attributeType)
        {
            if (attributeType == null)
            {
                throw new ArgumentNullException("attributeType");
            }
            if (!typeof(ValidationAttribute).IsAssignableFrom(attributeType))
            {
                throw new ArgumentException("attributeType");
            }
        }

        private static void ValidateAttributeFactory(DataAnnotationsModelValidationFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
        }

        #endregion

        #region IValidatableObject adapter registration

        /// <summary>
        /// Registers an adapter type for the given <see cref="modelType"/>, which must
        /// implement <see cref="IValidatableObject"/>. The adapter type must derive from
        /// <see cref="ModelValidator"/> and it must contain a public constructor
        /// which takes two parameters of types <see cref="ModelMetadata"/> and
        /// <see cref="ControllerContext"/>.
        /// </summary>
        public static void RegisterValidatableObjectAdapter(Type modelType, Type adapterType)
        {
            ValidateValidatableModelType(modelType);
            ValidateValidatableAdapterType(adapterType);
            ConstructorInfo constructor = GetValidatableAdapterConstructor(adapterType);

            _adaptersLock.EnterWriteLock();

            try
            {
                ValidatableFactories[modelType] = (metadata) => (ModelValidator)constructor.Invoke(new object[] { metadata });
            }
            finally
            {
                _adaptersLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Registers an adapter factory for the given <see cref="modelType"/>, which must
        /// implement <see cref="IValidatableObject"/>.
        /// </summary>
        public static void RegisterValidatableObjectAdapterFactory(Type modelType, DataAnnotationsValidatableObjectAdapterFactory factory)
        {
            ValidateValidatableModelType(modelType);
            ValidateValidatableFactory(factory);

            _adaptersLock.EnterWriteLock();

            try
            {
                ValidatableFactories[modelType] = factory;
            }
            finally
            {
                _adaptersLock.ExitWriteLock();
            }
        }

        /// <summary>
        /// Registers the default adapter type for objects which implement
        /// <see cref="IValidatableObject"/>. The adapter type must derive from
        /// <see cref="ModelValidator"/> and it must contain a public constructor
        /// which takes two parameters of types <see cref="ModelMetadata"/> and
        /// <see cref="ControllerContext"/>.
        /// </summary>
        public static void RegisterDefaultValidatableObjectAdapter(Type adapterType)
        {
            ValidateValidatableAdapterType(adapterType);
            ConstructorInfo constructor = GetValidatableAdapterConstructor(adapterType);

            DefaultValidatableFactory = metadata => (ModelValidator)constructor.Invoke(new object[] { metadata });
        }

        /// <summary>
        /// Registers the default adapter factory for objects which implement
        /// <see cref="IValidatableObject"/>.
        /// </summary>
        public static void RegisterDefaultValidatableObjectAdapterFactory(DataAnnotationsValidatableObjectAdapterFactory factory)
        {
            ValidateValidatableFactory(factory);

            DefaultValidatableFactory = factory;
        }

        // Helpers 

        private static ConstructorInfo GetValidatableAdapterConstructor(Type adapterType)
        {
            ConstructorInfo constructor = adapterType.GetConstructor(new[] { typeof(ModelMetadata)});
            if (constructor == null)
            {
                throw new ArgumentException("adapterType");
            }

            return constructor;
        }

        private static void ValidateValidatableAdapterType(Type adapterType)
        {
            if (adapterType == null)
            {
                throw new ArgumentNullException("adapterType");
            }
            if (!typeof(ModelValidator).IsAssignableFrom(adapterType))
            {
                throw new ArgumentException("adapterType");
            }
        }

        private static void ValidateValidatableModelType(Type modelType)
        {
            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }
            if (!typeof(IValidatableObject).IsAssignableFrom(modelType))
            {
                throw new ArgumentException("modelType");
            }
        }

        private static void ValidateValidatableFactory(DataAnnotationsValidatableObjectAdapterFactory factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }
        }

        #endregion
    }
}
