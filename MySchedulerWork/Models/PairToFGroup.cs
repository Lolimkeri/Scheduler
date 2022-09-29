using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MySchedulerWork.Models
{
    public class PairToFGroup
    {
        public int Id { get; set; }
        virtual public Pair Pair { get; set; }
        virtual public Group Group { get; set; }
    }
}
