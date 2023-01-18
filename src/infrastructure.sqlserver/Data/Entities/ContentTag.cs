using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.sqlserver.Data.Entities
{
    public class ContentTag
    {
        public int ContentTagId { get; set; }


        [MaxLength(50)]
        public string Name { get; set; }


        [MaxLength(400)]
        public string Description { get; set; }
    }
}
