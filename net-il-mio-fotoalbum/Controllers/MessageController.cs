using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{

    [Authorize(Roles = "ADMIN")]
    public class MessageController : Controller
    {

        private PhotoPortfolioContext _MyDb;

        public MessageController(PhotoPortfolioContext myDb)
        {
            this._MyDb = myDb;
        }

        public IActionResult Index()
        {
            List<Message> messages = _MyDb.Messages.ToList();

            return View("Index", messages);
        }
    }
}
