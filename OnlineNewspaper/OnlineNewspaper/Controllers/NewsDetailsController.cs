using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using OnlineNewspaper.Data;
using OnlineNewspaper.Models;

namespace OnlineNewspaper.Controllers
{
    public class NewsDetailsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IFileProvider fileProvider;
        private readonly IHostingEnvironment hostingEnvironment;


        public NewsDetailsController(ApplicationDbContext context,
                          IFileProvider fileprovider, IHostingEnvironment env)
        {
            _context = context;
            fileProvider = fileprovider;
            hostingEnvironment = env;
        }

        // GET: NewsDetails
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NewsDetails.Include(n => n.NewsCategory);
            return View(await applicationDbContext.ToListAsync());
        }


        public async Task<IActionResult> ManageNews()
        {
            var applicationDbContext = _context.NewsDetails.Include(n => n.NewsCategory);
            return View(await applicationDbContext.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> NationalNews()
        {
            var national = from n in _context.NewsDetails select n;
            national = national.Where(s => s.NewsCategoryID == 5);
            return View(await national.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> Sports()
        {
            var national = from n in _context.NewsDetails select n;
            national = national.Where(s => s.NewsCategoryID == 3);
            return View(await national.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> International()
        {
            var national = from n in _context.NewsDetails select n;
            national = national.Where(s => s.NewsCategoryID == 4);
            return View(await national.ToListAsync());
        }


        // GET: NewsDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsDetails = await _context.NewsDetails
                .Include(n => n.NewsCategory)
                .FirstOrDefaultAsync(m => m.NewsDetailsID == id);
            if (newsDetails == null)
            {
                return NotFound();
            }

            return View(newsDetails);
        }


        [Authorize(Roles ="Admin")]
        // GET: NewsDetails/Create
        public IActionResult Create()
        {
            ViewData["NewsCategoryID"] = new SelectList(_context.NewsCategory, "NewsCategoryID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsDetails newsDetails , IFormFile file)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsDetails);
                await _context.SaveChangesAsync();

                // Code to upload image if not null
                if (file != null || file.Length != 0)
                {
                    // Create a File Info 
                    FileInfo fi = new FileInfo(file.FileName);

                    // This code creates a unique file name to prevent duplications 
                    // stored at the file location
                    var newFilename = newsDetails.NewsDetailsID + "_" + String.Format("{0:d}",
                                      (DateTime.Now.Ticks / 10) % 100000000) + fi.Extension;
                    var webPath = hostingEnvironment.WebRootPath;
                    var path = Path.Combine("", webPath + @"\ImageFiles\" + newFilename);

                    // IMPORTANT: The pathToSave variable will be save on the column in the database
                    var pathToSave = @"/ImageFiles/" + newFilename;

                    // This stream the physical file to the allocate wwwroot/ImageFiles folder
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // This save the path to the record
                    newsDetails.Image = pathToSave;
                    _context.Update(newsDetails);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsDetails);
        }





        // GET: NewsDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsDetails = await _context.NewsDetails.FindAsync(id);
            if (newsDetails == null)
            {
                return NotFound();
            }
            ViewData["NewsCategoryID"] = new SelectList(_context.NewsCategory, "NewsCategoryID", "NewsCategoryID", newsDetails.NewsCategoryID);
            return View(newsDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsDetails newsDetails)
        {
            if (id != newsDetails.NewsDetailsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsDetailsExists(newsDetails.NewsDetailsID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NewsCategoryID"] = new SelectList(_context.NewsCategory, "NewsCategoryID", "NewsCategoryID", newsDetails.NewsCategoryID);
            return View(newsDetails);
        }

        // GET: NewsDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsDetails = await _context.NewsDetails
                .Include(n => n.NewsCategory)
                .FirstOrDefaultAsync(m => m.NewsDetailsID == id);
            if (newsDetails == null)
            {
                return NotFound();
            }

            return View(newsDetails);
        }

        // POST: NewsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsDetails = await _context.NewsDetails.FindAsync(id);
            _context.NewsDetails.Remove(newsDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsDetailsExists(int id)
        {
            return _context.NewsDetails.Any(e => e.NewsDetailsID == id);
        }
    }
}
