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
    public class PacksController : ControllerBase
    {
        private readonly CompanyContext _context;

        public PacksController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Packs.CountAsync();
            return Ok(amount);
        }


        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Pack>> Get(int id)
        {
            var pack = await _context.Packs.FirstOrDefaultAsync(ob => ob.Id == id);

            if (pack == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Pack with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(pack);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Pack>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Pack> PartOfPacks = await _context.Packs.ToListAsync();// get all list
            try
            {
                if (PartOfPacks.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartOfPacks.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfPacks.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfPacks = PartOfPacks.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfPacks = PartOfPacks.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfPacks);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no Packs to display"
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
        public async Task<ActionResult> Update(int id, [FromBody] Pack UpPack)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpPack == null)
            {
                Messages.Add("ID or Pack object is null");
                return BadRequest(Messages);
            }

            var pack = await _context.Packs.FirstOrDefaultAsync(ob => ob.Id == id);

            if (pack != null)
            {

                if (pack.Height != UpPack.Height) { pack.Height = UpPack.Height; }
                if (pack.Weight != UpPack.Weight) { pack.Weight = UpPack.Weight; }
                if (pack.PackTypeId != UpPack.PackTypeId) { pack.PackTypeId = UpPack.PackTypeId; }

                var packType = await _context.PackTypes.FirstOrDefaultAsync(ob => ob.Name == UpPack.PackTypeId);

                if (packType == null)
                {
                    Messages.Add($"There is no pack type with given Name={UpPack.PackTypeId}");
                    return NotFound(Messages);
                }

                pack.type = packType;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Pack updated succesfully");
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
                Messages.Add($"There is no pack with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var packToDelete = await _context.Packs.FirstOrDefaultAsync(ob => ob.Id == id);
            if (packToDelete == null)
            {
                Messages.Add("Pack with certain id was not found");
                return NotFound(Messages);
            }

            _context.Packs.Remove(packToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Pack deleted succesfully");
            return Ok(Messages);
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Pack pack)
        {
            List<string> Messages = new List<string>();
            if (pack == null)
            {
                Messages.Add("Recived pack is null");
                return NotFound(Messages);
            }

            Pack NewPack = pack;

            var packtype = await _context.PackTypes.FirstOrDefaultAsync(ob => ob.Name == pack.PackTypeId);

            if (packtype == null)
                Messages.Add("Choosed pack type doesnt exist");

            NewPack.type = packtype;

            await _context.AddAsync(NewPack);
            await _context.SaveChangesAsync();
            Messages.Add("Pack added succesfully");
            return Ok(Messages);
        }


    }
}
