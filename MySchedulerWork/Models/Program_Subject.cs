using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MySchedulerWork.Models
{
    public class Program_Subject
    {
        public int Id { get; set; }
        virtual public Subject Subject { get; set; }
        public string Type { get; set; }
        virtual public Teacher MainTeacher { get; set; }
        virtual public Teacher LabTeacher { get; set; }
        virtual public CourseProgram ProgramId { get; set; }
        [NotMapped]
        public string Name { get { return Subject.Name + " " + Type + " " + MainTeacher.ToShortString(); } }
    }
}
