﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_and_DataBase.Models
{
    public class Users
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Type { get; set; }
        public bool ISDeleted { get; set; } = false;


        [ForeignKey("Car")]
        public int? CarID { get; set; }
        public virtual Car Car { get; set; }
        public virtual ICollection<ImportReciept> ImportReciepts { get; set; } = new HashSet<ImportReciept>();
        public virtual ICollection<ExportReciept> ExportReciepts { get; set; } = new HashSet<ExportReciept>();

    }
}
