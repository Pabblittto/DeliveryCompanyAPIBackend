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
    public class WorkersController : ControllerBase
    {
        private readonly CompanyContext _context;

        public WorkersController(CompanyContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Workers.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/workers/id
        public async Task<ActionResult<Worker>> Get(int id)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(ob => ob.Id == id);

            if (worker == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Worker with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(worker);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Worker>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Worker> partOfWorkers = await _context.Workers.ToListAsync();// get all list
            try
            {
                if (partOfWorkers.Count != 0)// there is something in this list
                {

                    if (partOfWorkers.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = partOfWorkers.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        partOfWorkers = partOfWorkers.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        partOfWorkers = partOfWorkers.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(partOfWorkers);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no workers to display"
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


        [HttpPatch("{id}")]// api/Workers/Update
        public async Task<ActionResult> Update(int id, [FromBody] Worker UpWorker)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpWorker == null)
            {
                Messages.Add("ID or Worker object is null");
                return BadRequest(Messages);
            }

            Worker worker = await _context.Workers.FirstOrDefaultAsync(ob => ob.Id == id);


            if (worker != null)
            {
                if (worker.Name != UpWorker.Name) { worker.Name = UpWorker.Name; }
                if (worker.PositionId != UpWorker.PositionId) { worker.PositionId = UpWorker.PositionId; }
                if (worker.Surname != UpWorker.Surname) { worker.Surname = UpWorker.Surname; }
                if (worker.DepartmentId != UpWorker.DepartmentId) { worker.DepartmentId = UpWorker.DepartmentId; }

                var position = await _context.Positions.FirstOrDefaultAsync(ob => ob.Name == UpWorker.PositionId);

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpWorker.DepartmentId);

                if (department == null)
                {
                    Messages.Add($"Given department with id={id} doesnt exist");
                    return NotFound(Messages);
                }

                if (position == null)
                {
                    Messages.Add($"Given position with name={UpWorker.PositionId} doesnt exist");
                    return NotFound(Messages);
                }

                worker.department = department;
                worker.position = position;


                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Warehouse updated succesfully");
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
                Messages.Add($"There is no worker with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var workerToDelete = await _context.Workers.FirstOrDefaultAsync(ob => ob.Id == id);
            if (workerToDelete == null)
            {
                Messages.Add("Worker with certain id was not found");
                return NotFound(Messages);
            }

            _context.Workers.Remove(workerToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Worker deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Worker worker)
        {
            List<string> Messages = new List<string>();
            if (worker == null)
            {
                Messages.Add("Recived worker is null");
                return NotFound(Messages);
            }

            Worker NewWorker = worker;

            var stanowisko = await _context.Positions.FirstOrDefaultAsync(ob => ob.Name == worker.PositionId);
            var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == worker.DepartmentId);

            if (department == null)
            {
                Messages.Add($"Choosed department (id={worker.DepartmentId}) doesnt exist");
                return NotFound(Messages);
            }

            if (stanowisko == null)
            {
                Messages.Add($"Choosed position (name={worker.PositionId}) doesnt exists");
            }
            NewWorker.position = stanowisko;
            NewWorker.department = department;


            await _context.AddAsync(NewWorker);
            await _context.SaveChangesAsync();
            Messages.Add("Worker added succesfully");
            return Ok(Messages);
        }
    }
}
