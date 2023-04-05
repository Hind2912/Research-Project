using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusicBoxProj.Data;
using MusicBoxProj.Models;
using MusicBoxProj.ViewModel;

namespace MusicBoxProj.Controllers
{
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Songs

        
        public async Task<IActionResult> Index( string searchString)
        {
            if (_context.Songs == null)
            {
                return NotFound();
            }

            var Songs = from s in _context.Songs.Include(s => s.Album).Include(s => s.Band)
                        select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                Songs = Songs.Where(ss => ss.SongName!.Contains(searchString) || ss.SongName.Contains(searchString));
            }

            return View(await Songs.ToListAsync());
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Band)
                .FirstOrDefaultAsync(m => m.SongId == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "AlbumName");
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName");
            return View();



        }

        // POST: Songs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SongId,SongName,AlbumId,BandId,SongDuration,SongFilePath")] Song song)
        {
            if (ModelState.IsValid)
            {
                _context.Add(song);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "AlbumName");
            ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName");
            return View(song);
            

           
        }

        // GET: Songs/Edit/5


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

          

            var song = await _context.Songs
                //.Include(s=>s.ListOfPlayLists)
                .FirstOrDefaultAsync(s => s.SongId == id);
            if (song == null)
            {
                return NotFound();
            }
            SongEditVM vm = new SongEditVM();
            vm.SongId = song.SongId;
            vm.SongName = song.SongName;
            vm.AlbumId = song.AlbumId;
            vm.BandId= song.BandId;
            vm.SongDuration= song.SongDuration;
            vm.SongFilePath= song.SongFilePath;

            if (song.ListOfPlayLists != null)
            {
                vm.PlayListIDs = song.ListOfPlayLists.Select(sg => sg.PlayListId).ToArray();
            }
            vm.AlbumSelectList  = new SelectList(_context.Albums, "AlbumId", "AlbumName");
           vm.BandSelectList  = new SelectList(_context.Bands, "BandId", "BandName");
            return View(vm);


            //ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "AlbumId", song.AlbumId);
            //ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", song.BandId);
            //return View(song);
        }
        // POST: Songs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,SongEditVM vm)
        {
            if (id != vm.SongId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Song song = new Song()
                {
                    SongId = vm.SongId,
                    SongName = vm.SongName,
                    AlbumId = vm.AlbumId,
                    BandId = vm.BandId,
                    SongDuration = vm.SongDuration,
                    SongFilePath= vm.SongFilePath,


            };

                try
                {
                    _context.Update(song);
                    await _context.SaveChangesAsync();
                    
                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongExists(song.SongId))
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
            //ViewData["AlbumId"] = new SelectList(_context.Albums, "AlbumId", "AlbumId", song.AlbumId);
            //ViewData["BandId"] = new SelectList(_context.Bands, "BandId", "BandName", song.BandId);
            //return View(song);

            vm.AlbumSelectList = new SelectList(_context.Albums, "AlbumId", "AlbumName");
            vm.BandSelectList = new SelectList(_context.Bands, "BandId", "BandName");
            return View(vm);



        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Songs == null)
            {
                return NotFound();
            }

            var song = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Band)
                .FirstOrDefaultAsync(m => m.SongId == id);
            if (song == null)
            {
                return NotFound();
            }

            return View(song);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Songs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Songs'  is null.");
            }
            var song = await _context.Songs.FindAsync(id);
            if (song != null)
            {
                _context.Songs.Remove(song);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongExists(int id)
        {
          return _context.Songs.Any(e => e.SongId == id);
        }
    }
}
