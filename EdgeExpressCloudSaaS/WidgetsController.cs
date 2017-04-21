using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EdgeExpressCloudSaaS.Models;
using EdgeExpressCloudSaas.Models;

namespace EdgeExpressCloudSaaS
{
    public class WidgetsController : ApiController
    {
        private EdgeExpressCloudSaaSContext db = new EdgeExpressCloudSaaSContext();

        // GET: api/Widgets
        public IQueryable<Widget> GetWidgets()
        {
            return db.Widgets;
        }

        // GET: api/Widgets/5
        [ResponseType(typeof(Widget))]
        public async Task<IHttpActionResult> GetWidget(string id)
        {
            Widget widget = await db.Widgets.FindAsync(id);
            if (widget == null)
            {
                return NotFound();
            }

            return Ok(widget);
        }

        // PUT: api/Widgets/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutWidget(string id, Widget widget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != widget.Price)
            {
                return BadRequest();
            }

            db.Entry(widget).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WidgetExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Widgets
        [ResponseType(typeof(Widget))]
        public async Task<IHttpActionResult> PostWidget(Widget widget)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Widgets.Add(widget);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (WidgetExists(widget.Price))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = widget.Price }, widget);
        }

        // DELETE: api/Widgets/5
        [ResponseType(typeof(Widget))]
        public async Task<IHttpActionResult> DeleteWidget(string id)
        {
            Widget widget = await db.Widgets.FindAsync(id);
            if (widget == null)
            {
                return NotFound();
            }

            db.Widgets.Remove(widget);
            await db.SaveChangesAsync();

            return Ok(widget);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WidgetExists(string id)
        {
            return db.Widgets.Count(e => e.Price == id) > 0;
        }
    }
}