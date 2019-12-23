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
    public class InvoicesController : ControllerBase
    {
        private readonly CompanyContext _context;

        public InvoicesController(CompanyContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Invoices.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/Invoices/id
        public async Task<ActionResult<Invoice>> Get(int id)
        {
            var invoice = await _context.Invoices.FirstOrDefaultAsync(ob => ob.Id == id);

            if (invoice == null)
            {
                ModelState.AddModelError("errors", $"Invoice with certain id={id} not found");
                return NotFound(ModelState);
            }

            return Ok(invoice);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Invoice>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                ModelState.AddModelError("errors", "Problems occured. You send amount = 0");
                return NotFound(ModelState);
            }

            List<Invoice> PartOfInvoices = await _context.Invoices.ToListAsync();// get all list
            try
            {
                if (PartOfInvoices.Count != 0)// there is something in this list
                {

                    if (PartOfInvoices.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfInvoices.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            ModelState.AddModelError("errors", $"There are too few records in base to create {page} page.(Wrong page number)");
                            return NotFound(ModelState);
                        }
                        PartOfInvoices = PartOfInvoices.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfInvoices = PartOfInvoices.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfInvoices);
                }
                else
                {
                    ModelState.AddModelError("errors", "There are no Invoices to display");
                    return NotFound(ModelState);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("errors", $"Some seroius problems occured. You send {amount} and {page} as numbers");
                return NotFound(ModelState);
            }

        }

        [HttpPatch("{id}")]// api/Cars/Update
        public async Task<ActionResult> Update(int id, [FromBody] Invoice UpInvoice)
        {
            if (id == 0 || UpInvoice == null)
            {
                ModelState.AddModelError("errors", "ID or Invoice object is null");
                return BadRequest(ModelState);
            }

            var invoice = await _context.Invoices.FirstOrDefaultAsync(ob => ob.Id == id);
            if (invoice != null)
            {
                if (invoice.FilePath != UpInvoice.FilePath) { invoice.FilePath = UpInvoice.FilePath; }
                if (invoice.DepartmentId != UpInvoice.DepartmentId) { invoice.DepartmentId = UpInvoice.DepartmentId; }
                if (invoice.Description != UpInvoice.Description) { invoice.Description = UpInvoice.Description; }
                if (invoice.Date != UpInvoice.Date) { invoice.Date = UpInvoice.Date; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpInvoice.DepartmentId);

                if (department == null)
                {
                    ModelState.AddModelError("errors", "There is no dapertment with given id");
                    return NotFound(ModelState);
                }

                invoice.department = department;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        string OkMessage = "Invoice updated succesfully";
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
                ModelState.AddModelError("errors",$"There is no invoice with certain id={id}");
                return NotFound(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var invoiceToDelete = await _context.Invoices.FirstOrDefaultAsync(ob => ob.Id == id);
            if (invoiceToDelete == null)
            {
                ModelState.AddModelError("errors","Invoice with certain id was not found");
                return NotFound(ModelState);
            }

            _context.Invoices.Remove(invoiceToDelete);
            await _context.SaveChangesAsync();

            string OkMessage = "Invoice deleted succesfully";
            return Ok(OkMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Invoice invoice)
        {
            if (invoice == null)
            {
                ModelState.AddModelError("errors","Recived invoice is null");
                return NotFound(ModelState);
            }
            Invoice NewInvoice = invoice;

            Department department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == invoice.DepartmentId);

            if (department == null)
            {
                ModelState.AddModelError("errors","You can not add invoice to department, which doesnt exists");
                return NotFound(ModelState);
            }

            NewInvoice.department = department;

            await _context.AddAsync(NewInvoice);
            await _context.SaveChangesAsync();
            
            string OkMessage = "Invoice added succesfully";
            return Ok(OkMessage);

        }

    }
}
