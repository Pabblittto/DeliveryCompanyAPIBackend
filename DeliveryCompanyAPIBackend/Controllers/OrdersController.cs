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
    public class OrdersController : ControllerBase
    {
        private readonly CompanyContext _context;

        public OrdersController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.Orders.CountAsync();
            return Ok(amount);
        }


        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Order>> Get(int id)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(ob => ob.Id == id);

            if (order == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Order with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(order);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Order>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Order> PartOfOrders = await _context.Orders.ToListAsync();// get all list
            try
            {
                if (PartOfOrders.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartOfOrders.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfOrders.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfOrders = PartOfOrders.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfOrders = PartOfOrders.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfOrders);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no Orders to display"
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
        public async Task<ActionResult> Update(int id, [FromBody] Order UpOrder)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpOrder == null)
            {
                Messages.Add("ID or Department object is null");
                return BadRequest(Messages);
            }

            var order = await _context.Orders.FirstOrDefaultAsync(ob => ob.Id == id);

            if (order != null)
            {

                if (order.DepartmentId != UpOrder.DepartmentId) { order.DepartmentId = UpOrder.DepartmentId; }
                if (order.State != UpOrder.State) { order.State = UpOrder.State; }
                if (order.SenderId != UpOrder.SenderId) { order.SenderId = UpOrder.SenderId; }
                if (order.ReciverId != UpOrder.ReciverId) { order.ReciverId = UpOrder.ReciverId; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpOrder.DepartmentId);
                var sender = await _context.People.FirstOrDefaultAsync(ob => ob.Id == UpOrder.SenderId);
                var receiver = await _context.People.FirstOrDefaultAsync(ob => ob.Id == UpOrder.ReciverId);

                if (department==null || sender==null || receiver == null)// somethig is wrong
                {
                    if (department == null)
                        Messages.Add($"There is no department with given id={UpOrder.DepartmentId}");

                    if (sender == null)
                        Messages.Add($"There is no person(sender) with given id={UpOrder.SenderId}");

                    if (receiver == null)
                        Messages.Add($"There is no person(reciver) with given id={UpOrder.ReciverId}");

                    return NotFound(Messages);
                }
                order.department = department;
                order.Sender = sender;
                order.Receiver = receiver;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Order updated succesfully");
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
                Messages.Add($"There is no order with certain id={id}");
                return NotFound(Messages);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var orderToDelete = await _context.Orders.FirstOrDefaultAsync(ob => ob.Id == id);
            if (orderToDelete == null)
            {
                Messages.Add("Order with certain id was not found");
                return NotFound(Messages);
            }

            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Department deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Order order)
        {
            List<string> Messages = new List<string>();
            if (order == null)
            {
                Messages.Add("Recived order is null");
                return NotFound(Messages);
            }
            Order NewOrder = order;

            Person sender = await _context.People.FirstOrDefaultAsync(ob=>ob.Id==order.SenderId);
            Person receiver = await _context.People.FirstOrDefaultAsync(ob => ob.Id == order.ReciverId);
            Pack pack = await _context.Packs.FirstOrDefaultAsync(ob => ob.Id == order.PackId);
            Department department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == order.DepartmentId);

            if (sender == null)
                Messages.Add("Choosed sender doesnt exist");

            if (receiver == null)
                Messages.Add("Choosed receiver  doesnt exist");

            if (pack == null)
                Messages.Add("Choosed pack doesnt exist");

            if (department == null)
                Messages.Add("Choosed department doesnt exist");

            if (Messages.Count != 0)
                return NotFound(Messages);

            order.department = department;
            order.Sender = sender;
            order.Receiver = receiver;
            order.pack = pack;

            await _context.AddAsync(NewOrder);
            await _context.SaveChangesAsync();
            Messages.Add("Order added succesfully");
            return Ok(Messages);
        }

        
    }
}
