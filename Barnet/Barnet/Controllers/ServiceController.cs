using Barnet.Data;
using Barnet.Models;
using Barnet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.Controllers
{
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            VmService servcie = new VmService()
            {
                 
                Service = _context.Services.ToList(),
                Setting = _context.Settings.FirstOrDefault(),
                Socials=_context.Socials.ToList(),
                Feedbacks = _context.Feedbacks.ToList()
               
            };

            return View(servcie);
        }
    }
}
