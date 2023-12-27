using BulkeyWebRazor.Data;
using BulkeyWebRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkeyWebRazor.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public List<Category> CategoryList { get; set; }
        public IndexModel(ApplicationDbContext db) 
        {
            _db = db;
        }
        public void OnGet() // This Method id Fatch The Data from Database to View
        {
            CategoryList = _db.Categories.ToList();
        }
    }
}
