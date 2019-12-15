using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeliveryCompanyAPIBackend.Models;
using Microsoft.AspNetCore.Cors;

namespace DeliveryCompanyAPIBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyDomain")]
    public class StreetsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public StreetsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Streets.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Street>> Get(int id)
        {
            var street = await _context.Streets.FirstOrDefaultAsync(ob => ob.Id == id);

            if (street == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Street with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(street);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Street>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Street> partOfStreets = await _context.Streets.ToListAsync();// get all list
            try
            {
                if (partOfStreets.Count != 0)// there is something in this list
                {

                    if (partOfStreets.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = partOfStreets.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        partOfStreets = partOfStreets.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        partOfStreets = partOfStreets.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(partOfStreets);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no streets to display"
                };
                    return NotFound(Messages);
                }
            }
            catch (Exception e)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Some seroius problems occured. You send {amount} and {page} as numbers");
                Messages.Add(e.Message);
                return NotFound(Messages);
            }
        }


        [HttpPatch("{id}")]// api/Streets/Update
        public async Task<ActionResult> Update(int id, [FromBody] Street UpStreet)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpStreet == null)
            {
                Messages.Add("ID or Street object is null");
                return BadRequest(Messages);
            }

            Street street = await _context.Streets.FirstOrDefaultAsync(ob => ob.Id == id);


            if (street != null)
            {
                if (street.StreetName != UpStreet.StreetName) { street.StreetName = UpStreet.StreetName; }
                if (street.RegionId != UpStreet.RegionId) { street.RegionId = UpStreet.RegionId; }

                var region = await _context.Regions.FirstOrDefaultAsync(ob => ob.Id == UpStreet.RegionId);

                if (region == null)
                {
                    Messages.Add($"Given region with id={id} doesnt exist");
                    return NotFound(Messages);
                }
                street.region = region;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Street updated succesfully");
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
                Messages.Add($"There is no street with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var streetToDelete = await _context.Streets.FirstOrDefaultAsync(ob => ob.Id == id);
            if (streetToDelete == null)
            {
                Messages.Add("Street with certain name was not found");
                return NotFound(Messages);
            }

            _context.Streets.Remove(streetToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Street deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Street street)
        {
            List<string> Messages = new List<string>();
            if (street == null)
            {
                Messages.Add("Recived street is null");
                return NotFound(Messages);
            }

            Street NewStreet = street;

            var region = await  _context.Regions.FirstOrDefaultAsync(ob => ob.Id == street.RegionId);
            if (region == null)
            {
                Messages.Add($"Choosed region (id={street.RegionId}) doesnt exist");
                return NotFound(Messages);
            }

            NewStreet.region = region;

            await _context.AddAsync(NewStreet);
            await _context.SaveChangesAsync();
            Messages.Add("Street added succesfully");
            return Ok(Messages);
        }

    }
}
