using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using coderush.Services;
using Microsoft.AspNetCore.Authorization;

namespace coderush.Controllers.Api
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Branch")]
    public class BranchController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BranchController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        [HttpGet]
        public async Task<IActionResult> GetBranch()
        {
            List<Branch> Items = await _context.Branch.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<Branch> payload)
        {
            Branch branch = payload.value;
            _context.Branch.Add(branch);
            _context.SaveChanges();
            return Ok(branch);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<Branch> payload)
        {
            Branch branch = payload.value;
            _context.Branch.Update(branch);
            _context.SaveChanges();
            return Ok(branch);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody]CrudViewModel<Branch> payload)
        {
            Branch branch = _context.Branch
                .Where(x => x.BranchId == (int)payload.key)
                .FirstOrDefault();
            _context.Branch.Remove(branch);
            _context.SaveChanges();
            return Ok(branch);

        }
        
    }
}