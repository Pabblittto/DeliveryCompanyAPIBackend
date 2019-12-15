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
    public class ContractsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public ContractsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Contracts.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/Contracts/id
        public async Task<ActionResult<Contract>> Get(int id)
        {
            var contract = await _context.Contracts.FirstOrDefaultAsync(ob => ob.Id == id);

            if (contract == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Contract with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(contract);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Contract>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Contract> PartOfContracts = await _context.Contracts.ToListAsync();// get all list
            try
            {
                if (PartOfContracts.Count != 0)// there is something in this list
                {

                    if (PartOfContracts.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfContracts.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfContracts = PartOfContracts.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfContracts = PartOfContracts.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfContracts);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no Contracts to display"
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


        [HttpPatch("{id}")]// api/Contracts/Update
        public async Task<ActionResult> Update(int id, [FromBody] Contract UpContract)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpContract == null)
            {
                Messages.Add("ID or Contract object is null");
                return BadRequest(Messages);
            }

            var contract = await _context.Contracts.FirstOrDefaultAsync(ob => ob.Id == id);
            if (contract != null)
            {
                if (contract.Date != UpContract.Date) { contract.Date = UpContract.Date; }
                if (contract.FilePath != UpContract.FilePath) { contract.FilePath = UpContract.FilePath; }
                if (contract.WorkerId != UpContract.WorkerId) { contract.WorkerId = UpContract.WorkerId; }

                var worker = await  _context.Workers.FirstOrDefaultAsync(ob => ob.Id == UpContract.WorkerId);
                if (worker == null)
                {
                    Messages.Add("There are no worker with given id");
                    return NotFound(Messages);
                }

                contract.worker = worker;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Contract updated succesfully");
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
                Messages.Add($"There is no Contract with certain id={id}");
                return NotFound(Messages);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var contractToDlete = await _context.Contracts.FirstOrDefaultAsync(ob => ob.Id == id);
            if (contractToDlete == null)
            {
                Messages.Add("Contract with certain id was not found");
                return NotFound(Messages);
            }

            _context.Contracts.Remove(contractToDlete);
            await _context.SaveChangesAsync();

            Messages.Add("Car deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Contract contract)
        {
            List<string> Messages = new List<string>();
            if (contract == null)
            {
                Messages.Add("Recived contract is null");
                return NotFound(Messages);
            }
            Contract NewContract = contract;

            Worker worker = await _context.Workers.FirstOrDefaultAsync(ob => ob.Id == contract.WorkerId);

            if (worker == null)
            {
                Messages.Add("You can not add Contract to worker, which doesnt exists");
                return NotFound(Messages);
            }

            NewContract.worker = worker;

            await _context.AddAsync(NewContract);
            await _context.SaveChangesAsync();

            Messages.Add("Contract added succesfully");
            return Ok(Messages);
        }

    }
}
