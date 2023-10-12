using net_il_mio_fotoalbum.CustomValidationAttribute;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace net_il_mio_fotoalbum.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required(ErrorMessage ="Il campo del titolo è obbligatorio")]
        public string Title { get; set; }

        [MaxLength(500)]
        [Required(ErrorMessage ="Il campo della descrizione è obbligatorio")]
        [Column(TypeName = "text")]
        public string Description { get; set; }

        [MaxLength(300)]
        [Required(ErrorMessage ="Il campo del path dell'immagine è obbligatorio")]
        [StringLength(300, ErrorMessage ="Il path dell'immagine non può superare i 300 caratteri")]
        // custom validation
        [ValidFormatImage]
        public string? PathImage { get; set; }

        public byte[]? ImageFile { get; set; }

        public string ImageSrc => ImageFile is null ? (PathImage is null? "" : PathImage) : $"data:image/png;base64,{Convert.ToBase64String(ImageFile)}";

        public bool IsVisible { get; set; } = true;

        // relazione N:N con Category
        public List<Category>? Categories { get; set; }

        public Photo() { }

        public Photo(string title, string description, string pathImage)
        {
            this.Title = title;
            this.Description = description;
            this.PathImage = pathImage;
        }
    }
}
