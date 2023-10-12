using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using net_il_mio_fotoalbum.Database;
using net_il_mio_fotoalbum.Models;

namespace net_il_mio_fotoalbum.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MessageAPIController : ControllerBase
    {
        private PhotoPortfolioContext _MyDb;

        public MessageAPIController(PhotoPortfolioContext mydb)
        {
            this._MyDb = mydb;
        }


        [HttpPost]
        public IActionResult CreateMessage([FromBody] Message message)
        {
            try
            {
                _MyDb.Messages.Add(message);

                _MyDb.SaveChanges();

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest("Mi dispiace ma qualcosa è andato storto\n" + $"Messaggio di errore: { ex.Message}");
            }
        }
    }
}
