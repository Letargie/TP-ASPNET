using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TP_ASPNET.Models {
    public class Label {

        public Guid Id { get; set; }
        public string Text { get; set; }
    }
}