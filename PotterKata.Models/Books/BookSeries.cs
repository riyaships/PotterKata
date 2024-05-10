using System.Collections.Generic;

namespace PotterKata.Models.Books
{
    public class BookSeries
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Book> Books { get; set; }
    }

}
