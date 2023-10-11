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

                }
                catch (Exception ex)
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

        [HttpGet]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Photo newPhoto)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", newPhoto);
            }

            using(PhotoPortfolioContext db = new PhotoPortfolioContext())
            {
                db.Photos.Add(newPhoto);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            using(PhotoPortfolioContext db = new PhotoPortfolioContext())
            {
                Photo? photoToEdit = db.Photos.Where(photo => photo.Id == id).FirstOrDefault();

                if(photoToEdit == null)
                {
                    return NotFound("La foto che voui modificare non è stata trovata");
                }
                else
                {
                    return View("Update", photoToEdit);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id,Photo ModifiedPhoto)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", ModifiedPhoto);
            }

            using (PhotoPortfolioContext db = new PhotoPortfolioContext())
            {
                Photo photoToUpdate = db.Photos.Where(photo => photo.Id == id).FirstOrDefault();

                if(photoToUpdate != null)
                {
                    photoToUpdate.Title = ModifiedPhoto.Title;
                    photoToUpdate.Description = ModifiedPhoto.Description;
                    photoToUpdate.PathImage = ModifiedPhoto.PathImage;

                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound("Mi dispiace, la foto da aggiornare non è stata trovata");
                }
            }



        }
    }
}
