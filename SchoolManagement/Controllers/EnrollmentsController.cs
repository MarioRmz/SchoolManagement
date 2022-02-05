using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Models;

namespace SchoolManagement.Controllers
{
    public class EnrollmentsController : Controller
    {
        private SchoolManagement_DBEntities db = new SchoolManagement_DBEntities();

        // GET: Enrollments
        public async Task<ActionResult> Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Student).Include(e => e.Lecturer);
            return View(await enrollments.ToListAsync());
        }

        //Metodo para la partial view _enrollmentPartial
        public PartialViewResult _enrollmentPartial(int? courseid)
        {
            var enrollments = db.Enrollments.Where(q => q.CourseID == courseid)
                .Include(e => e.Course)
                .Include(e => e.Student);
            return PartialView(enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
        public ActionResult Create()
        {
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName");
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName");
            return View();
        }

        // POST: Enrollments/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,LecturerId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        [HttpPost]
        public async Task<JsonResult> AddStudent([Bind(Include = "CourseID,StudentID")] Enrollment enrollment)
        {
            try
            {
                var isEnrolled = db.Enrollments.Any(q => q.CourseID == enrollment.CourseID && q.StudentID == enrollment.StudentID);

                if (ModelState.IsValid && !isEnrolled)
                {
                    db.Enrollments.Add(enrollment);
                    await db.SaveChangesAsync();
                    return Json(new { IsSuccess = true, Message = "Estudiante agregado exitosamente" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { IsSuccess = false, Message = "El estudiante no pudo ser agregado, o ya fue registrado previamente" }, 
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Error de Sistema: Favor de comunicarse con tu Administrador" }, 
                    JsonRequestBehavior.AllowGet);
            }
        }

        // GET: Enrollments/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EnrollmentID,Grade,CourseID,StudentID,LecturerId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CourseID = new SelectList(db.Courses, "CourseID", "Title", enrollment.CourseID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "LastName", enrollment.StudentID);
            ViewBag.LecturerId = new SelectList(db.Lecturers, "Id", "FirstName", enrollment.LecturerId);
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Enrollment enrollment = await db.Enrollments.FindAsync(id);
            db.Enrollments.Remove(enrollment);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetStudents(string term)
        {
            //students es un objeto abstracto trayendo el nombre completo del estudiante y StudentID
            var students = db.Students.Select(q => new
            {
                Name = q.FirstName + " " + q.LastName,
                Id = q.StudentID
            }).Where(q => q.Name.Contains(term));
            return Json(students, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
