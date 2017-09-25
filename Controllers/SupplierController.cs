using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ClipperDBAccess;
using System.Web.Http.Description;

namespace ClipperCafeERP.Controllers
{
    public class SupplierController : ApiController
    {
        /// <summary>
        /// get all supplier data
        /// </summary>
        /// <returns></returns>
        /// 
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/supplier/authorize/get")]
        public IEnumerable<SupplierDetail> Get()
        {
            using (ClipperDBEntities entities = new ClipperDBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.SupplierDetails.ToList();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("api/supplier/authorize/get/{id}")]
        public IHttpActionResult Get(int id)
        {
            var context = new ClipperDBEntities();
            SupplierDetail supplier = context.SupplierDetails.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            else
            {
                context.Configuration.ProxyCreationEnabled = false;
                var supplierFound = context.SupplierDetails.First(e => e.SupplierDetailsID == id);
                return Ok(supplierFound);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/supplier/authorize/postsuppliers")]
        public IHttpActionResult Post(List<SupplierDetail> supplierDataReceived)
        {

            SupplierDetail supplier = new SupplierDetail();
            var context = new ClipperDBEntities();
            foreach (SupplierDetail supplierData in supplierDataReceived)
            {
                context.SupplierDetails.Add(supplierData);
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
                return InternalServerError(dbEx);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("api/supplier/authorize/postsupplier")]
        public IHttpActionResult PostSupplier(SupplierDetail supplierReceived)
        {
            var context = new ClipperDBEntities();
            context.SupplierDetails.Add(supplierReceived);
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
        [Route("api/supplier/authorize/delete/{id}")]
        public IHttpActionResult Delete(int id)
        {
            var context = new ClipperDBEntities();
            SupplierDetail Supplier = context.SupplierDetails.Find(id);
            if (Supplier == null)
            {
                return NotFound();
            }
            else
            {
                context.SupplierDetails.Remove(Supplier);
                context.SaveChanges();
                return Ok(Supplier);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("api/supplier/authorize/put/{id}")]
        public IHttpActionResult Put(int id, SupplierDetail supplier)
        {
            var context = new ClipperDBEntities();
            SupplierDetail SupplierStatus = context.SupplierDetails.Find(id);
            if (supplier == null)
            {
                return NotFound();
            }
            else if (supplier == null)
            {
                ArgumentNullException argumentNullException = new ArgumentNullException();
                return InternalServerError(argumentNullException);
            }
            else
            {
                SupplierDetail SupplierUpdate = context.SupplierDetails
                  .Where(e => e.SupplierDetailsID == supplier.SupplierDetailsID).FirstOrDefault();

                if (SupplierUpdate != null)
                {
                    context.Entry(SupplierUpdate).CurrentValues.SetValues(supplier);
                }
            }
            context.SaveChanges();
            return Ok(supplier);
        }
    }
}

