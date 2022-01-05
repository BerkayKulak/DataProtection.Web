using System;
using System.Collections.Generic;

#nullable disable

namespace DataProtection.Web.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Province { get; set; }
        public string PostCode { get; set; }
        public int? HouseId { get; set; }


    }
}
