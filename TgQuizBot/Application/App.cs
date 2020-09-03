using Ninject;
using NLog;
using System;
using Telegram.Bot;
using TgQuizBot.Networck;

namespace TgQuizBot.Application
{
    class App
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();
        protected IKernel Injector { get; private set; }

        public App()
        {
            Injector = new StandardKernel();
            Injector.Load(new[] { new ApplicationModule() });
        }

        public void Run()
        {
            /*
            var m = Injector.Get<Methods>();
            System.Collections.Specialized.NameValueCollection tt = new System.Collections.Specialized.NameValueCollection();
            tt.Add("login","111");
            tt.Add("password", "111");
            m.Process("Auth.Login", new System.Collections.Specialized.NameValueCollection(), tt);*/

            var telegram = Injector.Get<TelegramBotClient>();
            {
                Injector.Get<TelegramWorcker>();
                telegram.StartReceiving();

                Logger.Debug("Server Up");

                Console.ReadLine();

                telegram.StopReceiving();
                //Injector.Get<ActualTest>().Stop();
                Logger.Debug("Server Down");
            }
        }
    }
}
