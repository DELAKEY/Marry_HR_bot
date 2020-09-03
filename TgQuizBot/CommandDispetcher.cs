using System;
using System.Collections.Generic;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgQuizBot.Services;

namespace TgQuizBot
{
    public class CommandDispetcher
    {
        private readonly TelegramBotClient telegram;
        List<CommandData> Commands = new List<CommandData>();

        public CommandDispetcher(TelegramBotClient telegram, MainController mainController, QuizService quizService)
        {
            this.telegram = telegram;
            ClassRegister(mainController);
            ClassRegister(quizService);
        }
        public void ClassRegister(object obj)
        {
            Type type = obj.GetType(); 
            var methods = type.GetMethods();
            foreach(var method in methods)
            {
                var data = method.GetCustomAttribute<CommandAttribute>();
                if (data != null)
                {
                    Commands.Add(
                        new CommandData()
                        {
                            Object = obj,
                            m = method,
                            c = data
                        });
                }
            }
        }

        public void CommandInvoke(long chat, long user, string command)
        {
            var m = Commands.Find(x => x.c.Command == command.ToLower());
            if (m != null)
            {
                m.m.Invoke(m.Object, new object[2] { chat, user });
            }
            else
            {
                telegram.SendTextMessageAsync(
                   chatId: new ChatId(user),
                   text: "no command");
            }
        }
        public class CommandData {
            public object Object;
            public MethodInfo m;
            public CommandAttribute c;
        }

    }
    public class CommandAttribute : System.Attribute
    {
        public string Command { get; private set; }
        public string Discript { get; private set; }
        public CommandAttribute(string command, string discript)
        {
            Command = command;
            Discript = discript;
        }
    }

}
