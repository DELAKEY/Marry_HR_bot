using System;
using System.Collections.Generic;
using System.Text;

namespace TgQuizBot.Database.Models
{
    public class UserModel : ModelBase
    {
        public virtual long Id { get; set; }
        public virtual long TgId { get; set; }
        public virtual string State { get; set; }
    }
}
