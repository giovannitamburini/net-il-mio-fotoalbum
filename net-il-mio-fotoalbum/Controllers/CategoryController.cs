using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class CategoryController : Controller
    {

        private PhotoPortfolioContext _MyDb;

        public CategoryController(PhotoPortfolioContext myDb)
        {
            _MyDb = myDb;
        }

        public IActionResult Index()
        {
            List<Category> categories = _MyDb.Categories.ToList();

            return View("Index", categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Category newCategory = new Category();

            return View("Create", newCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category newCategory)
        {

            if (!ModelState.IsValid)
            {
                return View("Create", newCategory);
            }

            _MyDb.Categories.Add(newCategory);

            _MyDb.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Category? categoryToEdit = _MyDb.Categories.Where(category => category.Id == id).FirstOrDefault();

            if(categoryToEdit == null)
            {
                return NotFound("La categoria che vuoi modificare non è stata trovata");
            }
            else
            {
                return View("Update", categoryToEdit);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Category modifiedCategory)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", modifiedCategory);
            }

            Category? categoryToUpdate = _MyDb.Categories.Where(category => category.Id == id).FirstOrDefault();

            if(categoryToUpdate == null)
            {
                return NotFound("Mi dispiace, la categoria da aggiornare non è stata trovata");
            }
            else
            {
                categoryToUpdate.Title = modifiedCategory.Title;

                _MyDb.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Category? categoryToDelete = _MyDb.Categories.Where(category => category.Id == id).FirstOrDefault();

            if(categoryToDelete == null)
            {
                return NotFound("Mi dispiace ma la categoria che vuoi eliminare non è stata trovata");
            }
            else
            {
                _MyDb.Categories.Remove(categoryToDelete);

                _MyDb.SaveChanges();

                return RedirectToAction("Index");
            }
        }
    }
}
