using System.ComponentModel.DataAnnotations;

namespace CRUD_Operation.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Title { get; set; }
        // int and double required by defulte
        public int  Year { get; set; }
        public double Rate { get; set; }
        
        [Required , MaxLength(2500)]
        public string StoreLine { get; set; }
        [Required]
        public byte[] Poster { get; set; } 

        public byte GerneId { get; set; }
        public Gerne Gerne { get; set;}
    }

}
