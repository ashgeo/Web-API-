using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;

namespace ClipperCafeERP.Controllers
{
    public class SalesBalanceController : ApiController
    {
        /// <summary>
        /// Get all sales and expense data
        /// </summary>
        /// <returns></returns>
        /// 
        //[Authorize]
        //[HttpGet]
        //[Route("api/employees/authenticate")]
        [HttpGet]
        [Route("api/salesbalance/authorize")]
        public IEnumerable<SaleExpense> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.SaleExpenses.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/salesbalance/authorize/{id}")]
        public IHttpActionResult Get(DateTime saleDate)
        {
            var context = new ClipperDBEntities();
            SaleExpense saleExpense = context.SaleExpenses.Find(saleDate);
            if (saleExpense == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var salesExpenseFoundFound = context.SaleExpenses.First(e => e.Date == saleDate);
                return Ok(salesExpenseFoundFound);
            }
        }
        [HttpPost]
        [Route("api/salesbalance/authorize/{salesbalance}")]
        public IHttpActionResult Post(SaleExpense salesBalance)
        {           
            var context = new ClipperDBEntities();
            context.SaleExpenses.Add(salesBalance);           
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
        [Route("api/salesbalance/authorize/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            SaleExpense salesExpense = context.SaleExpenses.Find(id);
            if (salesExpense == null)
            {
                return NotFound();
            }
            else
            {
                context.SaleExpenses.Remove(salesExpense);
                context.SaveChanges();
                return Ok(salesExpense);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/salesbalance/authorize/{id}/{salesbalance}")]
        public IHttpActionResult Put(int id, SaleExpense salesExpense)
        {
            var context = new ClipperDBEntities();
            Sale salesExpenseStatus = context.Sales.Find(id);
            if (salesExpenseStatus == null)
            {
                return NotFound();
            }
            else if (salesExpense == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {              
                SaleExpense salesExpenseUpdate = context.SaleExpenses
                  .Where(e => e.SalesExpenseID == salesExpense.SalesExpenseID).FirstOrDefault();
                if (salesExpenseUpdate != null)
                {
                    context.Entry(salesExpense).CurrentValues.SetValues(salesExpense);
                }
            }
            context.SaveChanges();
            return Ok(salesExpense);

        }
    }
}
