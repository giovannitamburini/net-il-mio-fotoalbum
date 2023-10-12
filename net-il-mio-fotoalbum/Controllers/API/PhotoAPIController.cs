using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PhotoAPIController : ControllerBase
    {
        private PhotoPortfolioContext _myDb;

        public PhotoAPIController(PhotoPortfolioContext myDb)
        {
            _myDb = myDb;
        }


        [HttpGet]
        public IActionResult GetPhotos()
        {
                List<Photo> photos = _myDb.Photos.Include(photo => photo.Categories).ToList();

                return Ok(photos);
        }

        // punto interrogativo perchè potrei ricevere un argomento  nullo
        [HttpGet]
        public IActionResult SearchPhotoByStringInTheTitle(string? stringToSearch)
        {
            // inizializzo una nuova lista di pizze nel caso in cui la stringa di ricerca fosse vuota ("")
            List<Photo> foundedPhotos = new List<Photo>();

            // gestisco il caso in cui la stringa da cercare nel titolo sia nulla
            if (stringToSearch == null)
            {
                foundedPhotos = _myDb.Photos.Include(photo => photo.Categories).ToList();

                return Ok(foundedPhotos);
            }
            else
            {
                    foundedPhotos = _myDb.Photos.Where(photo => photo.Title.ToLower().Contains(stringToSearch.ToLower())).ToList();

                    return Ok(foundedPhotos);
            }
        }
    }
}
