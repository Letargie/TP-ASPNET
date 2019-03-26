using System;
using System.ComponentModel.DataAnnotations;

namespace TP_ASPNET.Models {
    public class User {

        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime InscriptionDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastConnectionDate { get; set; }
    }
}