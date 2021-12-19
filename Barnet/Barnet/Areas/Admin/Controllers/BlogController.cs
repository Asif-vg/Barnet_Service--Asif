using Barnet.Data;
using Barnet.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Blog> blog = _context.Blogs.OrderByDescending(o => o.CreatedDate).Include(bc => bc.BlogCategory).ToList();

            return View(blog);
        }


        public IActionResult Create()
        {
            ViewBag.Category = _context.BlogCategories.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile.ContentType== "image/jpeg" || model.ImageFile.ContentType == "image/png")
                {
                    if (model.ImageFile.Length <= 2097152)
                    {
                        string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + model.ImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream =new FileStream(filePath, FileMode.Create))
                        {
                            model.ImageFile.CopyTo(stream);
                        }

                        model.Image = fileName;
                        model.CreatedDate = DateTime.Now;
                        //Title = model.Title;
                        model.UserId = 1;

                        _context.Blogs.Add(model);
                        _context.SaveChanges();

                        return RedirectToAction("Index");

                    }
                    else
                    {
                        ModelState.AddModelError("", "You can upload only less than 2 mb");
                        ViewBag.Category = _context.BlogCategories.ToList();
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "You can upload only .jpeg, .jpg and .png");
                    ViewBag.Category = _context.BlogCategories.ToList();
                    return View(model);
                }

            }

            ViewBag.Category = _context.BlogCategories.ToList();

            return View(model);
        }

        public IActionResult Update(int? id)
        {
            Blog model = _context.Blogs.FirstOrDefault(b => b.Id == id);
            ViewBag.Category = _context.BlogCategories.ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Blog model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageFile != null)
                {
                    if (model.ImageFile.ContentType == "image/jpeg" || model.ImageFile.ContentType == "image/png")
                    {
                        if (model.ImageFile.Length <= 2097152)
                        {
                            if (!string.IsNullOrEmpty(model.Image))
                            {
                                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", model.Image);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }

                            string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + model.ImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                model.ImageFile.CopyTo(stream);
                            }

                            model.Image = fileName;
                        }
                        else
                        {
                            ModelState.AddModelError("", "You can upload only less than 2 mb.");
                            ViewBag.Category = _context.BlogCategories.ToList();
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "You can upload only .jpeg, .jpg and .png");
                        ViewBag.Category = _context.BlogCategories.ToList();
                        return View(model);
                    }
                }


                _context.Blogs.Update(model);
                _context.SaveChanges();

                
            }

            ViewBag.Category = _context.BlogCategories.ToList();
            return View(model);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                HttpContext.Session.SetString("NullIdError", "Id can not be null");
                return RedirectToAction("Index");
            }

            Blog blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                HttpContext.Session.SetString("NullDataError", "Can not found the data");
                return RedirectToAction("Index");
            }


            

            if (!string.IsNullOrEmpty(blog.Image))
            {
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", blog.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            //List<TagToBlog> tagToBlogs = _context.TagToBlogs.Where(t=>t.BlogId==id).ToList();
            //foreach (var item in tagToBlogs)
            //{
            //    _context.TagToBlogs.Remove(item);
            //}
            //_context.SaveChanges();

            _context.Blogs.Remove(blog);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
