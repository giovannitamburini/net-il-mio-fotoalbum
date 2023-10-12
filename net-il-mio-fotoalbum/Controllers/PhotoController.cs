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

        // ---------------------------------------------------------
        // INDEX
        // ---------------------------------------------------------

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

        // ---------------------------------------------------------
        // SEARCH
        // ---------------------------------------------------------

        [HttpGet]
        public IActionResult Search(string searchString)
        {
            try
            {
                List<Photo> photos;

                if (!string.IsNullOrEmpty(searchString))
                {
                    // query per trovare le foto che corrispondono alla stringa di ricerca
                    photos = _myDb.Photos.Where(p => p.Title.Contains(searchString)).ToList();
                }
                else
                {
                    // Se il campo di ricerca è vuoto, ottieni tutte le foto
                    photos = _myDb.Photos.ToList();
                }

                return View("Index", photos);
            }
            catch (Exception ex)
            {
                return NotFound($"La lista delle foto è vuota \n" +
                    $"Messaggio d'errore: {ex.Message}");
            }
        }

        // ---------------------------------------------------------
        // DETAILS
        // ---------------------------------------------------------

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


        // ---------------------------------------------------------
        // CREATE
        // ---------------------------------------------------------

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


        // ---------------------------------------------------------
        // UPDATE
        // ---------------------------------------------------------

        [HttpGet]
        public IActionResult Update(int id)
        {

            Photo? photoToEdit = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToEdit == null)
            {
                return NotFound("La foto che voui modificare non è stata trovata");
            }
            else
            {
                List<Category> dbCategoriesList = _myDb.Categories.ToList();

                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach(Category category in dbCategoriesList)
                {
                    selectListItems.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Title, Selected = photoToEdit.Categories.Any(categoryAssociated => categoryAssociated.Id == category.Id) });
                }

                PhotoFormModel model = new PhotoFormModel { Photo = photoToEdit, Categories = selectListItems };

                return View("Update", model);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PhotoFormModel data)
        {
            if (!ModelState.IsValid)
            {
                List<Category> dbCategoriesList = _myDb.Categories.ToList();

                List<SelectListItem> selectListItems = new List<SelectListItem>();

                foreach (Category category in dbCategoriesList)
                {
                    selectListItems.Add(new SelectListItem { Value = category.Id.ToString(), Text = category.Title});
                }

                data.Categories = selectListItems;

                return View("Update", data);
            }


            Photo? photoToUpdate = _myDb.Photos.Where(photo => photo.Id == id).Include(photo => photo.Categories).FirstOrDefault();

            if (photoToUpdate != null)
            {

                if(photoToUpdate.Categories != null)
                {
                    photoToUpdate.Categories.Clear();
                }

                photoToUpdate.Title = data.Photo.Title;
                photoToUpdate.Description = data.Photo.Description;
                photoToUpdate.PathImage = data.Photo.PathImage;

                if(data.SelectedCategoriesId != null)
                {
                    foreach(string categorySelectedId in data.SelectedCategoriesId)
                    {
                        int intCategoryId = int.Parse(categorySelectedId);

                        Category? categoryInDb = _myDb.Categories.Where(category => category.Id == intCategoryId).FirstOrDefault();

                        if(categoryInDb != null)
                        {
                            photoToUpdate.Categories.Add(categoryInDb);
                        }
                    }
                }

                _myDb.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return NotFound("Mi dispiace, la foto da aggiornare non è stata trovata");
            }
        }


        // ---------------------------------------------------------
        // DELETE
        // ---------------------------------------------------------

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
