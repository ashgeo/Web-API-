using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;

namespace ClipperCafeERP.Controllers
{
    public class SupplierNameController : ApiController
    {
        /// <summary>
        /// Get all supplier names 
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet]
        [Route("api/suppliername/authorize")]
        public IEnumerable<SupplierName> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.SupplierNames.ToList();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/suppliername/authorize/{id}")]
        public IHttpActionResult Get(int id)
        {   var context = new ClipperDBEntities();
            SupplierName supplierName = context.SupplierNames.Find(id);
            if (supplierName == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var supplierNamesFound = context.SupplierNames.First(e => e.SupplierID == id);
                return Ok(supplierNamesFound);
            }
        }

        [HttpPost]
        [Route("api/suppliername/authorize/{suppliername}")]
        public IHttpActionResult Post(SupplierName supplierName)
        {
            SupplierName supplierNames = new SupplierName();
            
            var context = new ClipperDBEntities();
            context.SupplierNames.Add(supplierName);           
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
                return InternalServerError(dbEx);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("api/suppliername/authorize/{id}")]
        public IHttpActionResult Delete(int id)
        {   var context = new ClipperDBEntities();
            SupplierName supplierName = context.SupplierNames.Find(id);
            if (supplierName == null)
            {
                return NotFound();
            }
            else
            {
                context.SupplierNames.Remove(supplierName);
                context.SaveChanges();
                return Ok(supplierName);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/suppliername/authorize/{id}/{suppliername}")]
        public IHttpActionResult Put(int id, SupplierName supplierName)
        {   var context = new ClipperDBEntities();
            SupplierName supplierNameStatus = context.SupplierNames.Find(id);
            if (supplierNameStatus == null)
            {
                return NotFound();
            }
            else if(supplierName== null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                SupplierName supplierNamesUpdate = context.SupplierNames
                  .Where(e => e.SupplierID == supplierName.SupplierID).FirstOrDefault();

                if (supplierNamesUpdate != null)
                {
                    context.Entry(supplierNamesUpdate).CurrentValues.SetValues(supplierName);
                }
            }
            context.SaveChanges();
            return Ok(supplierName);
        }


    }
}
