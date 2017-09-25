using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ClipperCafeERP.Controllers
{
    public class RolesController : ApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/employeerole/authorize/get")]
        public IEnumerable<Role> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Roles.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/employeerole/authorize/{id}")]
        public IHttpActionResult Get(int id)
        {
            var context = new ClipperDBEntities();
            Role role = context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var roleFound = context.Roles.First(e => e.RoleID == id);
                return Ok(roleFound);
            }
        }

        /// <summary>
        /// Post
        /// </summary>
        /// <param name="roleData"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        [Route("api/employeerole/authorize/{role}")]
        public IHttpActionResult Post(Role roleData)
        {
            Role role = new Role();
            Salary salaryReceived = new Salary();
            role.RoleID = roleData.RoleID;
            role.RoleName = roleData.RoleName;
            salaryReceived.RoleID = roleData.RoleID;
            salaryReceived.BasicSalary = roleData.Salary.BasicSalary;
            salaryReceived.Allowance = roleData.Salary.Allowance;
            salaryReceived.SalaryRateMF = roleData.Salary.SalaryRateMF;
            salaryReceived.SalaryRateSat = roleData.Salary.SalaryRateSat;
            salaryReceived.SalaryRateSun = roleData.Salary.SalaryRateSun;
            salaryReceived.SalaryRatePH = roleData.Salary.SalaryRatePH;
            role.Salary = salaryReceived;
            try
            {
                var context = new ClipperDBEntities();
                context.Roles.Add(role);
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
        [Route("api/employeerole/authorize/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            Role role = context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }
            else
            {
                context.Roles.Remove(role);
                context.SaveChanges();
                return Ok(role);
            }

        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/employeerole/authorize/{id}/{role}")]
        public IHttpActionResult Put(int id, Role role)
        {
            var context = new ClipperDBEntities();
            Role roleStatus = context.Roles.Find(id);
            if (roleStatus == null)
            {
                return NotFound();
            }
            else if (role == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                Role rolesUpdate = context.Roles
                    .Where(e => e.RoleID == role.RoleID).FirstOrDefault();
                if (rolesUpdate != null)
                {
                    context.Entry(role).CurrentValues.SetValues(role);
                }
            }
            context.SaveChanges();
            return Ok(role);
        }
    }
}
