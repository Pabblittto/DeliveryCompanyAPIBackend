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
    public class PeopleController : ControllerBase
    {
        private readonly CompanyContext _context;

        public PeopleController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount = await _context.People.CountAsync();
            return Ok(amount);
        }

        [HttpGet("{id}")]// api/departments/id
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _context.People.FirstOrDefaultAsync(ob => ob.Id == id);

            if (person == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Person with certain id={id} not found");
                return NotFound(Messages);
            }

            return Ok(person);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Person>>> GetMany(int amount, int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Person> PartOfPeople = await _context.People.ToListAsync();// get all list
            try
            {
                if (PartOfPeople.Count != 0)// there is something in this list
                { 

                    if (PartOfPeople.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfPeople.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2 < 0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfPeople = PartOfPeople.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfPeople = PartOfPeople.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfPeople);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There are no people to display"
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

        [HttpPatch("{id}")]// api/Packs/Update
        public async Task<ActionResult> Update(int id, [FromBody] Person UpPerson)
        {

            List<string> Messages = new List<string>();

            if (id == 0 || UpPerson == null)
            {
                Messages.Add("ID or Person object is null");
                return BadRequest(Messages);
            }

            var person = await _context.People.FirstOrDefaultAsync(ob => ob.Id == id);

            if (person != null)
            {
                if (person.BuildingNo != UpPerson.BuildingNo) { person.BuildingNo = UpPerson.BuildingNo; }
                if (person.City != UpPerson.City) { person.City = UpPerson.City; }
                if (person.Name != UpPerson.Name) { person.Name = UpPerson.Name; }
                if (person.Street != UpPerson.Street) { person.Street = UpPerson.Street; }
                if (person.Surname != UpPerson.Surname) { person.Surname = UpPerson.Surname; }
                if (person.TelNo != UpPerson.TelNo) { person.TelNo = UpPerson.TelNo; }


                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount == 1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Person updated succesfully");
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
                Messages.Add($"There is no person with certain id={id}");
                return NotFound(Messages);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var personToDelete = await _context.People.FirstOrDefaultAsync(ob => ob.Id == id);
            if (personToDelete == null)
            {
                Messages.Add("Person with certain id was not found");
                return NotFound(Messages);
            }

            _context.People.Remove(personToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Person deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Person person)
        {
            List<string> Messages = new List<string>();
            if (person == null)
            {
                Messages.Add("Recived person is null");
                return NotFound(Messages);
            }

            Person NewPerson = person;


            await _context.AddAsync(NewPerson);
            await _context.SaveChangesAsync();
            Messages.Add("Person added succesfully");
            return Ok(Messages);
        }


    }
}
