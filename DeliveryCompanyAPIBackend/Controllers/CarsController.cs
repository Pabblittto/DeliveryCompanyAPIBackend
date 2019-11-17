﻿using System;
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
    public class CarsController : ControllerBase
    {
        private readonly CompanyContext _context;

        public CarsController(CompanyContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAmount()
        {
            var amount =  await _context.Cars.CountAsync();
            return Ok(amount);
        }


        [HttpGet("{id}")]// api/Cars/id
        public async Task<ActionResult<Car>> GetCar(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(ob => ob.RegistrationNumber == id);

            if (car == null)
            {
                List<string> Messages = new List<string>();
                Messages.Add("Car with certain id not found");
                return NotFound(Messages);
            }

            return Ok(car);
        }

        [HttpGet("am={amount}/pg={page}")]
        public async Task<ActionResult<ICollection<Car>>> GetCars(int amount,int page)
        {
            if (amount == 0)
            {
                List<string> Messages = new List<string>() { "Problems occured. You send amount = 0" };
                return NotFound(Messages);
            }

            List<Car> PartOfCars = await _context.Cars.ToListAsync();// get all list
            try
            {
                if (PartOfCars.Count != 0)// there is something in this list
                {
                    //page-=1;// because index is counted from 0

                    if (PartOfCars.Count < (page) * amount)// if there is less elemnts for one page - take less elements
                    {
                        int amount2 = PartOfCars.Count - (page - 1) * amount;// number of elements less than it should be
                        if (amount2<0)
                        {
                            List<string> Messages = new List<string>()
                            {
                                $"There are too few records in base to create {page} page.(Wrong page number)"
                            };
                            return NotFound(Messages);
                        }
                        PartOfCars = PartOfCars.GetRange((page - 1) * amount, amount2);
                    }
                    else
                    {
                        PartOfCars = PartOfCars.GetRange((page - 1) * amount, amount);
                    }
                    return Ok(PartOfCars);
                }
                else
                {
                    List<string> Messages = new List<string>()
                {
                    "There is no Cars to display"
                };
                    return NotFound(Messages);
                }
            }catch(Exception e)
            {
                List<string> Messages = new List<string>();
                Messages.Add($"Some seroius problems occured. You send {amount} and {page} as numbers");
                return NotFound(Messages);
            }
            
        }

        [HttpPatch]// api/Cars/Update
        public async Task<ActionResult> Update(int id, Car UpCar)
        {

            List<string> Messages = new List<string>();

            if(id==0 || UpCar == null)
            {
                Messages.Add("ID or Car object is null");
                return BadRequest(Messages);
            }

            var car = await _context.Cars.FirstOrDefaultAsync(ob => ob.RegistrationNumber == id);
            if (car != null)
            {
                if (car.RegistrationNumber != UpCar.RegistrationNumber) { car.RegistrationNumber = UpCar.RegistrationNumber; }
                if (car.Mark != UpCar.Mark) { car.Mark = UpCar.Mark; }
                if (car.Model != UpCar.Model) { car.Model = UpCar.Model; }
                if (car.PolicyNumber != UpCar.PolicyNumber) { car.PolicyNumber = UpCar.PolicyNumber; }
                if (car.VIN != UpCar.VIN) { car.VIN = UpCar.VIN; }
                if (car.DepartmentId != UpCar.DepartmentId) { car.DepartmentId = UpCar.DepartmentId; }

                var department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == UpCar.DepartmentId);

                if (department == null)
                {
                    Messages.Add("There is no dapertment with given id");
                    return NotFound(Messages);
                }

                car.Department = department;

                try
                {
                    var ammount = await _context.SaveChangesAsync();

                    if (ammount==1)// amount means number of updated/added/etc. records
                    {
                        Messages.Add("Car updated succesfully");
                        return Ok(Messages);
                    }
                    else
                    {
                        Messages.Add($"{ammount} changes were made");
                        return Ok(Messages);
                    }

                }catch(Exception e)
                {
                    Messages.Add($"Some serous problems occured :c. You send {id} as ID");
                    return NotFound(Messages);
                }

            }
            else
            {
                Messages.Add("There is no car with certain id");
                return NotFound(Messages);
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            List<string> Messages = new List<string>();
            var carToDelete = await _context.Cars.FirstOrDefaultAsync(ob => ob.RegistrationNumber == id);
            if (carToDelete == null)
            {
                Messages.Add("Car with certain id was not found");
                return NotFound(Messages);
            }

            _context.Cars.Remove(carToDelete);
            await _context.SaveChangesAsync();

            Messages.Add("Car deleted succesfully");
            return Ok(Messages);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody]Car car)
        {
            List<string> Messages = new List<string>();
            if (car == null)
            {
                Messages.Add("Recived car is null");
                return NotFound(Messages);
            }
            Car NewCar = car;

            Department department = await _context.Departments.FirstOrDefaultAsync(ob => ob.Id == car.DepartmentId);

            if (department == null)
            {
                Messages.Add("You can not add car to department, which doesnt exists");
                return NotFound(Messages);
            }

            NewCar.Department = department;

            await _context.AddAsync(NewCar);
            Messages.Add("Car added succesfully");
            return Ok();

        }

        //// GET: api/Cars
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        //{
        //    return await _context.Cars.ToListAsync();
        //}

        //// GET: api/Cars/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Car>> GetCar(int id)
        //{
        //    var car = await _context.Cars.FindAsync(id);

        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    return car;
        //}

        //// PUT: api/Cars/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCar(int id, Car car)
        //{
        //    if (id != car.RegistrationNumber)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(car).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CarExists(id))
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

        //// POST: api/Cars
        //[HttpPost]
        //public async Task<ActionResult<Car>> PostCar(Car car)
        //{
        //    _context.Cars.Add(car);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCar", new { id = car.RegistrationNumber }, car);
        //}

        //// DELETE: api/Cars/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Car>> DeleteCar(int id)
        //{
        //    var car = await _context.Cars.FindAsync(id);
        //    if (car == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Cars.Remove(car);
        //    await _context.SaveChangesAsync();

        //    return car;
        //}

        //private bool CarExists(int id)
        //{
        //    return _context.Cars.Any(e => e.RegistrationNumber == id);
        //}
    }
}
