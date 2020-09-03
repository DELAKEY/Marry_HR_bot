using System;
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
