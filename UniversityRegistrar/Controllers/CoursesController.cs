using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using UniversityRegistrar.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UniversityRegistrar.Controllers
{
  public class CoursesController : Controller
  {
    private readonly UniversityRegistrarContext _db;

    public CoursesController(UniversityRegistrarContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Course> model = _db.Courses.ToList();
      return View(model);
    }

    public ActionResult Create()
    {
      // ViewBag.CourseId = new SelectList(_db.Courses, "CourseId", "CourseName");
      ViewBag.DepartmentId = new SelectList(_db.Departments, "DepartmentId", "DepartmentName");
      return View();
    }

    [HttpPost]
    public ActionResult Create(Course course, int DepartmentId)
    {
      _db.Courses.Add(course);
      _db.SaveChanges();
      if(DepartmentId != 0)
      {
        _db.CourseDepartment.Add(new CourseDepartment() { DepartmentId = DepartmentId, CourseId = course.CourseId});
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
    public ActionResult Details(int id)
    {
      var thisCourse = _db.Courses
          .Include(course => course.JoinEntities1)
          .ThenInclude(join => join.Student)
          .FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }
    public ActionResult Edit(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    [HttpPost]
    public ActionResult Edit(Course course)
    {
      _db.Entry(course).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      return View(thisCourse);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisCourse = _db.Courses.FirstOrDefault(course => course.CourseId == id);
      _db.Courses.Remove(thisCourse);
      // var registeredCourse = _db.CourseStudent.FirstOrDefault(courseStudent => courseStudent.CourseId == id);
      // _db.CourseStudent.Remove(registeredCourse);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}