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
    public class ParcelLockersController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ParcelLockersController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.ParcelLockers.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<ParcelLocker>> Get(int id)
        {
            var locker = await _context.ParcelLockers.FirstOrDefaultAsync(ob => ob.Id == id);

            if (locker == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Locker with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(locker);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<ParcelLocker>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<ParcelLocker> PartOfLockers = await _context.ParcelLockers.ToListAsync();// get all list
            try
            {
                if (PartOfLockers.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartOfLockers.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfLockers.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfLockers = PartOfLockers.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfLockers = PartOfLockers.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfLockers);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no Lockers to display"
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
        public async Task<ActionResult> Update(int id, [FromBody] ParcelLocker UpLocker)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpLocker == null)
            {
                Messages.Add("ID or Locker object is null");
                return BadRequest(Messages);
            }

            var locker = await _context.ParcelLockers.FirstOrDefaultAsync(ob => ob.Id == id);

            if (locker != null)
            {
                if (locker.CellsAmount != UpLocker.CellsAmount) { locker.CellsAmount = UpLocker.CellsAmount; }
                if (locker.FreeCells != UpLocker.FreeCells) { locker.FreeCells = UpLocker.FreeCells; }
                if (locker.StreetId != UpLocker.StreetId) { locker.StreetId = UpLocker.StreetId; }

                var street = await _context.Streets.FirstOrDefaultAsync(ob => ob.Id == UpLocker.StreetId);

                if (street == null)
                {
                    Messages.Add($"There is no street type with given id={UpLocker.StreetId}");
                    return NotFound(Messages);
                }

                locker.street = street;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Locker updated succesfully");
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
                Messages.Add($"There is no parcel locker with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var lockerToDelete = await _context.ParcelLockers.FirstOrDefaultAsync(ob => ob.Id == id);
            if (lockerToDelete == null)
            {
                Messages.Add("Locker with certain id was not found");
                return NotFound(Messages);
            }

            _context.ParcelLockers.Remove(lockerToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Locker deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]ParcelLocker locker)
        {
            List<string> Messages = new List<string>();
            if (locker == null)
            {
                Messages.Add("Recived loecker is null");
                return NotFound(Messages);
            }

            ParcelLocker NewLocker= locker;

            var street = await _context.Streets.FirstOrDefaultAsync(ob => ob.Id == locker.StreetId);

            if (street == null)
                Messages.Add("Choosed street doesnt exist");

            locker.street = street;

            await _context.AddAsync(NewLocker);
            await _context.SaveChangesAsync();
            Messages.Add("Locker added succesfully");
            return Ok(Messages);
        }
    }
}
