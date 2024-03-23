using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CredentialMgmtDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly AzureKeyVaultService _azureKeyVaultService;

        public IndexModel(ApplicationDbContext context, AzureKeyVaultService azureKeyVaultService)
        {
            _context = context;
            _azureKeyVaultService = azureKeyVaultService;
        }

        public IList<Product> Product { get;set; } = default!;
        public string? SecretValue { get; set; }

        public async Task OnGetAsync()
        {
            Product = await _context.Products.ToListAsync();
            SecretValue = await _azureKeyVaultService.GetSecret("testsecret");
        }
    }
}
