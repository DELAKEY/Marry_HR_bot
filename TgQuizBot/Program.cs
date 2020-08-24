using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using TgQuizBot.Application;

namespace TgQuizBot
{
    class Program
    {
        static void Main(string[] args)
        {
            new App().Run();
            return;
        }

    }
}
