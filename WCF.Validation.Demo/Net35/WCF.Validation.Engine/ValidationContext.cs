using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace WCF.Validation.Engine
{
    /// <summary>
    /// Copy from .NET 4.0 source code
    /// </summary>
    public sealed class ValidationContext : IServiceProvider
    {
        // Methods
        public ValidationContext(object instance, IServiceProvider serviceProvider, IDictionary<object, object> items)
        {
            ObjectInstance = instance;
            ServiceContainer = serviceProvider as IServiceContainer;
            Items = items;

            if (instance != null)
            {
                ObjectType = instance.GetType();
            }
        }

        public object GetService(Type serviceType)
        {
            return ServiceContainer.GetService(serviceType);
        }

        // Properties
        public string DisplayName { get; set; }

        public IDictionary<object, object> Items { get; private set; }

        public string MemberName { get; set; }

        public object ObjectInstance { get; private set; }

        public Type ObjectType { get; private set; }

        public IServiceContainer ServiceContainer { get; private set; }
    }


}
