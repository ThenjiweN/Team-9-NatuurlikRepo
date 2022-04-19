#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NatuurlikBase.Areas.Identity.Data;
using NatuurlikBase.Models;

namespace NatuurlikBase.Controllers
{
    public class TeasController : Controller
    {
        private readonly DatabaseContext _context;

        public TeasController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Teas
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tea.ToListAsync());
        }

        // GET: Teas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tea = await _context.Tea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tea == null)
            {
                return NotFound();
            }

            return View(tea);
        }

        // GET: Teas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Tea tea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tea);
        }

        // GET: Teas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tea = await _context.Tea.FindAsync(id);
            if (tea == null)
            {
                return NotFound();
            }
            return View(tea);
        }

        // POST: Teas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Tea tea)
        {
            if (id != tea.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeaExists(tea.Id))
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
            return View(tea);
        }

        // GET: Teas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tea = await _context.Tea
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tea == null)
            {
                return NotFound();
            }

            return View(tea);
        }

        // POST: Teas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tea = await _context.Tea.FindAsync(id);
            _context.Tea.Remove(tea);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeaExists(int id)
        {
            return _context.Tea.Any(e => e.Id == id);
        }
    }
}
