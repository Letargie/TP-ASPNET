using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TP_ASPNET.Models {
    public class TodoLabel{

        public Guid Id { get; set; }

        public Todo Todo { get; set; }
        [ForeignKey("Todo")] public Guid TodoGuid;

        public Label Label { get; set; }
        [ForeignKey("Label")] public Guid LabelGuid;
    }
}