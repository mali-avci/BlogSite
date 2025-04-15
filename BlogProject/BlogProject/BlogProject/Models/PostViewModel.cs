﻿using BlogProject.Entity;

namespace BlogProject.Models
{
    public class PostViewModel
    {
        public List<Post> Posts { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
