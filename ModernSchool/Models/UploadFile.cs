using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ModernSchool.Models
{
    public class UploadFile
    {
        public int Id { get; set; }
        public int SchoolId { get; set; }
        public int IndexId { get; set; }
        public string FileName { get; set; }
        public string FileGuid { get; set; }
        public string FileExtension { get; set; }
        public int? Year { get; set; }
        public int CreatedBy { get; set; } // 1 - School    2 - Inspektor
        public DateTime CreateDate { get; set; }
        
        [ForeignKey(nameof(SchoolId))]
        public virtual School School { get; set; }
        
        [ForeignKey(nameof(IndexId))]
        public virtual Index Index { get; set; }
    }
}
