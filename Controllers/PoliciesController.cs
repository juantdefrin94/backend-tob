using BackendTob.Data;
using BackendTob.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackendTob.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PoliciesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PoliciesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Policy>>> GetPolicies()
        {
            return await _context.Policies.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Policy>> CreatePolicy(Policy policy)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
            var today = DateTime.Today;
            var prefix = "P" + today.ToString("yyMMdd"); 

            var lastPolicy = await _context.Policies
                .Where(p => p.PolicyNumber.StartsWith(prefix))
                .OrderByDescending(p => p.PolicyNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastPolicy != null)
            {
                var lastNumberStr = lastPolicy.PolicyNumber.Substring(prefix.Length);
                if (int.TryParse(lastNumberStr, out var lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            policy.PolicyNumber = prefix + nextNumber.ToString("D3"); 

            
            if (policy.TSI <= 0 || policy.PremiumRate <= 0)
                return BadRequest("TSI and PremiumRate must be greater than zero.");

            policy.PremiumAmount = (policy.TSI * policy.PremiumRate) / 100;

            
            _context.Policies.Add(policy);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPolicy), new { id = policy.Id }, policy);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult<Policy>> GetPolicy(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null) return NotFound();
            return policy;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePolicy(int id, Policy updatedPolicy)
        {
            if (id != updatedPolicy.Id)
                return BadRequest("Policy ID mismatch.");

            var existingPolicy = await _context.Policies.FindAsync(id);
            if (existingPolicy == null)
                return NotFound();

            
            existingPolicy.BeneficiaryName = updatedPolicy.BeneficiaryName;
            existingPolicy.CarBrand = updatedPolicy.CarBrand;
            existingPolicy.CarType = updatedPolicy.CarType;
            existingPolicy.TSI = updatedPolicy.TSI;
            existingPolicy.PremiumRate = updatedPolicy.PremiumRate;
            existingPolicy.StartDate = updatedPolicy.StartDate;
            existingPolicy.EndDate = updatedPolicy.EndDate;

            
            existingPolicy.PremiumAmount = (updatedPolicy.TSI * updatedPolicy.PremiumRate) / 100;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePolicy(int id)
        {
            var policy = await _context.Policies.FindAsync(id);
            if (policy == null) return NotFound();

            _context.Policies.Remove(policy);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
