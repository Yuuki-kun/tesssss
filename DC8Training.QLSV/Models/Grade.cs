using DC8Training.QLSV.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DC8Training.QLSV.Models
{
    internal class Grade
    {
        /// <summary>
        /// UUID
        /// </summary>
        public string Id { get; set; }



        public GradingScaleType GradingScaleType { get; set; } = GradingScaleType.Point10;
        public LetterGrade? LetterGrade { get; set; }

        /// <summary>
        /// In Grade10
        /// </summary>
        public float? Point10 { get; set; }
        public float? Point4 { get; set; }

        public int StudentID { get; set; }
        public int ClassID { get; set; }
    }
}
