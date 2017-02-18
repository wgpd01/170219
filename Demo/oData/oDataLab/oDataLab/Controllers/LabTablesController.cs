using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using oDataLab.Models;

namespace oDataLab.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using oDataLab.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<LabTable>("LabTables");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class LabTablesController : ODataController
    {
        private LabDBEntities db = new LabDBEntities();

        // GET: odata/LabTables
        [EnableQuery]
        public IQueryable<LabTable> GetLabTables()
        {
            return db.LabTables;
        }

        // GET: odata/LabTables(5)
        [EnableQuery]
        public SingleResult<LabTable> GetLabTable([FromODataUri] int key)
        {
            return SingleResult.Create(db.LabTables.Where(labTable => labTable.id == key));
        }

        // PUT: odata/LabTables(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<LabTable> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LabTable labTable = db.LabTables.Find(key);
            if (labTable == null)
            {
                return NotFound();
            }

            patch.Put(labTable);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabTableExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(labTable);
        }

        // POST: odata/LabTables
        public IHttpActionResult Post(LabTable labTable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.LabTables.Add(labTable);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LabTableExists(labTable.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(labTable);
        }

        // PATCH: odata/LabTables(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<LabTable> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            LabTable labTable = db.LabTables.Find(key);
            if (labTable == null)
            {
                return NotFound();
            }

            patch.Patch(labTable);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabTableExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(labTable);
        }

        // DELETE: odata/LabTables(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            LabTable labTable = db.LabTables.Find(key);
            if (labTable == null)
            {
                return NotFound();
            }

            db.LabTables.Remove(labTable);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LabTableExists(int key)
        {
            return db.LabTables.Count(e => e.id == key) > 0;
        }
    }
}
