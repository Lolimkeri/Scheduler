using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySchedulerWork.Models
{
    
    public class Pair
    {
        public int Id { get; set; }
        virtual public Program_Subject Program_Subject { get; set; }
        virtual public Audience Audience { get; set; }
        virtual public DayOfWeek Day { get; set; }
        public int Number { get; set; }
        virtual public List<PairToFGroup> PairToFGroups { get; set; }
        public int IsEverWeek { get; set; }                                                                             // 0 -> +; 1-> чисельник; 2->знаменник;
        [NotMapped]
        public string NameOfPair { 
            get 
            {
                string res = "(" + Program_Subject.Type + ", " + Audience.Name + ", " + Program_Subject.MainTeacher.ToShortString();
                if (Program_Subject.LabTeacher != null)
                {
                    res += ", " +Program_Subject.LabTeacher.ToShortString();
                }
                res += ")";
                return  res; 
            } 
        }
        public Pair(Program_Subject s, Audience a, DayOfWeek d,int n)
        {
            Program_Subject = s;
            Audience = a;
            Day = d;
            Number = n;
           
        }
        public Pair()
        {

        }
    }
}
