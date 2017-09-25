using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.SqlClient;
namespace ClipperCafeERP.Controllers
{
    public class EmployeesController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/employees/authorize/get")]
        public IEnumerable<Employee> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;                
                return entities.Employees.ToList();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/employees/authorize/get/{id}")]
        public Employee Get(int id)
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Employees.First(e => e.EmpID == id);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/employees/authorize/post")]
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
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }           
            catch (Exception ex)
            {
                Exception raise = new Exception(HandleException(ex));
                throw raise;
            }        
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/employees/authorize/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            Employee employee = context.Employees.Find(id);
            UserAccount userAccount = context.UserAccounts.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            else if(userAccount==null)
            {
                return NotFound();
            }
            else
            {
                context.Employees.Remove(employee);
                context.UserAccounts.Remove(userAccount);
                context.SaveChanges();
                return Ok(employee);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/employees/authorize/put/{id}")]
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
                if (employee.EmpID == 0&& employeeStatus== null)
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
        public virtual string HandleException(Exception exception)
        {
             string RetrunValue = "";
            DbUpdateConcurrencyException concurrencyEx = exception as DbUpdateConcurrencyException;
            if (concurrencyEx != null)
            {
                // A custom exception of yours for concurrency issues
                throw new DBConcurrencyException();
            }

            DbUpdateException dbUpdateEx = exception as DbUpdateException;
            if (dbUpdateEx != null)
            {
                if (dbUpdateEx.InnerException != null
                        && dbUpdateEx.InnerException.InnerException != null)
                {
                    SqlException sqlException = dbUpdateEx.InnerException.InnerException as SqlException;
                    if (sqlException != null)
                    {
                        switch (sqlException.Number)
                        {
                            case 2627:  // Unique constraint error
                                RetrunValue= "Unique constraint error";
                                break;
                            case 547:
                                RetrunValue= "Constraint check violationr";// 
                                break;
                            case 2601:
                                RetrunValue = "Duplicate key Error";
                                break;

                            default:
                                return dbUpdateEx.InnerException.ToString();
                                // A custom exception of yours for other DB issues

                        }
                    }

                   
                }
            }
            return RetrunValue;
        }
    }
}
