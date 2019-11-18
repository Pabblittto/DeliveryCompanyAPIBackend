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
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public DepartmentsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Departments.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Department>> Get(int id)
        {
            var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == id);

            if (department == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Department with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(department);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Department>>> GetDepartments(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Department> PartOfDepartments = await _context.Departments.ToListAsync();// get all list
            try
            {
                if (PartOfDepartments.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartOfDepartments.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfDepartments.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfDepartments = PartOfDepartments.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfDepartments = PartOfDepartments.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfDepartments);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There is no Departments to display"
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

        [HttpPatch("{id}")]// api/Departments/Update
        public async Task<ActionResult> Update(int id,[FromBody] Department UpDepartment)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpDepartment == null)
            {
                Messages.Add("ID or Department object is null");
                return BadRequest(Messages);
            }

            var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == id);

            if (department != null)
            {
                if (department.BankAccountNo != UpDepartment.BankAccountNo) { department.BankAccountNo = UpDepartment.BankAccountNo; }
                if (department.BuildingNo != UpDepartment.BuildingNo) { department.BuildingNo = UpDepartment.BuildingNo; }
                if (department.ManagerTelNo != UpDepartment.ManagerTelNo) { department.ManagerTelNo = UpDepartment.ManagerTelNo; }
                if (department.OfficeTelNo != UpDepartment.OfficeTelNo) { department.OfficeTelNo = UpDepartment.OfficeTelNo; }
                if (department.Street != UpDepartment.Street) { department.Street = UpDepartment.Street; }
                if (department.Name != UpDepartment.Name) { department.Name = UpDepartment.Name; }


                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Department updated succesfully");
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
                Messages.Add($"There is no department with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var departmentToDelete = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == id);
            if (departmentToDelete == null)
            {
                Messages.Add("Department with certain id was not found");
                return NotFound(Messages);
            }

            _context.Departments.Remove(departmentToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Department deleted succesfully");
            return Ok(Messages);
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Department department)
        {
            List<string> Messages = new List<string>();
            if (department == null)
            {
                Messages.Add("Recived department is null");
                return NotFound(Messages);
            }
            Department NewDepartment = department;


            await _context.AddAsync(department);
            await _context.SaveChangesAsync();
            Messages.Add("Department added succesfully");
            return Ok(Messages);
        }

        //// GET: api/Departments
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        //{
        //    return await _context.Departments.ToListAsync();
        //}

        //// GET: api/Departments/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Department>> GetDepartment(int id)
        //{
        //    var department = await _context.Departments.FindAsync(id);

        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    return department;
        //}

        //// PUT: api/Departments/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDepartment(int id, Department department)
        //{
        //    if (id != department.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(department).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DepartmentExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Departments
        //[HttpPost]
        //public async Task<ActionResult<Department>> PostDepartment(Department department)
        //{
        //    _context.Departments.Add(department);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        //}

        //// DELETE: api/Departments/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Department>> DeleteDepartment(int id)
        //{
        //    var department = await _context.Departments.FindAsync(id);
        //    if (department == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Departments.Remove(department);
        //    await _context.SaveChangesAsync();

        //    return department;
        //}

        //private bool DepartmentExists(int id)
        //{
        //    return _context.Departments.Any(e => e.Id == id);
        //}
    }
}
