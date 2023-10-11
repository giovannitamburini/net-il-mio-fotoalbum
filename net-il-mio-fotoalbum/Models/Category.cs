using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il titolo della categoria è obbligatorio")]
        [MaxLength(100, ErrorMessage ="Il titolo della categoria non può essere superiore ai 100 caratteri")]
        public string Title { get; set; }

        // relazione N:N con Photo
        public List<Photo>? Photos { get; set; }

        public Category() { }
    }
}