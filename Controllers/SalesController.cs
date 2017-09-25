using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;

namespace ClipperCafeERP.Controllers
{
    public class SalesController : ApiController
    {
        /// <summary>
        /// Get all sales data;
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/sales/authorize/get")]
        public IEnumerable<Sale> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Sales.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/sales/authorize/get/{date}")]
        public IHttpActionResult  Get(DateTime date)
        {            
            var context = new ClipperDBEntities();
            Sale sale = context.Sales.Find(date);
            if (sale == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var salesFoundFound = context.Sales.First(e => e.Date == date);
                return Ok(salesFoundFound);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/sales/authorize/postsales")]
        public IHttpActionResult PostSales(List<Sale> SalesReceived)
        {
          
            Sale sale = new Sale();
            var context = new ClipperDBEntities();

            foreach (Sale saleData in SalesReceived)
            {
                sale = saleData;
                sale.Date = Convert.ToDateTime(saleData.Date);             
                context.Sales.Add(sale);
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
            catch (Exception ex)
            {
                // Exception raise = new Exception(HandleException(ex));
                Exception raise = ex.InnerException;
                throw raise;
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/sales/authorize/postsale")]
        public IHttpActionResult PostSale(Sale salesReceived)
        {   
            var context = new ClipperDBEntities();
            context.Sales.Add(salesReceived);          
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
                // Exception raise = new Exception(HandleException(ex));
                Exception raise = ex.InnerException;
                throw raise;
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/sales/authorize/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            Sale sale = context.Sales.Find(id);
            if (sale == null)
            {
                return NotFound();
            }
            else
            {
                context.Sales.Remove(sale);
                context.SaveChanges();
                return Ok(sale);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/sales/authorize/put/{id}")]
        public IHttpActionResult Put(int id, Sale sale)
        {
            var context = new ClipperDBEntities();
            Sale saleStatus = context.Sales.Find(id);
            if (saleStatus == null)
            {
                return NotFound();
            }
            else if (sale == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                Sale salesUpdate = context.Sales
                .Where(e => e.SaleID == sale.SaleID).FirstOrDefault();
                if (salesUpdate != null)
                {
                    context.Entry(salesUpdate).CurrentValues.SetValues(sale);
                }
            }
            context.SaveChanges();
            return Ok(sale);
        }
    }
}
