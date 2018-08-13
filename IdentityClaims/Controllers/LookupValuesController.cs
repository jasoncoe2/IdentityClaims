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
    public class LookupValuesController : Controller
    {
        private readonly IdentityClaimsContext _context;

        public LookupValuesController(IdentityClaimsContext context)
        {
            _context = context;
        }

        // GET: LookupValues
        public async Task<IActionResult> Index()
        {
            var identityClaimsContext = _context.LookupValues.Include(l => l.Lookup).OrderBy(l => l.Lookup.LookupCode).ThenBy(l => l.LookupValueCode);
            return View(await identityClaimsContext.ToListAsync());
        }

        // GET: LookupValues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookupValue = await _context.LookupValues
                .Include(l => l.Lookup)
                .SingleOrDefaultAsync(m => m.LookupValueID == id);
            if (lookupValue == null)
            {
                return NotFound();
            }

            return View(lookupValue);
        }

        // GET: LookupValues/Create
        public IActionResult Create()
        {
            ViewData["LookupID"] = new SelectList(_context.Lookups.OrderBy(l => l.LookupCode), "LookupID", "LookupCode");
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", GlobalData.gY);
            return View();
        }

        // POST: LookupValues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LookupValueID,LookupID,LookupValueCode,LookupValueDesc,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,ActiveFlag")] LookupValue lookupValue)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lookupValue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LookupID"] = new SelectList(_context.Lookups, "LookupID", "LookupCode", lookupValue.LookupID);
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", lookupValue.ActiveFlag);
            return View(lookupValue);
        }

        // GET: LookupValues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookupValue = await _context.LookupValues.SingleOrDefaultAsync(m => m.LookupValueID == id);
            if (lookupValue == null)
            {
                return NotFound();
            }
            ViewData["LookupID"] = new SelectList(_context.Lookups, "LookupID", "LookupCode", lookupValue.LookupID);
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", lookupValue.ActiveFlag);
            return View(lookupValue);
        }

        // POST: LookupValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LookupValueID,LookupID,LookupValueCode,LookupValueDesc,Attribute1,Attribute2,Attribute3,Attribute4,Attribute5,ActiveFlag")] LookupValue lookupValue)
        {
            if (id != lookupValue.LookupValueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lookupValue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LookupValueExists(lookupValue.LookupValueID))
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
            ViewData["ActiveFlag"] = new SelectList(_context.LookupValues.Where(lv => lv.ActiveFlag == GlobalData.gY).Join(_context.Lookups.Where(lu => lu.ActiveFlag == GlobalData.gY && lu.LookupCode == GlobalData.YesNo), lv => lv.LookupID, lu => lu.LookupID, (lv, lu) => new { lv.Attribute1, lv.Attribute2 }), "Attribute2", "Attribute1", lookupValue.ActiveFlag);
            ViewData["LookupID"] = new SelectList(_context.Lookups, "LookupID", "ActiveFlag", lookupValue.LookupID);
            return View(lookupValue);
        }

        // GET: LookupValues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lookupValue = await _context.LookupValues
                .Include(l => l.Lookup)
                .SingleOrDefaultAsync(m => m.LookupValueID == id);
            if (lookupValue == null)
            {
                return NotFound();
            }

            return View(lookupValue);
        }

        // POST: LookupValues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lookupValue = await _context.LookupValues.SingleOrDefaultAsync(m => m.LookupValueID == id);
            _context.LookupValues.Remove(lookupValue);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LookupValueExists(int id)
        {
            return _context.LookupValues.Any(e => e.LookupValueID == id);
        }
    }
}
