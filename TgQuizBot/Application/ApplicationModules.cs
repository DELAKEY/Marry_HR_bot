using Ninject.Modules;
using Telegram.Bot;
using TgQuizBot.Database;

namespace TgQuizBot.Application
{

    public class ApplicationModule : NinjectModule
    {
        public ApplicationModule()
        {

        }

        public override void Load()
        {

            Bind<TelegramBotClient>().ToConstant(
                new TelegramBotClient("1277962709:AAFzx-5DZX4UvZhXTWfhuecfz-IF9I4wQ8o"));

            Bind<DataBase>().ToConstant(
                new DataProvider
                ("82.146.34.243",
                "testmap","root2",
                "11223344556677889900")
                .Database).InSingletonScope();
            Bind<CommandDispetcher>().To<CommandDispetcher>().InSingletonScope();
           //Bind<UserServices>().To<UserServices>().InSingletonScope();*/

        }
    }
}
