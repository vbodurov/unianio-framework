using Unianio.Enums;
using Unianio.Human;
using Unianio.MakeHuman;
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
            RegisterSingletonType<IHumanBoneWrapper, MakeHumanBoneWrapper>(HumanoidType.MakeHuman.ToString());
//            DefineIdentifierForType<IVisualTestsScene>(scene.VisualTests);
        }
    }
}