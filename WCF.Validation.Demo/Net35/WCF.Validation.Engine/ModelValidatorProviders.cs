using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace WCF.Validation.Engine
{
    public static class ModelValidatorProviders
    {
        private static readonly Collection<ModelValidatorProvider> _providers = new Collection<ModelValidatorProvider>
                                                                                    {
                                                                                        new DataAnnotationsModelValidatorProvider()
                                                                                    };

        public static Collection<ModelValidatorProvider> Providers
        {
            get
            {
                return _providers;
            }
        }

        public static IEnumerable<ModelValidator> GetValidators(this Collection<ModelValidatorProvider> providers, ModelMetadata metadata) {
            return providers.SelectMany(provider => provider.GetValidators(metadata));
        }
    }
}
