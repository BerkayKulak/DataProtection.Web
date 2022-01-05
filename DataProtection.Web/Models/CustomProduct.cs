using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataProtection.Web.Models
{
    public partial class Address
    {
        [NotMapped] // burdaki property veritabanındaki bir sütun değil
        public string EncryptedId { get; set; }
    }
}
