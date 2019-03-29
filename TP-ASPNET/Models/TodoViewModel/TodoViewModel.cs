using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TP_ASPNET.Controllers;
using TP_ASPNET.Models;

namespace ASPNetCoreIdentity.Models.ConnectionViewModels {
    public class TodoViewModel {
        [Required]
        public Todo Todo { get; set; }
        
        [Required]
        public ICollection<Label> Labels { get; set; }
    }
}