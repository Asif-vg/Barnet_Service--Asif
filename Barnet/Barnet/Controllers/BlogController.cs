using Barnet.Data;
using Barnet.Models;
using Barnet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.Controllers
{

    

    public class BlogController : Controller
    {
        private readonly AppDbContext _context;

        public BlogController(AppDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            VmBlog blog = new VmBlog()
            {
                Blogs = _context.Blogs.ToList()
            };
            return View(blog);
        }
    }
}
