using CRUD_Operation.Models;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRUD_Operation.ViewModel
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(250)]
        public string Title { get; set; }
        // int and double required by defulte
        public int Year { get; set; }
        [Range(1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2500)]
        public string StoreLine { get; set; }
        [Display(Name = "Select Poster..... ")]
        public byte[] Poster { get; set; }
        [Display(Name ="Gerne")]
        public byte GerneId { get; set; }

        public IEnumerable<Gerne> Gernes { get; set; }
    }
}
