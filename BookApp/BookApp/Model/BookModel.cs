using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookApp.Model
{
    public class BookModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bat buoc nhap")]

        public string Title { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }
    }
}
