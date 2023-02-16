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
    public class AlbumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlbumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Albums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Albums.Include(a => a.Band);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            AlbumDetailsVM vm = new AlbumDetailsVM();
            var album = await _context.Albums
                .Include(a => a.Band)
                .Include(a => a.ListOfGenres)
                .ThenInclude(g => g.Genre)
                //.Include(s => s.ListOfSongs)
                //.ThenInclude(s => s.SongFilePath)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            vm.AlbumId = album.AlbumId;
            vm.AlbumTitle = album.AlbumName;
            vm.Band = album.Band;
            vm.ReleaseDate = album.ReleaseDate;
            
            foreach(var AlbumGenre in album.ListOfGenres)
            {
                vm.Genres.Add(AlbumGenre.Genre);
            }

            //foreach(var Song in album.ListOfSongs)
            //{
            //    vm.Songs.Add(Song);
            //}

            return View(vm);
        }

        // GET: Albums/Create
        public IActionResult Create()
        {
            AlbumCreateVM vm = new AlbumCreateVM();

            vm.BandSelectList = new SelectList(_context.Bands, "BandId", "BandName");
            vm.GenreSelectList = new MultiSelectList(_context.Genres, "GenreId", "GenreName");
            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // POST: Albums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AlbumCreateVM vm)
        {
            if (ModelState.IsValid)

            {
                Album album= new Album()
                {
                    BandId = vm.BandId,
                    AlbumName = vm.AlbumTitle,
                    ReleaseDate= vm.ReleaseDate,
                    
                };
                _context.Add(album);
                await _context.SaveChangesAsync();

                if (vm.GenresIds != null)
                {
                    List<AlbumGenre> albumGenres = new List<AlbumGenre>();
                    foreach (int id in vm.GenresIds)
                    {
                        albumGenres.Add(new AlbumGenre() { AlbumId = album.AlbumId, GenreId = id });

                    }

                    _context.AlbumGenre.AddRange(albumGenres);
                    await _context.SaveChangesAsync();
                }
                    return RedirectToAction(nameof(Index));
            }
            
            vm.BandSelectList = new SelectList(_context.Bands, "BandId", "BandName",vm.BandId);
            vm.GenreSelectList = new MultiSelectList(_context.Genres, "GenreId", "GenreName", vm.GenresIds);
            
            return View(vm);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", album.BandId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlbumId,AlbumName,ReleaseDate,BandId")] Album album)
        {
            if (id != album.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumExists(album.AlbumId))
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
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", album.BandId);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(a => a.Band)
                .FirstOrDefaultAsync(m => m.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Albums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Albums'  is null.");
            }
            var album = await _context.Albums.FindAsync(id);
            if (album != null)
            {
                _context.Albums.Remove(album);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlbumExists(int id)
        {
          return _context.Albums.Any(e => e.AlbumId == id);
        }
    }
}
