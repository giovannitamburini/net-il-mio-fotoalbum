using System.ComponentModel.DataAnnotations;

namespace net_il_mio_fotoalbum.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo del messaggio è obbligatorio")]
        [EmailAddress(ErrorMessage ="Devi inserire un indirizzo mail valido")]
        public string Mail { get; set; }

        [Required(ErrorMessage = "Il campo del messaggio è obbligatorio")]
        public string MessageText { get; set; }

        public Message() { }

        public Message(string mail, string messageText)
        {
            this.Mail = mail;
            this.MessageText = messageText;
        }
    }
}
