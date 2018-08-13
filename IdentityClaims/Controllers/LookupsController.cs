using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IdentityClaims.Models;
using Microsoft.AspNetCore.Authorization;

namespace IdentityClaims.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LookupsController : Controller
    {
        private readonly IdentityClaimsContext _context;

        public LookupsController(IdentityClaimsContext context)
        {
            _context = context;
        }

        // GET: Lookups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lookups.OrderBy(l => l.LookupCode).ToListAsync());
        }

        // GET: Lookups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookup = await _context.Lookups
                .SingleOrDefaultAsync(m => m.LookupID == id);
            if (lookup == null)
            {
                return NotFound();
            }

            return View(lookup);
        }

        // GET: Lookups/Create
        public IActionResult Create()
        {
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", GlobalData.gY);
            return View();
        }

        // POST: Lookups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LookupID,LookupCode,LookupDesc,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,ActiveFlag")] Lookup lookup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lookup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", lookup.ActiveFlag);
            return View(lookup);
        }

        // GET: Lookups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookup = await _context.Lookups.SingleOrDefaultAsync(m => m.LookupID == id);
            if (lookup == null)
            {
                return NotFound();
            }
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", GlobalData.gY);
            return View(lookup);
        }

        // POST: Lookups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LookupID,LookupCode,LookupDesc,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,ActiveFlag")] Lookup lookup)
        {
            if (id != lookup.LookupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lookup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LookupExists(lookup.LookupID))
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
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", lookup.ActiveFlag);
            return View(lookup);
        }

        // GET: Lookups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookup = await _context.Lookups
                .SingleOrDefaultAsync(m => m.LookupID == id);
            if (lookup == null)
            {
                return NotFound();
            }

            return View(lookup);
        }

        // POST: Lookups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lookup = await _context.Lookups.SingleOrDefaultAsync(m => m.LookupID == id);
            _context.Lookups.Remove(lookup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LookupExists(int id)
        {
            return _context.Lookups.Any(e => e.LookupID == id);
        }
    }
}
