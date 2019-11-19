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
    public class WarehousController : ControllerBase
    {
        private readonly CompanyContext _context;

        public WarehousController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Warehouses.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Warehous>> Get(int id)
        {
            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(ob => ob.Id == id);

            if (warehouse == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Warehouse with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(warehouse);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Warehous>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Warehous> partOfWarehouses = await _context.Warehouses.ToListAsync();// get all list
            try
            {
                if (partOfWarehouses.Count != 0)// there is something in this list
                {

                    if (partOfWarehouses.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = partOfWarehouses.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        partOfWarehouses = partOfWarehouses.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        partOfWarehouses = partOfWarehouses.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(partOfWarehouses);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no warehouses to display"
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
        public async Task<ActionResult> Update(int id, [FromBody] Warehous UpWarehouse)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpWarehouse == null)
            {
                Messages.Add("ID or Warehouse object is null");
                return BadRequest(Messages);
            }

            Warehous warehouse = await _context.Warehouses.FirstOrDefaultAsync(ob => ob.Id == id);


            if (warehouse != null)
            {
                if (warehouse.HouseNumber != UpWarehouse.HouseNumber) { warehouse.HouseNumber = UpWarehouse.HouseNumber; }
                if (warehouse.Street != UpWarehouse.Street) { warehouse.Street = UpWarehouse.Street; }
                if (warehouse.DepartmentId != UpWarehouse.DepartmentId) { warehouse.DepartmentId = UpWarehouse.DepartmentId; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpWarehouse.DepartmentId);

                if (department == null)
                {
                    Messages.Add($"Given department with id={id} doesnt exist");
                    return NotFound(Messages);
                }
                warehouse.department = department;

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
                Messages.Add($"There is no warehouse with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var warehouseToDelete = await _context.Warehouses.FirstOrDefaultAsync(ob => ob.Id == id);
            if (warehouseToDelete == null)
            {
                Messages.Add("Warehouse with certain name was not found");
                return NotFound(Messages);
            }

            _context.Warehouses.Remove(warehouseToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Warehouse deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Warehous warehouse)
        {
            List<string> Messages = new List<string>();
            if (warehouse == null)
            {
                Messages.Add("Recived warehouse is null");
                return NotFound(Messages);
            }

            Warehous NewWarehouse = warehouse;

            var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == warehoue.DepartmentId);

            if (department == null)
            {
                Messages.Add($"Choosed department (id={warehouse.DepartmentId}) doesnt exist");
                return NotFound(Messages);
            }

            await _context.AddAsync(NewWarehouse);
            await _context.SaveChangesAsync();
            Messages.Add("Warehouse added succesfully");
            return Ok(Messages);
        }


    }
}
