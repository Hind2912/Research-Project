using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicBoxProj.Data;
using MusicBoxProj.Models;
using MusicBoxProj.ViewModel;

namespace MusicBoxProj.Controllers
{
    public class BandsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BandsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bands
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.Bands == null)
            {
                return NotFound();
            }

            var band = from b in _context.Bands select b;
            if (!String.IsNullOrEmpty(searchString))
            {
                band = band.Where(bb => bb.BandName!.Contains(searchString) || bb.BandName.Contains(searchString));
            }

            return View(await band.ToListAsync());
        }
        // GET: Bands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bands == null)
            {
                return NotFound();
            }

            BandDetailsVM vm = new BandDetailsVM();
            var band = await _context.Bands
                .Include(b => b.ListOfSongs)
                .Include(b => b.ListOfAlbums)
                .FirstOrDefaultAsync(m => m.BandId == id);
            if (band == null)
            {
                return NotFound();
            }

            vm.BandId= band.BandId;
            vm.BandName = band.BandName;
            
            foreach(var SongList in band.ListOfSongs)
            {
                vm.Songs.Add(SongList);

            }
            foreach (var AlbumList in band.ListOfAlbums)
            {
                vm.Albums.Add(AlbumList);

            }

            return View(vm);
        }

        // GET: Bands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BandId,BandName")] Band band)
        {
            if (ModelState.IsValid)
            {
                _context.Add(band);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(band);
        }

        // GET: Bands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bands == null)
            {
                return NotFound();
            }

            var band = await _context.Bands.FindAsync(id);
            if (band == null)
            {
                return NotFound();
            }
            return View(band);
        }

        // POST: Bands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BandId,BandName")] Band band)
        {
            if (id != band.BandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(band);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BandExists(band.BandId))
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
            return View(band);
        }

        // GET: Bands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bands == null)
            {
                return NotFound();
            }

            var band = await _context.Bands
                .FirstOrDefaultAsync(m => m.BandId == id);
            if (band == null)
            {
                return NotFound();
            }

            return View(band);
        }

        // POST: Bands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bands == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Bands'  is null.");
            }
            var band = await _context.Bands.FindAsync(id);
            if (band != null)
            {
                _context.Bands.Remove(band);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BandExists(int id)
        {
          return _context.Bands.Any(e => e.BandId == id);
        }
    }
}
