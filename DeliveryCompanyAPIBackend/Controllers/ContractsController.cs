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
                ModelState.AddModelError("errors", $"Contract with certain id={id} not found");
                return NotFound(ModelState);
            }

            return Ok(contract);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Contract>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                ModelState.AddModelError("errors","Problems occured. You send amount = 0");
                return NotFound(ModelState);
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
                            ModelState.AddModelError("errors",$"There are too few records in base to create {page} page.(Wrong page number)");
                            return NotFound(ModelState);
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
                    ModelState.AddModelError("errors", "There are no Contracts to display");
                    return NotFound(ModelState);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("errors",$"Some seroius problems occured. You send {amount} and {page} as numbers");
                return NotFound(ModelState);
            }

        }


        [HttpPatch("{id}")]// api/Contracts/Update
        public async Task<ActionResult> Update(int id, [FromBody] Contract UpContract)
        {

            if (id == 0 || UpContract == null)
            {
                ModelState.AddModelError("errors", "ID or Contract object is null");
                return BadRequest(ModelState);
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
                    ModelState.AddModelError("errors","There are no worker with given id");
                    return NotFound(ModelState);
                }

                contract.worker = worker;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        string OkMessage = "Contract updated succesfully";
                        return Ok(OkMessage);
                    }
                    else
                    {
                        string OkMessage = $"{ammount} changes were made";
                        return Ok(OkMessage);
                    }

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("errors",$"Some serous problems occured :c. You send {id} as ID");
                    return NotFound(ModelState);
                }

            }
            else
            {
                ModelState.AddModelError("errors", $"There is no Contract with certain id={id}");
                return NotFound(ModelState);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var contractToDlete = await _context.Contracts.FirstOrDefaultAsync(ob => ob.Id == id);
            if (contractToDlete == null)
            {
                ModelState.AddModelError("errors","Contract with certain id was not found");
                return NotFound(ModelState);
            }

            _context.Contracts.Remove(contractToDlete);
            await _context.SaveChangesAsync();

            string OkMessage = "Car deleted succesfully";
            return Ok(OkMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Contract contract)
        {
            if (contract == null)
            {
                ModelState.AddModelError("errors","Recived contract is null");
                return NotFound(ModelState);
            }
            Contract NewContract = contract;

            Worker worker = await _context.Workers.FirstOrDefaultAsync(ob => ob.Id == contract.WorkerId);

            if (worker == null)
            {
                ModelState.AddModelError("errors","You can not add Contract to worker, which doesnt exists");
                return NotFound(ModelState);
            }

            NewContract.worker = worker;

            await _context.AddAsync(NewContract);
            await _context.SaveChangesAsync();

            string OkMessage = "Contract added succesfully";
            return Ok(OkMessage);
        }

    }
}
