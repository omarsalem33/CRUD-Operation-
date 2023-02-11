using CRUD_Operation.Models;
using CRUD_Operation.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD_Operation.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context) { _context = context; }
       
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.OrderByDescending(i => i.Rate).ToListAsync();
            return View(movies);
        }

        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieFormViewModel 
            {
                Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync()
            };
            return View(ViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid) {
                model.Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync();
                return View(model);
            }
            var files = Request.Form.Files;
            if(!files.Any())
            {
                model.Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select movie poster");
                return View(model);
            }

            var posterFormat = files.FirstOrDefault();
            var allowed = new List<string> { ".jpg", ".png" };
            if (!allowed.Contains(Path.GetExtension(posterFormat.FileName).ToLower()))
            {
                model.Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Only .png , .jpg format");
                return View(model);
            }

            if(posterFormat.Length > 1048576)
            {
                model.Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "poster cannot to be more than 1 MB");
                return View(model);
            }
            using var datastream = new MemoryStream();
            await posterFormat.CopyToAsync(datastream);
            var movies = new Movie
            {
                Title = model.Title,
                Rate = model.Rate,
                GerneId = model.GerneId,
                Year = model.Year,
                StoreLine = model.StoreLine,
                Poster = datastream.ToArray()
            };
            _context.Add(movies);
            _context.SaveChanges();
           
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(id);
            if(movie == null)
                return NotFound();
            var viewModel = new MovieFormViewModel
            {
                Id = movie.Id, 
                Title = movie.Title,
                Rate = movie.Rate,
                GerneId= movie.GerneId,
                Year = movie.Year,
                StoreLine = movie.StoreLine,
                Poster = movie.Poster,
                Gernes = await _context.Gernes.OrderBy(g => g.Name).ToListAsync()

            };
            return View("create" , viewModel);
                
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Gernes = await _context.Gernes.OrderBy(g => g.Name).ToListAsync();
                return View("Create",model);
            }

            var movie = await _context.Movies.FindAsync(model.Id);
            if (movie == null)
                return NotFound();
            var files = Request.Form.Files;
            if(files.Any())
            {
                var poster = files.FirstOrDefault();
                using var datastream = new MemoryStream();
                
                await poster.CopyToAsync(datastream);
                var posterFormat = files.FirstOrDefault();
                model.Poster = datastream.ToArray();

                var allowed = new List<string> { ".jpg", ".png" };
                if (!allowed.Contains(Path.GetExtension(posterFormat.FileName).ToLower()))
                {
                    model.Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Only .png , .jpg format");
                    return View("Create",model);
                }

                if (posterFormat.Length > 1048576)
                {
                    model.Gernes = await _context.Gernes.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "poster cannot to be more than 1 MB");
                    return View("Create",model);
                }
                
                movie.Poster = model.Poster; 
            }

            movie.Title = model.Title;
            movie.GerneId = model.GerneId;
            movie.StoreLine = model.StoreLine;
            movie.Rate = model.Rate;
            movie.Year = model.Year;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details (int? Id)
        {
            if (Id == null)
                return BadRequest();
            var movie = await _context.Movies.Include(m => m.Gerne).SingleOrDefaultAsync(m => m.Id == Id);
            if(movie == null)
                return NotFound();
            
            return View(movie);
        }


        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
                return BadRequest();
            var movie = await _context.Movies.FindAsync(Id);
            if (movie == null)
                return NotFound();

            _context.Remove(movie);
            _context.SaveChanges();
            return Ok();
        }
  
    }
}
