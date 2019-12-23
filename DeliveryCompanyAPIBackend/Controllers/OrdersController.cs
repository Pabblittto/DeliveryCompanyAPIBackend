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
                ModelState.AddModelError("errors",$"Order with certain id={id} not found");
                return NotFound(ModelState);
            }

            return Ok(order);
        }


        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Order>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                ModelState.AddModelError("errors","Problems occured. You send amount = 0");
                return NotFound(ModelState);
            }

            List<Order> PartOfOrders = await _context.Orders.ToListAsync();// get all list
            try
            {
                if (PartOfOrders.Count != 0)// there is something in this list
                {
                    if (PartOfOrders.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfOrders.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            ModelState.AddModelError("errors", $"There are too few records in base to create {page} page.(Wrong page number)");
                            return NotFound(ModelState);
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
                    ModelState.AddModelError("errors", "There are no Orders to display");
                    return NotFound(ModelState);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("errors",$"Some seroius problems occured. You send {amount} and {page} as numbers");
                return NotFound(ModelState);
            }

        }

        [HttpPatch("{id}")]// api/Departments/Update
        public async Task<ActionResult> Update(int id, [FromBody] Order UpOrder)
        {

            if (id == 0 || UpOrder == null)
            {
                ModelState.AddModelError("errors","ID or Department object is null");
                return BadRequest(ModelState);
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
                        ModelState.AddModelError("errors", $"There is no department with given id={UpOrder.DepartmentId}");

                    if (sender == null)
                        ModelState.AddModelError("errors", $"There is no person(sender) with given id={UpOrder.SenderId}");


                    if (receiver == null)
                        ModelState.AddModelError("errors", $"There is no person(reciver) with given id={UpOrder.ReciverId}");

                    return NotFound(ModelState);
                }
                order.department = department;
                order.Sender = sender;
                order.Receiver = receiver;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        string OkMessage = "Order updated succesfully";
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
                ModelState.AddModelError("errors",$"There is no order with certain id={id}");
                return NotFound(ModelState);
            }
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var orderToDelete = await _context.Orders.FirstOrDefaultAsync(ob => ob.Id == id);
            if (orderToDelete == null)
            {
                ModelState.AddModelError("errors","Order with certain id was not found");
                return NotFound(ModelState);
            }

            _context.Orders.Remove(orderToDelete);
            await _context.SaveChangesAsync();

            string OkMessage = "Department deleted succesfully";
            return Ok(OkMessage);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Order order)
        {
            if (order == null)
            {
                ModelState.AddModelError("errors","Recived order is null");
                return NotFound(ModelState);
            }
            Order NewOrder = order;

            Person sender = await _context.People.FirstOrDefaultAsync(ob=>ob.Id==order.SenderId);
            Person receiver = await _context.People.FirstOrDefaultAsync(ob => ob.Id == order.ReciverId);
            Pack pack = await _context.Packs.FirstOrDefaultAsync(ob => ob.Id == order.PackId);
            Department department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == order.DepartmentId);

            if (sender == null)
                ModelState.AddModelError("errors","Choosed sender doesnt exist");

            if (receiver == null)
                ModelState.AddModelError("errors","Choosed receiver  doesnt exist");

            if (pack == null)
                ModelState.AddModelError("errors","Choosed pack doesnt exist");

            if (department == null)
                ModelState.AddModelError("errors","Choosed department doesnt exist");

            if (ModelState.ErrorCount != 0)
                return NotFound(ModelState);

            order.department = department;
            order.Sender = sender;
            order.Receiver = receiver;
            order.pack = pack;

            await _context.AddAsync(NewOrder);
            await _context.SaveChangesAsync();
            string OkMessage = "Order added succesfully";
            return Ok(OkMessage);
        }

        
    }
}
