using Microsoft.AspNetCore.Mvc.Rendering;

namespace net_il_mio_fotoalbum.Models
{
    public class PhotoFormModel
    {
        public Photo Photo { get; set; }

        public IFormFile? ImageFormFile { get; set; }

        // per gestire la select multipla nel form
        public List<SelectListItem>? Categories { get; set; }
        public List<string>? SelectedCategoriesId { get; set; } 
    }
}
