using System.Collections.Generic;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgQuizBot.Database;
using TgQuizBot.Database.Models;
using TgQuizBot.Services;

namespace TgQuizBot
{
    public class MainController
    {
        private readonly TelegramBotClient telegram;
        private readonly QuizService quiz;
        private readonly DataBase database;
        public MainController(TelegramBotClient telegram, QuizService quiz, DataBase dataBase)
        {
            this.database = dataBase;
            this.telegram = telegram;
            this.quiz = quiz;
        }
        [Command("start", "")]
        public void start(long chat, long user)
        {
            var t = telegram.GetChatMemberAsync(new ChatId(chat), (int)chat);
            t.Wait();
            string code = t.Result.User.LanguageCode;
            LangModel lang = database.Langs.GetByCode(code);
            if (lang == null)
            {
                lang = database.Langs.GetById(1);
            } 

            var keys = new List<List<InlineKeyboardButton>>();
            keys.Add(new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton()
                {
                    Text = lang.Applicant,
                    CallbackData = "start master ru"
                }
            });
            keys.Add(new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton()
                {
                    Text = lang.Emploer,
                    CallbackData = "start slave ru"

                }
            });
            var keyboard = new InlineKeyboardMarkup(keys);
            telegram.SendTextMessageAsync(
                chatId: new ChatId(chat),
                text: lang.Wellcome
                , replyMarkup: keyboard);

        }
        [Callback("start")]
        public void callbackstart(long chat, long user, string param)
        {
            string[] data = param.Split(' ');
            if (data[1] == "slave")
                quiz.StartQuiz(chat, 1);
            else
                quiz.StartQuiz(chat, 2);
        }
    }
}