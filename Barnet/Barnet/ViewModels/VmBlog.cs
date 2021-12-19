using Barnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.ViewModels
{
    public class VmBlog :VmLayout
    {
        public List<Blog> Blogs { get; set; }
    }
}
