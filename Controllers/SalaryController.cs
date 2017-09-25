using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;

namespace ClipperCafeERP.Controllers
{
    public class SalaryController : ApiController
    {
        /// <summary>
        /// Get Salary Details 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/salary/authorize")]
        public IEnumerable<Salary> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Salaries.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/salary/authorize/{id}")]
        public IHttpActionResult Get(int id)
        {
            var context = new ClipperDBEntities();
            SaleExpense salaryRates = context.SaleExpenses.Find(id);
            if (salaryRates == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var slaryFound = context.Salaries.First(e => e.RoleID == id);
                return Ok(slaryFound);
            }

        }


        //Salary details will be added when add the role of the employee so that 
        //Bellow register code does not required


        //public void Register(Salary emp)
        //{
        //  }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/salary/authorize/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            Salary salary = context.Salaries.Find(id);
            if (salary == null)
            {
                return NotFound();
            }
            else
            {
                context.Salaries.Remove(salary);
                context.SaveChanges();
                return Ok(salary);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/staff/authorize/{id}/{salary}")]
        public IHttpActionResult Put(int id, Salary salary)
        {
            var context = new ClipperDBEntities();
            Salary salariesStatus = context.Salaries.Find(id);
            if (salariesStatus == null)
            {
                return NotFound();
            }
            else if (salary == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                Salary employeeSalaryUpdate = context.Salaries
                    .Where(e => e.RoleID == salary.RoleID).FirstOrDefault();
                if (employeeSalaryUpdate != null)
                {
                    context.Entry(salary).CurrentValues.SetValues(salary);
                }
            }
            context.SaveChanges();
            return Ok(salary);
        }
    }
}
