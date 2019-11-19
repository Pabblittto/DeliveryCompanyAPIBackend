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
                List<string> Messages = new List<string>();
                Messages.Add($"Invoice with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(invoice);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Invoice>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Invoice> PartOfInvoices = await _context.Invoices.ToListAsync();// get all list
            try
            {
                if (PartOfInvoices.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartOfInvoices.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfInvoices.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
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
                    List<string> Messages = new List<string>()
                {
                    "There are no Invoices to display"
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

        [HttpPatch("{id}")]// api/Cars/Update
        public async Task<ActionResult> Update(int id, [FromBody] Invoice UpInvoice)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpInvoice == null)
            {
                Messages.Add("ID or Invoice object is null");
                return BadRequest(Messages);
            }

            var invoice = await _context.Invoices.FirstOrDefaultAsync(ob => ob.Id == id);
            if (invoice != null)
            {
                if (invoice.FilePath != UpInvoice.FilePath) { invoice.FilePath = UpInvoice.FilePath; }
                if (invoice.DepartmentId != UpInvoice.DepartmentId) { invoice.DepartmentId = UpInvoice.DepartmentId; }
                if (invoice.Description != UpInvoice.Description) { invoice.Description = UpInvoice.Description; }
                if (invoice.data != UpInvoice.data) { invoice.data = UpInvoice.data; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpInvoice.DepartmentId);

                if (department == null)
                {
                    Messages.Add("There is no dapertment with given id");
                    return NotFound(Messages);
                }

                invoice.department = department;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Invoice updated succesfully");
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
                Messages.Add($"There is no invoice with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var invoiceToDelete = await _context.Invoices.FirstOrDefaultAsync(ob => ob.Id == id);
            if (invoiceToDelete == null)
            {
                Messages.Add("Invoice with certain id was not found");
                return NotFound(Messages);
            }

            _context.Invoices.Remove(invoiceToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Invoice deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Invoice invoice)
        {
            List<string> Messages = new List<string>();
            if (invoice == null)
            {
                Messages.Add("Recived invoice is null");
                return NotFound(Messages);
            }
            Invoice NewInvoice = invoice;

            Department department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == invoice.DepartmentId);

            if (department == null)
            {
                Messages.Add("You can not add invoice to department, which doesnt exists");
                return NotFound(Messages);
            }

            NewInvoice.department = department;

            await _context.AddAsync(NewInvoice);
            await _context.SaveChangesAsync();
            
            Messages.Add("Invoice added succesfully");
            return Ok(Messages);

        }

    }
}
