using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TP_ASPNET.Models {
    public class Task {

        public Guid Id { get; set; }
        public string Text { get; set; }
        public Boolean Done { get; set; }
        public Todo Todo { get; set; }
        [ForeignKey("Todo")]
        public Guid TodoGuid;
    }
}