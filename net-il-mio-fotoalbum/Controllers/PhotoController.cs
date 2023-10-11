using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        public IActionResult Index()
        {
            using (PhotoPortfolioContext db = new PhotoPortfolioContext())
            {
                try
                {
                    List<Photo> photos = db.Photos.ToList();

                    return View("Index", photos);

                }catch (Exception ex)
                {
                    return NotFound($"La lista delle foto è vuota \n" +
                        $"Messaggio d'errore: {ex.Message}");
                }
            }
        }

        public IActionResult Details(int id)
        {
            using (PhotoPortfolioContext db = new PhotoPortfolioContext())
            {
                Photo? foundedPhoto = db.Photos.Where(photo => photo.Id == id).FirstOrDefault();

                if (foundedPhoto == null)
                {
                    return NotFound($"La foto con id '{id}' non è stata trovata");
                }
                else
                {
                    return View("Details", foundedPhoto);
                }
            }
        }
    }
}
