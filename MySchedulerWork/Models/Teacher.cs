using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySchedulerWork.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FutherName { get; set; }
        [NotMapped]
        public string FullName { get {return LastName + " " + Name + " " + FutherName; } }
        public override string ToString()
        {
            return LastName + " " + Name + " " + FutherName;
        }
        public string ToShortString()
        {
            return LastName + " " + Name[0] + "." + FutherName[0]+'.';
        }
    }
}
