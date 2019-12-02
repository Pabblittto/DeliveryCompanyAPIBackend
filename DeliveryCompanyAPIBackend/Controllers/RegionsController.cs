using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryCompanyAPIBackend.Models;

namespace DeliveryCompanyAPIBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public RegionsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Regions.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Region>> Get(int id)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(ob => ob.Id == id);

            if (region == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Region with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(region);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Region>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Region> partOfRegions= await _context.Regions.ToListAsync();// get all list
            try
            {
                if (partOfRegions.Count != 0)// there is something in this list
                {

                    if (partOfRegions.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = partOfRegions.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        partOfRegions = partOfRegions.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        partOfRegions = partOfRegions.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(partOfRegions);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no regions to display"
                };
                    return NotFound(Messages);
                }
            }
            catch (Exception e)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Some seroius problems occured. You send {amount} and {page} as numbers");
                return NotFound(Messages);
            }
        }

        [HttpPatch("{id}")]// api/Packs/Update
        public async Task<ActionResult> Update(int id, [FromBody] Region UpRegion)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpRegion == null)
            {
                Messages.Add("ID or Region object is null");
                return BadRequest(Messages);
            }

            var region= await _context.Regions.FirstOrDefaultAsync(ob => ob.Id == id);


            if (region!= null)
            {
                if (region.CityName != UpRegion.CityName) { region.CityName = UpRegion.CityName; }
                if (region.DepartmentId != UpRegion.DepartmentId) { region.DepartmentId = UpRegion.DepartmentId; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpRegion.DepartmentId);

                if (department == null)
                {
                    Messages.Add("Choosed department doesny exist");
                    return NotFound(Messages);
                }

                region.department = department;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Region updated succesfully");
                        return Ok(Messages);
                    }
                    else
                    {
                        Messages.Add($"{ammount} changes were made");
                        return Ok(Messages);
                    }

                }
                catch (Exception e)
                {
                    Messages.Add($"Some serous problems occured :c. You send {id} as ID");
                    return NotFound(Messages);
                }
            }
            else
            {
                Messages.Add($"There is no region with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var regionToDelete = await _context.Regions.FirstOrDefaultAsync(ob => ob.Id == id);
            if (regionToDelete == null)
            {
                Messages.Add("Position with certain name was not found");
                return NotFound(Messages);
            }

            _context.Regions.Remove(regionToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Region deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Region region)
        {
            List<string> Messages = new List<string>();
            if (region == null)
            {
                Messages.Add("Recived region is null");
                return NotFound(Messages);
            }

            Region NewRegion = region;

            var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == region.DepartmentId);
            if (department == null)
            {
                Messages.Add("You can not add region to a department, which doesnt exists");
                return NotFound(Messages);
            }
            NewRegion.department = department;

            await _context.AddAsync(NewRegion);
            await _context.SaveChangesAsync();
            Messages.Add("Region added succesfully");
            return Ok(Messages);
        }

    }
}
