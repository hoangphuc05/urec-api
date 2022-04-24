
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UREC_api
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Uid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        //public string StudentID
    }
}
