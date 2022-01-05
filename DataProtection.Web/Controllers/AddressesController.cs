using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataProtection.Web.Models;
using Microsoft.AspNetCore.DataProtection;

namespace DataProtection.Web.Controllers
{
    public class AddressesController : Controller
    {
        private readonly ExampleDb2Context _context;
        private readonly IDataProtector _dataProtector;

        public AddressesController(ExampleDb2Context context,IDataProtectionProvider dataProtectionProvider)
        {
            _context = context;
            _dataProtector = dataProtectionProvider.CreateProtector("AddressesController");
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            var adress = await _context.Addresses.ToListAsync();

            // Şifrelenen dataya ömür biçtik 
            var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();


            adress.ForEach(x =>
            {
                // 5 saniye olacak.
                x.EncryptedId = timeLimitedProtector.Protect(x.Id.ToString(), TimeSpan.FromSeconds(5));
            });
            return View(adress);
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            
            var timeLimitedProtector = _dataProtector.ToTimeLimitedDataProtector();

            int decryptedId = int.Parse(timeLimitedProtector.Unprotect(id));

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == decryptedId);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,Province,PostCode,HouseId")] Address address)
        {
            if (ModelState.IsValid)
            {
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,Province,PostCode,HouseId")] Address address)
        {
            if (id != address.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Id))
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
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Addresses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            _context.Addresses.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(int id)
        {
            return _context.Addresses.Any(e => e.Id == id);
        }
    }
}
