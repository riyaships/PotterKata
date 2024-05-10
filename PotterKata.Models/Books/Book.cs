using System.Diagnostics;

namespace PotterKata.Models.Books
{
    public class Book
    {
        public Book()
        {
            
        }
        public Book(int id, string title, int seriesId, decimal price)
        {
            Id = id;
            Title = title;
            SeriesId = seriesId;
            Price = price;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int SeriesId { get; set; }
    }

}
