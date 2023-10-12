using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers
{
    public class MessageController : Controller
    {

        private PhotoPortfolioContext _MyDb;

        public MessageController(PhotoPortfolioContext myDb)
        {
            this._MyDb = myDb;
        }

        public IActionResult Index()
        {
            List<Message>? messages = _MyDb.Messages.ToList();

            if(messages == null || messages.Count == 0)
            {
                return NotFound("Mi dispiace ma non sono stati trovati messaggi da mostrare");
            }
            else
            {
                return View("Index", messages);
            }
        }
    }
}
