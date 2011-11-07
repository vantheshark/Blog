
namespace WCF.Validation.Engine
{
    public class ModelMetadataProviders
    {
        private ModelMetadataProvider _currentProvider;
        private static ModelMetadataProviders _instance = new ModelMetadataProviders();
        

        internal ModelMetadataProviders()
        {
            _currentProvider = new DataAnnotationsModelMetadataProvider();
        }

        public static ModelMetadataProvider Current
        {
            get
            {
                return _instance._currentProvider;
            }
            set
            {
                _instance._currentProvider = value;
            }
        }
    }
}
