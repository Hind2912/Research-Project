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
            var Album = await _context.Albums
                .Include(a => a.Band)
                .Include(b => b.ListOfSongs)
                .Include(a => a.ListOfGenres)
                .ThenInclude(a => a.Genre)
                .FirstOrDefaultAsync(m => m.AlbumId == id);

            if (Album == null)
            {
                return NotFound();
            }

            vm.AlbumId = Album.AlbumId;
            vm.AlbumName = Album.AlbumName;
            vm.ReleaseDate = Album.ReleaseDate;
            vm.Band = Album.Band;
            

            foreach(var AlbumGenre in Album.ListOfGenres)
            {
                vm.Genres.Add(AlbumGenre.Genre);
            }

            foreach(var SongsList in Album.ListOfSongs )
            {
                vm.Songs.Add(SongsList);
            }
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
        public async Task<IActionResult> Create (AlbumCreateVM vm)
        {
            if (ModelState.IsValid)
            {

                Album album = new Album()
                {
                    ReleaseDate = vm.ReleaseDate,
                    BandId = vm.BandId,
                    AlbumName = vm.AlbumName
                    
                };
                _context.Add(album);
                await _context.SaveChangesAsync();

                if (vm.GenresIds != null)
                {
                    List<AlbumGenre> albumGenres= new List<AlbumGenre>();

                    foreach (int id in vm.GenresIds)
                    {
                        albumGenres.Add(new AlbumGenre() { AlbumId = album.AlbumId,GenreId = id });
                    }


                    _context.AlbumGenre.AddRange(albumGenres);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
            vm.BandSelectList = new SelectList(_context.Bands, "BandId", "BandName");
            vm.GenreSelectList = new MultiSelectList(_context.Genres, "GenreId", "GenreName");
            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // GET: Albums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Albums == null)
            {
                return NotFound();
            }

            var album = await _context.Albums
                .Include(b => b.ListOfGenres)
                .Include(b => b.ListOfSongs)
                .FirstOrDefaultAsync(b => b.AlbumId == id);
            if (album == null)
            {
                return NotFound();
            }

            AlbumEditVM vm = new AlbumEditVM();
            vm.AlbumId = album.AlbumId;
            vm.AlbumName = album.AlbumName;
            vm.BandId = album.BandId;
            //vm.SongName = album.

            if (album.ListOfGenres != null)
            {
                vm.GenreIds = album.ListOfGenres.Select(a => a.GenreId).ToArray();
            }
            vm.BandSelectList = new SelectList(_context.Bands, "BandId", "BandName");
            vm.GenreSelectList = new MultiSelectList(_context.Genres, "GenreId", "GenreName");
            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AlbumEditVM vm)
        {
            if (id != vm.AlbumId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Album album = new Album()
                {
                    AlbumId = vm.AlbumId,
                    AlbumName = vm.AlbumName,
                    BandId = vm.BandId,
                    ReleaseDate = vm.ReleaseDate
                };
                try
                {
                    _context.Update(album);
                    await _context.SaveChangesAsync();

                    List<AlbumGenre> oldAlbumGenres = _context.AlbumGenre.Where(ag => ag.AlbumId == vm.AlbumId).ToList();

                    _context.AlbumGenre.RemoveRange(oldAlbumGenres);

                    if (vm.GenreIds != null)
                    {
                        List<AlbumGenre> newAlbumGenre = new List<AlbumGenre>();

                        foreach (int gid in vm.GenreIds)
                        {
                            newAlbumGenre.Add(new AlbumGenre() { AlbumId = album.AlbumId, GenreId = gid });
                        }
                        _context.AlbumGenre.AddRange(newAlbumGenre);
                        await _context.SaveChangesAsync();
                    }
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
            vm.BandSelectList = new SelectList(_context.Bands, "BandId", "BandName");
            vm.GenreSelectList = new MultiSelectList(_context.Genres, "GenreId", "GenreName");
            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
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
