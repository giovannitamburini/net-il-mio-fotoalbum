namespace net_il_mio_fotoalbum.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }

        // relazione N:N con Photo
        public List<Photo> Photos { get; set; }

        public Category() { }
    }
}
