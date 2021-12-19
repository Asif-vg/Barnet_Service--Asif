using Barnet.Data;
using Barnet.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Barnet.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FeedbackController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FeedbackController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Feedback> feedback = _context.Feedbacks.ToList();
            return View(feedback);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Feedback model)
        {
            if (ModelState.IsValid)
            {
                if (model.ClientImageFile.ContentType == "image/jpeg" || model.ClientImageFile.ContentType == "image/png")
                {
                    if (model.ClientImageFile.Length <= 2097152)
                    {
                        string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + model.ClientImageFile.FileName;
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.ClientImageFile.CopyTo(stream);
                        }


                        model.ClientImage = fileName;

                        _context.Feedbacks.Add(model);
                        _context.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "You can upload only less than 2 mb");
                        return View(model);
                    }

                }
                else
                {
                    ModelState.AddModelError("", "You can upload only .jpeg, .jpg and .png");
                    return View(model);
                }
            }

            return View(model);
        }

        public IActionResult Update(int? id)
        {
            Feedback model = _context.Feedbacks.FirstOrDefault(b => b.Id == id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Update(Feedback model)
        {
            if (ModelState.IsValid)
            {
                if (model.ClientImageFile != null)
                {
                    if (model.ClientImageFile.ContentType == "image/jpeg" || model.ClientImageFile.ContentType == "image/png")
                    {
                        if (model.ClientImageFile.Length <= 2097152)
                        {
                            if (!string.IsNullOrEmpty(model.ClientImage))
                            {
                                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", model.ClientImage);
                                if (System.IO.File.Exists(oldImagePath))
                                {
                                    System.IO.File.Delete(oldImagePath);
                                }
                            }

                            string fileName = Guid.NewGuid() + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + model.ClientImageFile.FileName;
                            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                model.ClientImageFile.CopyTo(stream);
                            }

                            model.ClientImage = fileName;

                        }
                        else
                        {
                            ModelState.AddModelError("", "You can upload only less than 2 mb");
                            return View(model);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "You can upload only .jpeg, .jpg and .png");
                        return View(model);
                    }
                }

                _context.Feedbacks.Update(model);
                _context.SaveChanges();
            }

            return View(model);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                HttpContext.Session.SetString("NullIdError", "Id can not be null");
                return RedirectToAction("Index");
            }

            Feedback feedback = _context.Feedbacks.Find(id);
            if (feedback == null)
            {
                HttpContext.Session.SetString("NullDataError", "Can not found the data");
                return RedirectToAction("Index");
            }




            if (!string.IsNullOrEmpty(feedback.ClientImage))
            {
                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", feedback.ClientImage);
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

            _context.Feedbacks.Remove(feedback);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
