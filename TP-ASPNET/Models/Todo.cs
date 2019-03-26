﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TP_ASPNET.Models {
    public class Todo {

        public Guid Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastModificationDate { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}