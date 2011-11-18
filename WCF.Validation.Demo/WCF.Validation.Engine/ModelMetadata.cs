using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WCF.Validation.Engine
{
    public class ModelMetadata
    {
        public string FullName { get; protected set; }
        
        private Func<object> _modelAccessor;
        private IEnumerable<ModelMetadata> _properties;

        protected ModelMetadataProvider Provider { get; set; }

        public virtual bool IsRequired { get; set; }

        private object _modelValue;
        public object ModelValue
        {
            get
            {
                if (_modelAccessor != null)
                {
                    _modelValue = _modelAccessor();
                    _modelAccessor = null;
                }
                return _modelValue;
            }
            set
            {
                _modelValue = value;
                _modelAccessor = null;
                _properties = null;
            }
        }

        public virtual bool IsComplexType
        {
            get
            {
                return !(TypeDescriptor.GetConverter(ModelType).CanConvertFrom(typeof(string)));
            }
        }

        public Type ModelType { get; private set; }
        public Type ContainerType { get; private set; }
        public string PropertyName { get; private set; }


        public virtual IEnumerable<ModelMetadata> Properties
        {
            get
            {
                if (IsComplexType)
                {
                    if (_properties == null)
                    {
                        _properties = Provider.GetMetadataForProperties(ModelValue, ModelType);
                        var list = _properties.ToList();
                        list.ForEach(p => p.FullName = FullName + "." + p.FullName);
                        _properties = list;
                    }
                    return _properties;
                }

                return Enumerable.Empty<ModelMetadata>();
            }
        }

        public ModelMetadata(ModelMetadataProvider provider, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            Check.Requires<ArgumentNullException>(provider != null);
            Check.Requires<ArgumentNullException>(modelType != null);

            Provider = provider;
            ModelType = modelType;
            ContainerType = containerType;
            PropertyName = propertyName;

            _modelAccessor = modelAccessor;

            FullName = propertyName ?? modelType.Name;
        }

        public virtual IEnumerable<ModelValidator> GetValidators()
        {
            if (typeof(IEnumerable).IsAssignableFrom(ModelType) && IsComplexType /* not a string which is a list of char*/)
            {
                return GetValidatorForEnumerable();
            }

            if (IsComplexType)
            {
                return GetValidatorForOtherComplexTypes();
            }

            return ModelValidatorProviders.Providers.GetValidators(this);
        }

        private IEnumerable<ModelValidator> GetValidatorForEnumerable()
        {
            var childValidators = Enumerable.Empty<ModelValidator>();
            if (ModelValue == null)
            {
                return GetValidatorForNullModel();
            }

            var index = 0;
            foreach (var item in (IEnumerable)ModelValue)
            {
                var propertyMetaData = Provider.GetMetadataForType(() => item, item.GetType());
                propertyMetaData.FullName = string.Format("{0}[{1}]", FullName, index++);
                childValidators = childValidators.Concat(propertyMetaData.GetValidators());
            }
            return childValidators.Concat(new[] { ModelValidator.GetModelValidator(this) });
        }

        private IEnumerable<ModelValidator> GetValidatorForOtherComplexTypes()
        {
            return ModelValue != null 
                              ? ModelValidatorProviders.Providers.GetValidators(this).Union(new [] { ModelValidator.GetModelValidator(this)}) // composit + other validators for it's attributes
                              : GetValidatorForNullModel();
        }

        protected virtual IEnumerable<ModelValidator> GetValidatorForNullModel()
        {
            // NOTE: The base model metadata will not need validation on null
            return Enumerable.Empty<ModelValidator>();
        }
    }
}
