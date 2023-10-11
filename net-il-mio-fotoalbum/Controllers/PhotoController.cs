using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class PhotoController : Controller
    {
        private PhotoPortfolioContext _myDb;

        public PhotoController(PhotoPortfolioContext myDb)
        {
            this._myDb = myDb;
        }

        public IActionResult Index()
        {
            try
            {
                List<Photo> photos = _myDb.Photos.ToList();

                return View("Index", photos);

            }
            catch (Exception ex)
            {
                return NotFound($"La lista delle foto è vuota \n" +
                    $"Messaggio d'errore: {ex.Message}");
            }
        }

        public IActionResult Details(int id)
        {

            Photo? foundedPhoto = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (foundedPhoto == null)
            {
                return NotFound($"La foto con id '{id}' non è stata trovata");
            }
            else
            {
                return View("Details", foundedPhoto);
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



            _myDb.Photos.Add(newPhoto);

            _myDb.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Update(int id)
        {


            Photo? photoToEdit = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToEdit == null)
            {
                return NotFound("La foto che voui modificare non è stata trovata");
            }
            else
            {
                return View("Update", photoToEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Photo ModifiedPhoto)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", ModifiedPhoto);
            }


            Photo photoToUpdate = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToUpdate != null)
            {
                photoToUpdate.Title = ModifiedPhoto.Title;
                photoToUpdate.Description = ModifiedPhoto.Description;
                photoToUpdate.PathImage = ModifiedPhoto.PathImage;

                _myDb.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace, la foto da aggiornare non è stata trovata");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {


            Photo? photoToDelete = _myDb.Photos.Where(photo => photo.Id == id).FirstOrDefault();

            if (photoToDelete != null)
            {
                _myDb.Photos.Remove(photoToDelete);

                _myDb.SaveChanges();

                return RedirectToAction("index");
            }
            else
            {
                return NotFound("Mi dispiace ma l'articolo che vuoi eliminare non è stato trovato");
            }
        }
    }
}
