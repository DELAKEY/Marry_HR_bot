using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using TgQuizBot.Database;
using TgQuizBot.Services;

namespace TgQuizBot.Networck
{
    class TelegramWorcker
    {
        private readonly TelegramBotClient telegram;
        private readonly CommandDispetcher commandDispetcher;
        CallbackDispetcher callbackDispetcher;
        DataBase database;
        QuizService quiz;
        public TelegramWorcker(TelegramBotClient telegram,
            CommandDispetcher commandDispetcher, CallbackDispetcher callbackDispetcher,
            DataBase dataBase,
            QuizService quiz)
        {
            this.telegram = telegram;
            this.commandDispetcher = commandDispetcher;
            this.callbackDispetcher = callbackDispetcher;
            this.database = dataBase;
            this.quiz = quiz;

            telegram.OnCallbackQuery += Telegram_OnCallbackQuery;
            telegram.OnMessage += Telegram_OnMessage;
        }

        private void Telegram_OnMessage(object sender, MessageEventArgs e)
        {
            string text = e.Message.Text;

            if ((text != null) && (text[0] == '/'))
            {
                string command = text.Substring(1);

                commandDispetcher.CommandInvoke(e.Message.Chat.Id, e.Message.From.Id, command);
            }
            else
            {
                var user = database.Users.GetByTg(e.Message.Chat.Id);
                string[] state = user.State.Split(' ');
                switch (state[0])
                {
                    case "quiz":
                        quiz.Process(e.Message.Chat.Id, e.Message);
                        break;
                }
            }
        }

        private void Telegram_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            callbackDispetcher.CommandInvoke(e.CallbackQuery.Message.Chat.Id, e.CallbackQuery.From.Id, e.CallbackQuery.Data);
        }
    }
}
