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
    public class CourierTabletsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CourierTabletsController(CompanyContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.CourierTablets.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/CourierTablets/id
        public async Task<ActionResult<CourierTablet>> Get(int id)
        {
            var CourierTablet = await _context.CourierTablets.FirstOrDefaultAsync(ob => ob.Id == id);

            if (CourierTablet == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Courier tablet with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(CourierTablet);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<CourierTablet>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<CourierTablet> PartOfTablets = await _context.CourierTablets.ToListAsync();// get all list
            try
            {
                if (PartOfTablets.Count != 0)// there is something in this list
                {

                    if (PartOfTablets.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfTablets.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfTablets = PartOfTablets.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfTablets = PartOfTablets.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfTablets);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no Courier Tablets to display"
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


        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var courierTabletToDelete = await _context.CourierTablets.FirstOrDefaultAsync(ob => ob.Id == id);
            if (courierTabletToDelete == null)
            {
                Messages.Add("Contract with certain id was not found");
                return NotFound(Messages);
            }

            _context.CourierTablets.Remove(courierTabletToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Car deleted succesfully");
            return Ok(Messages);
        }


        [HttpPatch]// api/Contracts/Update
        public async Task<ActionResult> Update(int id, CourierTablet UpTablet)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpTablet == null)
            {
                Messages.Add("ID or Tablet object is null");
                return BadRequest(Messages);
            }

            var tablet = await _context.CourierTablets.FirstOrDefaultAsync(ob => ob.Id == id);
            if (tablet != null)
            {

                if (tablet.AddedDate != UpTablet.AddedDate) { tablet.AddedDate = UpTablet.AddedDate; }
                if (tablet.DepartmentId != UpTablet.DepartmentId) { tablet.DepartmentId = UpTablet.DepartmentId; }
                if (tablet.Model != UpTablet.Model) { tablet.Model = UpTablet.Model; }
                if (tablet.ProdYear != UpTablet.ProdYear) { tablet.ProdYear = UpTablet.ProdYear; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpTablet.DepartmentId);
                if (department == null)
                {
                    Messages.Add("There is no department with given id");
                    return NotFound(Messages);
                }

                tablet.department = department;

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
                Messages.Add($"There is no tablet with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]CourierTablet tablet)
        {
            List<string> Messages = new List<string>();
            if (tablet == null)
            {
                Messages.Add("Recived courier tablet is null");
                return NotFound(Messages);
            }
            CourierTablet NewTablet = tablet;

            Department department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == tablet.DepartmentId);

            if (department == null)
            {
                Messages.Add("You can not add Tablet to department, which doesnt exists");
                return NotFound(Messages);
            }

            NewTablet.department = department;

            await _context.AddAsync(NewTablet);
            Messages.Add("Contract added succesfully");
            return Ok();
        }


       
    }
}
