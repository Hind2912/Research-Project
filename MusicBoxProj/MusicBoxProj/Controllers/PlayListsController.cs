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
    public class PlayListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PlayLists
        public async Task<IActionResult> Index(string searchString)
        {
            if (_context.PlayList == null)
            {
                return NotFound();
            }

            var pList = from p in _context.PlayList select p;
            if (!String.IsNullOrEmpty(searchString))
            {
                pList = pList.Where(pl => pl.PlayListName!.Contains(searchString) || pl.PlayListName.Contains(searchString));
            }

            return View(await pList.ToListAsync());
        }

        // GET: PlayLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlayList == null)
            {
                return NotFound();
            }
            PlayListDetailsVM vm = new PlayListDetailsVM();
            var playList = await _context.PlayList
                .Include(p => p.ListOfSongs)
                .ThenInclude(p => p.Song)
                .FirstOrDefaultAsync(m => m.PlayListId == id);

            if (playList == null)
            {
                return NotFound();
            }
            vm.PlayListId = playList.PlayListId;
            vm.PlayListName = playList.PlayListName;



            foreach (var SongList in playList.ListOfSongs)
            {
                vm.Songs.Add(SongList.Song);
            }
            return View(vm);
        }

        // GET: PlayLists/Create
        public IActionResult Create()
        {
            PlayListCreateVM vm = new PlayListCreateVM();
            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // POST: PlayLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PlayListCreateVM vm)
        {
            if (ModelState.IsValid)
            {
                PlayList playList = new PlayList()
                {
                    PlayListName = vm.PlayListName,
                };

                await _context.PlayList.AddAsync(playList);
                await _context.SaveChangesAsync();

                List<PlayListSong> listOfSongs = new List<PlayListSong>();

                foreach (int songID in vm.SelectedSongIDs)
                {
                    listOfSongs.Add(new PlayListSong()
                    {
                        PlayListId = playList.PlayListId,
                        SongId = songID
                    });
                }

                await _context.PlayListSong.AddRangeAsync(listOfSongs);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // GET: PlayLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlayList == null)
            {
                return NotFound();
            }

            var playList = await _context.PlayList
                .Include(p => p.ListOfSongs)
                .FirstOrDefaultAsync(p => p.PlayListId == id);
            if (playList == null)
            {
                return NotFound();
            }
            PlayListEditVM vm = new PlayListEditVM();
            vm.PlayListId = playList.PlayListId;
            vm.PlayListName = playList.PlayListName;

            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // POST: PlayLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PlayListEditVM vm)
        {
            if (id != vm.PlayListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                PlayList pl = new PlayList()
                {
                    PlayListId = vm.PlayListId,
                    PlayListName = vm.PlayListName,

                };
                try
                {
                    _context.Update(pl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayListExists(pl.PlayListId))
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
            vm.SongSelectList = new MultiSelectList(_context.Songs, "SongId", "SongName");
            return View(vm);
        }

        // GET: PlayLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlayList == null)
            {
                return NotFound();
            }

            var playList = await _context.PlayList
                .FirstOrDefaultAsync(m => m.PlayListId == id);
            if (playList == null)
            {
                return NotFound();
            }

            return View(playList);
        }

        // POST: PlayLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlayList == null)
            {
                return Problem("Entity set 'ApplicationDbContext.playLists'  is null.");
            }
            var playList = await _context.PlayList.FindAsync(id);
            if (playList != null)
            {
                _context.PlayList.Remove(playList);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayListExists(int id)
        {
            return _context.PlayList.Any(e => e.PlayListId == id);
        }
    }
}
