using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Models
{
    public class EnrollmentMetadata
    {
        //NOTA: A pesar de tener una Metaclase como en los otros 2 casos, en nuestra forma de Enrollment no se estan reflejando los 
        //nombres de display que pusimos. Debido a esto se debera cambiar eso manualmente.
        [Display(Name = "Grado del Estudiante")]
        public Nullable<decimal> Grade { get; set; }

        [Display(Name = "Curso")]
        public int CourseID { get; set; }

        [Display(Name = "Curso")]
        public Course Course { get; set; }

        [Display(Name = "Estudiante")]
        public int StudentID { get; set; }

        [Display(Name = "Estudiante")]
        public Student Student { get; set; }

        [Display(Name = "Profesor")]
        public Nullable<int> LecturerID { get; set; }

        [Display(Name = "Profesor")]
        public Lecturer Lecturer { get; set; }
    }

    [MetadataType(typeof(EnrollmentMetadata))]
    public partial class Enrollme { }
}