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
                List<Photo> photos = db.Photos.ToList();

                return View("Index", photos);
            }

        }
    }
}
