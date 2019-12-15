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
    public class PositionsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public PositionsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Positions.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Position>> Get(string id)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(ob => ob.Name == id);

            if (position == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Position with certain name={id} not found");
                return NotFound(Messages);
            }

            return Ok(position);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Position>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Position> partOfPositions = await _context.Positions.ToListAsync();// get all list
            try
            {
                if (partOfPositions.Count != 0)// there is something in this list
                {

                    if (partOfPositions.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = partOfPositions.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        partOfPositions = partOfPositions.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        partOfPositions = partOfPositions.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(partOfPositions);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no positions to display"
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
        public async Task<ActionResult> Update(string id, [FromBody] Position UpPosition)
        {

            List<string> Messages = new List<string>();

            if (id == "" || UpPosition == null)
            {
                Messages.Add("ID or Position object is null");
                return BadRequest(Messages);
            }

            Position position = await _context.Positions.FirstOrDefaultAsync(ob => ob.Name == id);

            
            if (position != null)
            {
                if (position.MaxSalary != UpPosition.MaxSalary) { position.MaxSalary = UpPosition.MaxSalary; }
                if (position.MinSalary != UpPosition.MinSalary) { position.MinSalary = UpPosition.MinSalary; }


                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Position updated succesfully");
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
                Messages.Add($"There is no position with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            List<string> Messages = new List<string>();
            var positionToDelete = await _context.Positions.FirstOrDefaultAsync(ob => ob.Name == id);
            if (positionToDelete == null)
            {
                Messages.Add("Position with certain name was not found");
                return NotFound(Messages);
            }

            _context.Positions.Remove(positionToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Position deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Position position)
        {
            List<string> Messages = new List<string>();
            if (position == null)
            {
                Messages.Add("Recived position is null");
                return NotFound(Messages);
            }

            Position NewPosition = position;


            await _context.AddAsync(NewPosition);
            await _context.SaveChangesAsync();
            Messages.Add("Position added succesfully");
            return Ok(Messages);
        }
    }
}
