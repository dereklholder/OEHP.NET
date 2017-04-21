using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EdgeExpressCloudSaas.Models
{
    [Table("Inventory")]
    public class Widget
    {
        [Key]
        public int ID { get; set; }
        public string Price { get; set; }
        public string ItemName { get; set; }
    }
}