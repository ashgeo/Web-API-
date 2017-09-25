using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;

namespace ClipperCafeERP.Controllers
{
    public class UserAccountController : ApiController
    {
        /// <summary>
        /// get used data 
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/UserAccount/authorize")]
        public IEnumerable<UserAccount> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                
                return entities.UserAccounts.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/UserAccount/authenticate")]
        public IHttpActionResult Get(string userName,string password)
        {
            var context = new ClipperDBEntities();
            UserAccount userAccount = context.UserAccounts.Find(userName);
            if (userAccount == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var userFound = context.UserAccounts.First(e => e.UserName == userName && e.Password == password);
                if (userFound != null)
                {
                    return Ok();
                }
                else
                {
                     return NotFound();
                }
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/useraccount/validate-username")]
        public IHttpActionResult Get(string userName)
        {
            var context = new ClipperDBEntities();
            UserAccount userAccount = context.UserAccounts.Where(b => b.UserName == userName)
                    .FirstOrDefault();
            if (userAccount == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var userFound = context.UserAccounts.First(e => e.UserName == userName );
                if (userFound != null)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }


        ///Follwing functions are not required as it perform from Employe controller

        //public IHttpActionResult Post(UserAccount userDataReceived)
        //{

        //    UserAccount supplier = new UserAccount();
        //    var context = new ClipperDBEntities();
        //    if (userDataReceived != null)
        //    {
        //        context.UserAccounts.Add(userDataReceived);

        //        try
        //        {
        //            context.SaveChanges();
        //            return Ok();
        //        }
        //        catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
        //        {
        //            Exception raise = dbEx;
        //            foreach (var validationErrors in dbEx.EntityValidationErrors)
        //            {
        //                foreach (var validationError in validationErrors.ValidationErrors)
        //                {
        //                    string message = string.Format("{0}:{1}",
        //                        validationErrors.Entry.Entity.ToString(),
        //                        validationError.ErrorMessage);
        //                    raise = new InvalidOperationException(message, raise);
        //                }
        //            }
        //            throw raise;
        //        }
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //public void Delete(DateTime date)
        //{
        //    var context = new ClipperDBEntities();
        //    Sale sale = context.Sales.Find(date);
        //    context.Sales.Remove(sale);
        //    context.SaveChanges();
        //}
        //public void Put(int id, Sale sale)
        //{
        //    var context = new ClipperDBEntities();
        //    if (sale.Date == null)
        //    {
        //        context.Sales.Add(sale);
        //    }
        //    else
        //    {
        //        Sale salesUpdate = context.Sales
        //          .Where(e => e.Date == sale.Date).FirstOrDefault();

        //        if (salesUpdate != null)
        //        {
        //            context.Entry(salesUpdate).CurrentValues.SetValues(sale);
        //        }
        //    }

        //    context.SaveChanges();
        //}



    }
}
