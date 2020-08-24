using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TgQuizBot.Services;

namespace TgQuizBot
{
    public class MainController
    {
        private readonly TelegramBotClient telegram;
        private readonly QuizService quiz;
        public MainController(TelegramBotClient telegram,QuizService quiz)
        {

            this.telegram = telegram;
            this.quiz = quiz;
        }
        [Command("start","")]
        public void start(long chat, long user)
        {
            var keys = new List<List<InlineKeyboardButton>>();
            keys.Add(new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton()
                {
                    Text = "Работодатель",
                    CallbackData = "start master"
                }
            });
            keys.Add(new List<InlineKeyboardButton>()
            {
                new InlineKeyboardButton()
                {
                    Text = "Работник",
                    CallbackData = "start slave"

                }
            });
            var keyboard = new InlineKeyboardMarkup(keys);
           
            FileStream file = System.IO.File.OpenRead("wellcome.txt");
            telegram.SendTextMessageAsync(
                chatId: new ChatId(chat),
                text: ( new StreamReader( file).ReadToEnd())
                ,replyMarkup:keyboard);
        }
        [Callback("start")]
        public void callbackstart(long chat,long user,string param)
        {
            string[] data = param.Split(' ');
            if (data[1] == "slave")
                quiz.StartQuiz(chat, 1);
            else
                quiz.StartQuiz(chat, 2);
        }
    }
}
