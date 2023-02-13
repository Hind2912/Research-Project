using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicBoxProj.Data;
using MusicBoxProj.Models;

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
        public async Task<IActionResult> Index()
        {
              return View(await _context.playLists.ToListAsync());
        }

        // GET: PlayLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.playLists == null)
            {
                return NotFound();
            }

            var playList = await _context.playLists
                .FirstOrDefaultAsync(m => m.PlayListId == id);
            if (playList == null)
            {
                return NotFound();
            }

            return View(playList);
        }

        // GET: PlayLists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlayLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlayListId,PlayListName")] PlayList playList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(playList);
        }

        // GET: PlayLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.playLists == null)
            {
                return NotFound();
            }

            var playList = await _context.playLists.FindAsync(id);
            if (playList == null)
            {
                return NotFound();
            }
            return View(playList);
        }

        // POST: PlayLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlayListId,PlayListName")] PlayList playList)
        {
            if (id != playList.PlayListId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayListExists(playList.PlayListId))
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
            return View(playList);
        }

        // GET: PlayLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.playLists == null)
            {
                return NotFound();
            }

            var playList = await _context.playLists
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
            if (_context.playLists == null)
            {
                return Problem("Entity set 'ApplicationDbContext.playLists'  is null.");
            }
            var playList = await _context.playLists.FindAsync(id);
            if (playList != null)
            {
                _context.playLists.Remove(playList);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayListExists(int id)
        {
          return _context.playLists.Any(e => e.PlayListId == id);
        }
    }
}
