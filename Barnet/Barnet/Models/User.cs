using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [Column(TypeName = "ntext")]
        public string Email { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
