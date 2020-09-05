namespace BookStore_UI.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Years { get; set; }
        public string ISBN { get; set; }
        public string Summary { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }


        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}