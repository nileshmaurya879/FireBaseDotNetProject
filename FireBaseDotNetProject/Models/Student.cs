﻿namespace FireBaseDotNetProject.Models
{
    public class Student
    {
        public string? Id { get; set; } // firebase unique id
        public string? Student_id { get; set; }
        public string? fullname { get; set; }
        public string? degree_title { get; set; }
        public string? address { get; set; }
        public string? phone { get; set; }
    }
}
