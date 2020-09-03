using System;
using System.Collections.Generic;
using System.Reflection;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgQuizBot.Services;

namespace TgQuizBot
{
    class CallbackDispetcher
    {
        private readonly TelegramBotClient telegram;
        List<CallbackData> Commands = new List<CallbackData>();

        public CallbackDispetcher(TelegramBotClient telegram, MainController mainController,QuizService quizService)
        {
            this.telegram = telegram;
            ClassRegister(mainController);
            ClassRegister(quizService);
        }
        public void ClassRegister(object obj)
        {
            Type type = obj.GetType();
            var methods = type.GetMethods();
            foreach (var method in methods)
            {
                var data = method.GetCustomAttribute<CallbackAttribute>();
                if (data != null)
                {
                    Commands.Add(
                        new CallbackData()
                        {
                            Object = obj,
                            m = method,
                            c = data
                        });
                }
            }
        }

        public void CommandInvoke(long chat, long user, string data)
        {
            var s = data.Split(' ');
            var m = Commands.Find(x => x.c.Command == s[0].ToLower());
            if (m != null)
            {
                m.m.Invoke(m.Object, new object[3] { chat, user,data });
            }
            else
            {
                telegram.SendTextMessageAsync(
                   chatId: new ChatId(user),
                   text: "no command");
            }
        }
        public class CallbackData
        {
            public object Object;
            public MethodInfo m;
            public CallbackAttribute c;
        }

    }
    public class CallbackAttribute : System.Attribute
    {
        public string Command { get; private set; }
        public CallbackAttribute(string command)
        {
            Command = command;
        }
    }
}

