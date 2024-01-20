using System.Configuration;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using CarVilla.Areas.ViewModels;
using CarVilla.DAL;
using CarVilla.Helpers;
using CarVilla.Migrations;
using CarVilla.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace CarVilla.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin,Moderator")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.products.ToListAsync();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductVm vm)
        {
            if (!ModelState.IsValid) return View();
            if (!vm.ImageFile.CheckImage())
            {
                ModelState.AddModelError("Image", "Only image file that it's size less than 3 Mb accepted.");
            }
            Product product = new Product()
            {
                Name = vm.Name,
                Price = vm.Price,
                Description = vm.Description,
                ImageUrl = vm.ImageFile.Upload(_env.WebRootPath, @"/Upload/"),
            };
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update(int id)
        {
            Product product = await _context.products.Where(c=>c.ID == id).FirstOrDefaultAsync();
            if(product == null)
            {
                return View();
            }
            UpdateProductVm vm = new UpdateProductVm()
            {
                Id = product.ID,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
            };
            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductVm vm)
        {
            Product exist = await _context.products.Where(c=>c.ID == vm.Id).FirstOrDefaultAsync();
            if(exist == null) return View(); 
            if(!ModelState.IsValid) return View();
            if (!vm.ImageFile.CheckImage())
            {
                ModelState.AddModelError("Image", "Only Image file that less than 3 MB is accepted");
            }
            Product product = new Product()
            {
                Name = vm.Name, 
                Price = vm.Price, 
                Description = vm.Description,
                ImageUrl = vm.ImageFile.Upload(_env.WebRootPath,@"/Upload/"),
            };
            await _context.SaveChangesAsync();  
            return View();
        }
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.products.Where(c => c.ID == id).FirstOrDefaultAsync();
             _context.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Product"); 
        }
    }
}
