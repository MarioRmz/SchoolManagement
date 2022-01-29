using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

//El namespace se modifica para que haga referencia al namespace original de Courses perteneciente al modelo
//Fisicamente CoursesMetadata esta en MetaClasses, pero de esta manera hacemos que haga referencia directa a Courses original
namespace SchoolManagement.Models
{
    public class CoursesMetadata
    {
        [StringLength(50)]
        public string Title { get; set; }

        [Range(1,8)]
        public int Credits { get; set; }
    }

    //Los movimientos los hacemos aqui en otro archivo alejado de Course original para que cada que se
    //actualice el modelo, no se borren nuestros cambios

    //De esta forma hacemos referencia a CoursesMetadata para las data validations y la conectamos con la clase
    //Course original que esta en el modelo
    [MetadataType(typeof(CoursesMetadata))]
    public partial class Course
    {

    }
}