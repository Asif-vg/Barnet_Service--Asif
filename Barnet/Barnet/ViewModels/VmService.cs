using Barnet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.ViewModels
{
    public class VmService :VmLayout
    {
        public Banner Banner { get; set; }
        public List<Service> Service { get; set; }
        public List<ServiceCategory> ServiceCategories { get; set; }
        public List<Feedback> Feedbacks { get; set; }

    }
}
