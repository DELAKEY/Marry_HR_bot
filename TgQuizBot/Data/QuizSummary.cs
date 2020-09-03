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
