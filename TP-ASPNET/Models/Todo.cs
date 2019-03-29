using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TP_ASPNET.Models {
    public class Todo {

        public Guid Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime LastModificationDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public Boolean Done { get; set; }
        public User User { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual ICollection<TodoLabel> TodoLabels { get; set; }
    }
}