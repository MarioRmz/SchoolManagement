using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

//Al igual que en CoursesMetadata se modifica el namespace conforme aparece en Student original para que 
//exista la conexion/comunicacion correspondiente
namespace SchoolManagement.Models
{
    public class StudentMetadata
    {
        //Se le agregan las validaciones a los campos, esta vez con Display para que muestre nombres personalizados
        [StringLength(50)]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Fecha de Inscripcion")]
        public Nullable<System.DateTime> EnrollmentDate { get; set; }

        [StringLength(50)]
        [Display(Name = "Segundo Nombre")]
        public string MiddleName { get; set; }
    }

    //Hacemos referencia a nuestra clase metadata para la comunicacion de validaciones y displays
    [MetadataType(typeof(StudentMetadata))]
    public partial class Student
    {

    }
}