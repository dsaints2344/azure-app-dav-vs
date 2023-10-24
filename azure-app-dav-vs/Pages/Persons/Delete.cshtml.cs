﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using azure_app_dav_vs.Data;

namespace azure_app_dav_vs.Pages.Persons
{
    public class DeleteModel : PageModel
    {
        private readonly azure_app_dav_vs.Data.AppDbContext _context;

        public DeleteModel(azure_app_dav_vs.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Person Person { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.persons == null)
            {
                return NotFound();
            }

            var person = await _context.persons.FirstOrDefaultAsync(m => m.Id == id);

            if (person == null)
            {
                return NotFound();
            }
            else 
            {
                Person = person;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.persons == null)
            {
                return NotFound();
            }
            var person = await _context.persons.FindAsync(id);

            if (person != null)
            {
                Person = person;
                _context.persons.Remove(Person);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}