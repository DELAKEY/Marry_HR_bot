using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgQuizBot.Database;

namespace TgQuizBot.initdb
{
    class Program
    {
        static void Main(string[] args)
        {
            DataProvider.CreateDatabase();
            Console.ReadLine();
        }
    
    }
}
