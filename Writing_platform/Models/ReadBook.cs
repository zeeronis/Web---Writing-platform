using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Writing_platform.Models
{
    public class ReadBook
    {
        public string AuthorName { get; set; }
        public int BookID { get; set; }
        public string BookName { get; set; }
        public string BookContent { get; set; }
        public DateTime? BookPublishDate { get; set; }
        public List<GenreType> Genres { get; set; }
        public float Rating { get; set; }

        public string[] PostsUserName { get; set; }
        public string[] PostsContent { get; set; }

        [DisplayName("Leave your review")]
        public string NewPostContent { get; set; }

        [DisplayName("Rating")]
        [Range(0, 100, ErrorMessage = "{0} must be between {1} and {2}")]
        public float NewRating { get; set; } = 0;
    }
}