using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MySchedulerWork.Models
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int Course { get; set; }
        public int AmountOfStudents { get; set; }
        virtual public List<PairToFGroup> PairToFGroups { get; set; }
        virtual public CourseProgram CourseProgram { get; set; }
    }
}
