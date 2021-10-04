using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class Task
    {
        [Required]
        public int id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        public User AssignedTo { get; set; }

        public string Description { get; set; }

        [Required]
        public string State { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
