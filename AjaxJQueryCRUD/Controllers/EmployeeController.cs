using AjaxJQueryCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;

namespace AjaxJQueryCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewAll()
        {
            return View(GetAllEmployees());
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            using (AjaxJQueryDBEntities db = new AjaxJQueryDBEntities())
            {
                return db.Employees.ToList();
            }

        }

        public ActionResult AddOrEdit(int id = 0)
        {
            Employee employee = new Employee();
            if (id != 0)
            {
                using (AjaxJQueryDBEntities db = new AjaxJQueryDBEntities())
                {
                    employee = db.Employees.FirstOrDefault(x => x.EmployeeID == id);
                }
            }
            return View(employee);
        }

        [HttpPost]
        public JsonResult AddOrEdit(Employee employee)
        {
            try
            {
                if (employee.UpLoad != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(employee.UpLoad.FileName);
                    string extension = Path.GetExtension(employee.UpLoad.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    employee.ImagePath = @"~/Media/Images/" + fileName;
                    employee.UpLoad.SaveAs(Path.Combine(Server.MapPath("~/Media/Images/"), fileName));
                }
                using (AjaxJQueryDBEntities db = new AjaxJQueryDBEntities())
                {
                    if (employee.EmployeeID == 0)
                    {
                        db.Employees.Add(employee);
                        db.SaveChanges();
                    }
                    else
                    {
                        db.Entry(employee).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }
                return Json(new
                {
                    success = true,
                    html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployees()),
                    message = "Submitted Successfully"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                using (AjaxJQueryDBEntities db = new AjaxJQueryDBEntities())
                {
                    db.Employees.Remove(db.Employees.Find(id));
                    db.SaveChanges();
                    return Json(new
                    {
                        success = true,
                        html = GlobalClass.RenderRazorViewToString(this, "ViewAll", GetAllEmployees()),
                        message = "Deleted Successfully"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}