using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Assignment4.Core;

namespace Assignment4.Entities
{
    public class Task
    {
        [Required]
        public int Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Title { get; set; }

        public User AssignedTo { get; set; }

        public string Description { get; set; }

        [Required]
        public State State { get; set; }

        public ICollection<Tag> Tags { get; set; }
    }
}
