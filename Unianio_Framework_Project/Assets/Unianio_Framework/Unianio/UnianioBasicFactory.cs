using Unianio.Services;

namespace Unianio
{
    public class UnianioBasicFactory : SimpleBaseFactory
    {
        public UnianioBasicFactory()
        {
            RegisterSingletonType<IMessenger, DefaultMessenger>();

            // multiple instances:
            //RegisterFactoryType<ISomething, Something>();

            // identifiers
//            DefineIdentifierForType<IVisualTestsScene>(scene.VisualTests);
        }
    }
}