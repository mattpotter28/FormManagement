using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FormToPDF.Models;
using iTextSharp.text.pdf;
using System.IO;

namespace FormToPDF.Controllers
{
    public class FormsController : Controller
    {
        private FormToPDFContext db = new FormToPDFContext();

        // GET: Forms
        public ActionResult Index()
        {
            return View(db.Forms.ToList());
        }

        // GET: Forms/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // GET: Forms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Forms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Date,FormType,FormID")] Form form)
        {
            if (ModelState.IsValid)
            {
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("/Index");
            }

            return View("/Index");
        }

        // GET: Forms/Delete/5
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Form form = db.Forms.Find(id);
            if (form == null)
            {
                return HttpNotFound();
            }
            return View(form);
        }

        // POST: Forms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Form form = db.Forms.Find(id);
            System.IO.File.Delete(Server.MapPath("~/Forms/" + form.FormName));
            db.Forms.Remove(form);
            db.SaveChanges();
            return RedirectToAction("/Index");
        }

        public ActionResult Open()
        {
            return View();
        }

        public ActionResult ITRequest()
        {
            ITRequest objModel = new ITRequest();
            objModel.getLocations = LocationHandler.getLocationList();
            return View(objModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult HandleITRequest(ITRequest model)
        {
            model.ID = Guid.NewGuid();
            model.date = DateTime.Now;

            string P_InputStream = "ITRequest.pdf";
            string P_OutputStream = model.ID.ToString() + "ITRequest.pdf";
            string formFile = Server.MapPath(P_InputStream);
            string newFile = Server.MapPath(P_OutputStream);
            PdfReader reader = new PdfReader(formFile);
            using (PdfStamper stamper = new PdfStamper(reader, new FileStream(newFile, FileMode.Create)))
            {
                AcroFields fields = stamper.AcroFields;

                fields.SetField("txtRequestFor", model.name);
                fields.SetField("txtEmpID", model.empNum);
                fields.SetField("txtReqDate", model.date.ToShortDateString());

                fields.SetField("txtRequestBy", model.manager);
                fields.SetField("txtContactNum", model.contact);
                fields.SetField("txtLocation", model.location);

                if (model.status != null)
                    fields.SetField("chkEmpStatus", model.status);

                fields.SetField("txtShipAddress", model.address);

                if (model.priority != null)
                    fields.SetField("rdPriority", model.priority);

                if (model.tempDesired != null)
                    model.desired = DateTime.Parse(model.tempDesired);
                fields.SetField("txtDateDesired", model.desired.ToShortDateString());

                if (model.request == "Desktop")
                {
                    fields.SetField("chkDesktop", "Yes");
                    if (model.condition != null)
                        fields.SetField("chkDesk", model.condition);
                    fields.SetField("txtSpec1", model.dSpecs);
                }
                else if (model.request == "Laptop")
                {
                    fields.SetField("chkLaptop", "Yes");
                    fields.SetField("chkLap", model.condition);
                    if (model.tempReturn != null)
                        model.returnDate = DateTime.Parse(model.tempReturn);
                    fields.SetField("txtLapDateReturn", model.returnDate.ToShortDateString());
                    fields.SetField("txtSpec2", model.lSpecs);
                }
                else if (model.request == "Hardware")
                {
                    fields.SetField("chkHardware", "Yes");
                    fields.SetField("txtHardware", model.hardware);
                }
                else if (model.request == "App")
                {
                    fields.SetField("chkApp", "Yes");
                    fields.SetField("txtApp", model.app);
                }
                else if (model.request == "Other")
                {
                    fields.SetField("chkMove", "Yes");
                    fields.SetField("txtOther", model.other);
                }

                fields.SetField("txtComments", model.comments);

                // flatten form fields and close document
                stamper.FormFlattening = true;
                stamper.Close();
            }

            // TicketHandler.sendTicket(formFile);

            Form form = new Form()
            {
                ID = Guid.NewGuid(),
                Name = model.name,
                Date = model.date,
                FormType = "IT Request",
                FormName = model.ID.ToString() + "ITRequest.pdf"
            };

            if (ModelState.IsValid)
            {
                db.Forms.Add(form);
                db.SaveChanges();
                return RedirectToAction("/Index");
            }

            return View("/Index");
        }
    }
}
