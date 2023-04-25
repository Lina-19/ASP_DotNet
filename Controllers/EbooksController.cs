using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EBOOK.Data;
using EBOOK.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace EBOOK.Controllers
{
    public class EbooksController : Controller
    {
        private readonly EbookDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EbooksController(EbookDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }

        // GET: Ebooks
        [Authorize(Roles ="Administator")]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Ebooks.ToListAsync());
        }
        //GET
        [Authorize(Roles ="User,Administator")]

        public async Task<IActionResult> Index1()
        {
            return View(await _context.Ebooks.ToListAsync());
        }
        // GET: Ebooks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Ebooks == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        [Authorize(Roles = "Administator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ebooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Auteur,Description,Prix,DisplayOrder,Stock,CreatedDateTime")] Ebook ebook)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(ebook);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(ebook);
        //}
        public async Task<IActionResult> Create(Ebook eb)
        {
            string strinfFileName = UploadFile(eb);
            var book = new Ebook
            {
                Id = eb.Id,
                Auteur = eb.Auteur,
                CreatedDateTime = eb.CreatedDateTime,
                Description = eb.Description,
                DisplayOrder = eb.DisplayOrder,
                Prix = eb.Prix,
                Stock = eb.Stock,
                Title = eb.Title,
                ImageUrl = strinfFileName
            };
            _context.Ebooks.Add(book);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        //Up
        private string UploadFile(Ebook eb)
        {
            string? fileName = null;
            if (eb.BookImage != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                fileName = Guid.NewGuid().ToString() + "" + eb.BookImage.FileName;
                string filePath = Path.Combine(uploadDir, fileName);
                using (var fileStreeam = new FileStream(filePath, FileMode.Create))
                {
                    eb.BookImage.CopyTo(fileStreeam);
                }
            }
            return fileName;
        }
        // GET: Ebooks/Edit/5
        [Authorize(Roles = "Administator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Ebooks == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebooks.FindAsync(id);
            if (ebook == null)
            {
                return NotFound();
            }
            return View(ebook);
        }

        // POST: Ebooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Auteur,Description,Prix,DisplayOrder,Stock,CreatedDateTime")] Ebook ebook)
        {
            if (id != ebook.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ebook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EbookExists(ebook.Id))
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
            return View(ebook);
        }

        // GET: Ebooks/Delete/5
        [Authorize(Roles = "Administator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Ebooks == null)
            {
                return NotFound();
            }

            var ebook = await _context.Ebooks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ebook == null)
            {
                return NotFound();
            }

            return View(ebook);
        }

        // POST: Ebooks/Delete/5
        [Authorize(Roles = "Administator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Ebooks == null)
            {
                return Problem("Entity set 'EbookDbContext.Ebooks'  is null.");
            }
            var ebook = await _context.Ebooks.FindAsync(id);
            if (ebook != null)
            {
                _context.Ebooks.Remove(ebook);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EbookExists(int id)
        {
          return _context.Ebooks.Any(e => e.Id == id);
        }
    }
}
