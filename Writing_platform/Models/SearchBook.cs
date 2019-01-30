using System.Collections.Generic;

namespace Writing_platform.Models
{
    public class SearchBook
    {
        public string AuthorName { get; set; }
        public string BookName { get; set; }
        public List<EnumModel> Genres { get; set; }
        public bool IsTopByGenre { get; set; }

        public List<Book> Result { get; set; }
        
    }
}