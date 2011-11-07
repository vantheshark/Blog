using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WCF.Validation.Engine
{
    public class DataAnnotationsModelMetadataProvider : AssociatedMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var result = new DataAnnotationsModelMetadata(this, containerType, modelAccessor, modelType, propertyName);

            RequiredAttribute requiredAttribute = attributes.OfType<RequiredAttribute>().FirstOrDefault();
            if (requiredAttribute != null)
            {
                result.IsRequired = true;
            }

            return result;
        }
    }
}
