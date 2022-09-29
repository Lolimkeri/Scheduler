using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySchedulerWork.Models;
namespace MySchedulerWork.Services
{
    public class SchedulerService
    {
        public Pair AddPair(Program_Subject subject, Audience audience, DayOfWeek day, int Number)
        {
            Pair pair = new Pair( subject, audience, day, Number);
            return pair;
        }
        public void DeletePair()
        {

        }
    }
}
