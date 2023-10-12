using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

            Photo? foundedPhoto = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

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
            List<SelectListItem> AllCategorySelectList = new List<SelectListItem>();

            List<Category> databaseAllCategory = _myDb.Categories.ToList();

            foreach (Category category in databaseAllCategory)
            {
                AllCategorySelectList.Add(new SelectListItem { Text = category.Title, Value = category.Id.ToString() });
            }

            PhotoFormModel model = new PhotoFormModel { Photo = new Photo(), Categories = AllCategorySelectList };

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<SelectListItem> allCategoriesSelectList = new List<SelectListItem>();
                List<Category> databaseAllCategories = _myDb.Categories.ToList();

                foreach (Category category in databaseAllCategories)
                {
                    allCategoriesSelectList.Add(
                        new SelectListItem
                        {
                            Text = category.Title,
                            Value = category.Id.ToString()
                        });
                }

                data.Categories = allCategoriesSelectList;

                return View("Create", data);
            }

            data.Photo.Categories = new List<Category>();

            if (data.SelectedCategoriesId != null)
            {
                foreach (string categorySelectedId in data.SelectedCategoriesId)
                {
                    int intCategorySelectedId = int.Parse(categorySelectedId);

                    Category? categoryInDb = _myDb.Categories.Where(category => category.Id == intCategorySelectedId).FirstOrDefault();

                    if (categoryInDb != null)
                    {
                        data.Photo.Categories.Add(categoryInDb);
                    }
                }
            }

            _myDb.Photos.Add(data.Photo);
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
