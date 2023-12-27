using BulkeyWebRazor.Data;
using BulkeyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkeyWebRazor.Pages.Categories
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }
        public CreateModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {

        }
        public IActionResult OnPost() // This Method through Insert The Data in to the database.
        {
            _db.Categories.Add(Category);
            _db.SaveChanges();
            TempData["success"] = "Category created a Successfully";
            return RedirectToPage("Index");
        }
    }
}
