using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WCF.Validation.Engine
{
    internal static class TypeDescriptorHelper
    {
        public static IEnumerable<PropertyDescriptor> GetPropertyDescriptors(Type type)
        {
            var prop = TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>();
            return prop;
        }

        public static ICustomTypeDescriptor Get(Type type)
        {
            return new AssociatedMetadataTypeTypeDescriptionProvider(type).GetTypeDescriptor(type);
        }
    }
}
