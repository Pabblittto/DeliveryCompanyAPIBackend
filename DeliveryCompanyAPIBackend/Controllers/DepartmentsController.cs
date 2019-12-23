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
                ModelState.AddModelError("errors", $"Department with certain id={id} not found");
                return NotFound(ModelState);
            }

            return Ok(department);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Department>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                ModelState.AddModelError("errors", "Problems occured. You send amount = 0");
                return NotFound(ModelState);
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
                            ModelState.AddModelError("errors", $"There are too few records in base to create {page} page.(Wrong page number)");
                            return NotFound(ModelState);
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
                    ModelState.AddModelError("errors", "There are no Departments to display");
                    return NotFound(ModelState);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("errors", $"Some seroius problems occured. You send {amount} and {page} as numbers");
                return NotFound(ModelState);
            }
        }

        [HttpPatch("{id}")]// api/Departments/Update
        public async Task<ActionResult> Update(int id,[FromBody] Department UpDepartment)
        {

            if (id == 0 || UpDepartment == null)
            {
                ModelState.AddModelError("errors","ID or Department object is null");
                return BadRequest(ModelState);
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
                        string OkMessage = "Department updated succesfully";
                        return Ok(OkMessage);
                    }
                    else
                    {
                        string OkMessage=$"{ammount} changes were made";

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
                ModelState.AddModelError("errors", $"There is no department with certain id={id}");
                return NotFound(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var departmentToDelete = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == id);
            if (departmentToDelete == null)
            {
                ModelState.AddModelError("errors", "Department with certain id was not found");
                return NotFound(ModelState);
            }

            _context.Departments.Remove(departmentToDelete);
            await _context.SaveChangesAsync();


            string OkMessage = "Department deleted succesfully";
            return Ok(OkMessage);
        }


        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Department department)
        {
            if (department == null)
            {
                ModelState.AddModelError("errors","Recived department is null");
                return NotFound(ModelState);
            }
            Department NewDepartment = department;


            await _context.AddAsync(NewDepartment);
            await _context.SaveChangesAsync();
            string OkMessage = "Department added succesfully";
            return Ok(OkMessage);
        }

    }
}
