using System;
using System.Collections.Generic;
using System.Text;

namespace TgQuizBot.Database.Models
{
    public class IntervierModel : ModelBase
    {
        public virtual int Id { get; set; }
        public virtual long User { get; set; }
    }
}
