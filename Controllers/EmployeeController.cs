using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;
using System.Security.Claims;

namespace ClipperCafeERP.Controllers
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        [Route("api/employee/authorize")]
        public IEnumerable<Employee> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                var test = entities.Employees.ToList();
                return entities.Employees.ToList();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/employee/authorize/{id}")]
        public Employee Get(int id)
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Employees.First(e => e.EmpID == id);
            }
        }
        [HttpPost]
        [Route("api/employee/authorize/post")]
        public IHttpActionResult Post(Employee emp)
        {
            UserAccount userAccount = new UserAccount();
            Employee employee = new Employee();
            employee.EmpID = emp.EmpID;
            employee.FirstName = emp.FirstName;
            employee.LastName = emp.LastName;
            employee.EmpRole = emp.EmpRole;
            employee.Phone = emp.Phone;
            employee.Email = emp.Email;
            userAccount.UserRole = emp.EmpRole;
            userAccount.UserName = emp.UserName;
            userAccount.Password = emp.Password;
            employee.UserAccount = userAccount;
            var context = new ClipperDBEntities();
            context.Employees.Add(employee);
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
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/employee/authorize/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            Employee employee = context.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
                return Ok(employee);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]       
        [Route("api/employee/authorize/{id}/{employee}")]
        public IHttpActionResult Put(int id, Employee employee)
        {
            var context = new ClipperDBEntities();
            Employee employeeStatus = context.Employees.Find(id);
            if (employeeStatus == null)
            {
                return NotFound();
            }
            else if (employee == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                if (employee.EmpID == 0)
                {
                    context.Employees.Add(employee);
                }
                else
                {
                    Employee employeeUpdate = context.Employees
                      .Where(e => e.EmpID == employee.EmpID).FirstOrDefault();

                    if (employeeUpdate != null)
                    {
                        context.Entry(employeeUpdate).CurrentValues.SetValues(employee);
                    }
                }
                context.SaveChanges();
                return Ok();
            }
        }
    }
}
