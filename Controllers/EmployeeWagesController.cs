using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;

namespace ClipperCafeERP.Controllers
{
    public class EmployeeWagesController : ApiController
    {

        [HttpGet]
        [Route("api/employeewages/authorize")]
        public IEnumerable<SalaryPayment> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.SalaryPayments.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/employeewages/authorize/{id}")]
        public IHttpActionResult Get(int id)
        {

            var context = new ClipperDBEntities();
            SalaryPayment salary = context.SalaryPayments.Find(id);
            if (salary == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var salaryFound = context.SalaryPayments.First(e => e.EmpID == id);
                return Ok(salaryFound);
            }
        }
        [HttpPost]
        [Route("api/employeewages/authorize/{wages}")]
        public IHttpActionResult Post(List<SalaryPayment> SalaryPaymentReceived)
        {

            SalaryPayment userAccount = new SalaryPayment();
            var context = new ClipperDBEntities();

            foreach (SalaryPayment payment in SalaryPaymentReceived)
            {
                context.SalaryPayments.Add(payment);
            }
            try
            {
                context.SaveChanges();
                return Ok();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                        validationErrors.Entry.Entity.ToString(),
                        validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/employeewages/authorize/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            SalaryPayment salaryPay = context.SalaryPayments.Find(id);
            if (salaryPay == null)
            {
                return NotFound();
            }
            else
            {
                context.SalaryPayments.Remove(salaryPay);
                context.SaveChanges();
                return Ok(salaryPay);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/employeewages/authorize/{id}/{wages}")]
        public IHttpActionResult Put(int id, SalaryPayment salaryPayement)
        {
            var context = new ClipperDBEntities();
            SalaryPayment salaryPayStatus = context.SalaryPayments.Find(id);
            if (salaryPayStatus == null)
            {
                return NotFound();
            }
            else if (salaryPayement == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                if (salaryPayement.EmpID == 0)
                {
                    context.SalaryPayments.Add(salaryPayement);
                }
                else
                {
                    Employee employeeUpdate = context.Employees
                      .Where(e => e.EmpID == salaryPayement.EmpID).FirstOrDefault();

                    if (employeeUpdate != null)
                    {
                        context.Entry(employeeUpdate).CurrentValues.SetValues(salaryPayement);
                    }
                }

                context.SaveChanges();
                return Ok();
            }
        }






    }
}
