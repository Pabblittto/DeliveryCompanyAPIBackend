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
    public class PackTypesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public PackTypesController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.PackTypes.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<PackType>> Get(string id)
        {
            var packtype = await _context.PackTypes.FirstOrDefaultAsync(ob => ob.Name == id);

            if (packtype == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"PackType with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(packtype);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<PackType>>> GetPackTypes(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<PackType> PartofPackTypes = await _context.PackTypes.ToListAsync();// get all list
            try
            {
                if (PartofPackTypes.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartofPackTypes.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartofPackTypes.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartofPackTypes = PartofPackTypes.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartofPackTypes = PartofPackTypes.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartofPackTypes);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no Pack Types to display"
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
        public async Task<ActionResult> Update(string id, [FromBody] PackType UpPackType)
        {

            List<string> Messages = new List<string>();

            if (id == "" || UpPackType == null)
            {
                Messages.Add("ID or Pack Type object is null");
                return BadRequest(Messages);
            }

            var packType = await _context.PackTypes.FirstOrDefaultAsync(ob => ob.Name == id);

            if (packType != null)
            {
                if (packType.MaxWeight != UpPackType.MaxWeight) { packType.MaxWeight = UpPackType.MaxWeight; }
                if (packType.MinWeight != UpPackType.MinWeight) { packType.MinWeight = UpPackType.MinWeight; }
                if (packType.Price != UpPackType.Price) { packType.Price = UpPackType.Price; }
                
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
        public async Task<ActionResult> Delete(string id)
        {
            List<string> Messages = new List<string>();
            var packTypeToDelete = await _context.PackTypes.FirstOrDefaultAsync(ob => ob.Name == id);
            if (packTypeToDelete == null)
            {
                Messages.Add("Pack type with certain name was not found");
                return NotFound(Messages);
            }

            _context.PackTypes.Remove(packTypeToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Pack type deleted succesfully");
            return Ok(Messages);
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody]PackType packType)
        {
            List<string> Messages = new List<string>();
            if (packType == null)
            {
                Messages.Add("Recived pack type is null");
                return NotFound(Messages);
            }

            PackType NewPackType = packType;

            await _context.AddAsync(NewPackType);
            await _context.SaveChangesAsync();
            Messages.Add("Pack type added succesfully");
            return Ok(Messages);
        }


    }
}
