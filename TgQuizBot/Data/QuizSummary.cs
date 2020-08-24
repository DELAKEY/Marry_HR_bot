using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TgQuizBot.Database;
using TgQuizBot.Database.Models;

namespace TgQuizBot.Data
{
    class QuizSummary
    {
        public int Id { get; set; }
        public RemovableList<QuestModel> Quests { get; set; }

        
    }
}
