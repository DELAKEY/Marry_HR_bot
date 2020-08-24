using FluentNHibernate.Utils;
using HtmlAgilityPack;
using Ninject;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TgQuizBot.Database;
using TgQuizBot.Database.Models;
using TgQuizBot.Networck;

namespace TgQuizBot.Services
{
    public class QuizService
    {
        TelegramBotClient telegram;
        DataBase database;
        [Inject]
        public QuizService(TelegramBotClient telegram, DataBase database)
        {
            this.telegram = telegram;
            this.database = database;
        }
        public void StartQuiz(long chat, int quizid)
        {


            var user = database.Users.GetByTg(chat);

            var inter = new IntervierModel()
            {
                User = chat
            };
            database.Interviers.SaveOrUpdate(inter);
            var quest = database.Quests.GetFirst(quizid);
            SendQuest(chat, quest);
            user.State = "quiz " + inter.Id + " " + quest.Id;
            database.Users.SaveOrUpdate(user);

        }

        public void Process(long chat, Telegram.Bot.Types.Message message)
        {
            var user = database.Users.GetByTg(chat);
            string[] state = user.State.Split(' ');
            long questid = long.Parse(state[2]);
            var quest = database.Quests.GetById((int)questid);

            switch (quest.AnswerType)
            {
                case AnswerEnum.Text:
                case AnswerEnum.EditClose:
                    {
                        var answer = new AnswerModel()
                        {
                            Text = message.Text,
                            Quest = (int)questid,
                            Intervier = int.Parse(state[1])

                        };
                        database.Answers.SaveOrUpdate(answer);
                    }
                    break;
                case AnswerEnum.File:
                case AnswerEnum.FileOrText:
                    {
                        string savedata = "";
                        if (message.Document == null){
                            if (quest.AnswerType == AnswerEnum.File)
                            {
                                telegram.SendTextMessageAsync(
                                    chatId: new ChatId(chat),
                                    text: "я жду файл");
                                return;
                            }
                            savedata = message.Text;
                        }
                        else if (message.Text == null)
                        {
                            telegram.SendTextMessageAsync(
                                chatId: new ChatId(chat),
                                text: "я принимаю только текст и файлы");
                        }
                        else
                        {

                            var tinfo = telegram.GetFileAsync(message.Document.FileId);
                            tinfo.Wait();
                            var tfile = telegram.DownloadFileAsync(tinfo.Result.FilePath);
                            tfile.Wait();
                            tfile.Result.Position = 0;
                            var st = ReadFully(tfile.Result);

                            if (!Directory.Exists(state[1]))
                                Directory.CreateDirectory(state[1]);

                            var file = System.IO.File.Create(message.Document.FileName);
                            file.Write(st, 0, st.Length);
                            file.Close();
                            savedata = state[1] + "/" + message.Document.FileName;
                            
                        }
                        var answer = new AnswerModel()
                        {
                            Text = savedata,
                            Quest = (int)questid,
                            Intervier = int.Parse(state[1])

                        };
                        database.Answers.SaveOrUpdate(answer);
                    }
                    break;
                case AnswerEnum.Inn:
                    {
                        string inn = message.Text;
                        var data = InnCheck(inn);
                        if (data != null)
                        {
                            telegram.SendTextMessageAsync(
                                chatId: new ChatId(chat),
                                text: data);
                            var answerw = new AnswerModel()
                            {
                                Text = message.Text,
                                Quest = (int)questid,
                                Intervier = int.Parse(state[1])

                            };
                            database.Answers.SaveOrUpdate(answerw);
                        }
                        else
                        {
                            telegram.SendTextMessageAsync(
                                chatId: new ChatId(chat),
                                text: "не верный ИНН\r\n попробуйте ввести заново").Wait();
                            return;
                        }
                    }
                    break;

            }

            var newquest = database.Quests.GetNext((int)questid);
            if (newquest != null)
            {
                SendQuest(chat, newquest);
                user.State = "quiz " + state[1] + " " + newquest.Id;
                database.Users.SaveOrUpdate(user);
            }
            else
            {
                //save
                List<string> text = new List<string>();
                var intrvier = database.Interviers.GetById((int)long.Parse(state[1]));

                var answers = database.Answers.GetAnswers(long.Parse(state[1]));
                foreach (var answer in answers)
                {
                    var lquest = database.Quests.GetById(answer.Quest);
                    text.Add(answer.Text);
                    switch (lquest.AnswerType)
                    {
                        case AnswerEnum.Inn:
                            text.Add(InnCheck(answer.Text));
                            break;
                        default:
                            break;
                    }
                }
                var sender = new SheetsWorker();
                sender.Send(text.ToArray());
                telegram.SendTextMessageAsync(
                    chatId: new ChatId(chat),
                    text: "благодарим за оставленную заявку")
                    .Wait();
                user.State = "";
                database.Users.SaveOrUpdate(user);
            }

        }
        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public void SendQuest(long chat, QuestModel quest)
        {
            switch (quest.AnswerType)
            {
                case AnswerEnum.Inn:
                case AnswerEnum.Text:
                case AnswerEnum.File:
                    telegram.SendTextMessageAsync(
                        chatId: new ChatId(chat),
                        text: quest.Text
                        );
                    break;
                case AnswerEnum.Close:
                case AnswerEnum.EditClose:
                    telegram.SendTextMessageAsync(
                        chatId: new ChatId(chat),
                        text: quest.Text);
                    break;
            }
        }
        [Callback("quiz")]
        public void inlinecallback(long chat, long user, string data)
        {
            string[] param = data.Split(' ');
            telegram.SendTextMessageAsync(
                chatId: new ChatId(chat),
                text: "select");
        }

        public string InnCheck(string inn)
        {
            string url = "https://ind-kod.org.ua/cgi-bin/decodecode.cgi?c=" + inn;

            string html = "";
            using (WebClient client = new WebClient())
            {


                byte[] g = client.DownloadData(url);
                html = Encoding.UTF8.GetString(g);
            }

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var el_dr = doc.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/div[3]/center/b/u[1]");
            if (el_dr == null) return null;

            var el_pol = doc.DocumentNode.SelectSingleNode(
                "/html/body/div[1]/div[3]/center/b/u[2]");
            var val = "пол " + el_pol.InnerText + "\r\nдата рождения" + el_dr.InnerText;
            return val;
        }
        [Command("check", "")]
        public void testc(long chat, long user)
        {
            var ret = InnCheck("3017910191");

        }
    }
}