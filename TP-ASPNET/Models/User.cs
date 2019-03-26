using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace TP_ASPNET.Models {
    [Table("AspNetUsers")]
    public class User : IdentityUser {

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime InscriptionDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastConnectionDate { get; set; }
    }
}