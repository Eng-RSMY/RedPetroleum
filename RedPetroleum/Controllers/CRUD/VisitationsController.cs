using Microsoft.AspNet.Identity;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RedPetroleum.Controllers.CRUD
{
    [Authorize(Roles = "admin, manager")]
    public class VisitationsController : Controller
    {
        UnitOfWork unitOfWork;

        public VisitationsController()
        {
            this.unitOfWork = new UnitOfWork();
        }

        public VisitationsController(UnitOfWork unit)
        {
            this.unitOfWork = unit;
        }
        // GET: Visitations
        public ActionResult Index(int? page, string searching)
        {
            var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            
            var visitation = unitOfWork.Visitations.GetAllIndex(pageNumber, pageSize, searching);
            return View(visitation);
        }

        // GET: Visitations/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitation visitation = unitOfWork.Visitations.Get(id);
            ViewBag.DepName = unitOfWork.Visitations.GetDepartmentName(visitation);
            ViewBag.EmployeeName = unitOfWork.Visitations.GetEmployeeName(visitation);
            if(visitation == null)
            {
                return HttpNotFound();
            }
            return View(visitation);
        }

        // GET: Visitations/Create
        public ActionResult Create()
        {
            var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
            if (User.IsInRole("admin"))
            {
                ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
            }
            if (User.IsInRole("manager"))
            {
                ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(currentUser.Id), "EmployeeId", "EFullName");
            }
            return View();
        }

        // POST: Visitations/Create
        [HttpPost]
        public async Task<ActionResult> Create(Visitation visitation)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    visitation.Id = Guid.NewGuid();
                    unitOfWork.Visitations.Create(visitation);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
                if (User.IsInRole("admin"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
                }
                if (User.IsInRole("manager"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(currentUser.Id), "EmployeeId", "EFullName");
                }
                return View(visitation);
            }
            catch
            {
                var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
                if (User.IsInRole("admin"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
                }
                if (User.IsInRole("manager"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(currentUser.Id), "EmployeeId", "EFullName");
                }
                ViewBag.Message = "Что-то пошло не так, обратитесь к администратору.";
                return View(visitation);
            }
        }

        // GET: Visitations/Edit/5
        public async Task<ActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitation visitation = await unitOfWork.Visitations.GetAsync(id);
            if (visitation == null)
            {
                return HttpNotFound();
            }
            var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
            if (User.IsInRole("admin"))
            {
                ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
            }
            if (User.IsInRole("manager"))
            {
                ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(currentUser.Id), "EmployeeId", "EFullName");
            }
            return View(visitation);
        }

        // POST: Visitations/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Guid id, Visitation collection)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Visitations.Update(collection);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
                if (User.IsInRole("admin"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
                }
                if (User.IsInRole("manager"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(currentUser.Id), "EmployeeId", "EFullName");
                }
                return View(collection);
            }
            catch
            {
                var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
                if (User.IsInRole("admin"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
                }
                if (User.IsInRole("manager"))
                {
                    ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(currentUser.Id), "EmployeeId", "EFullName");
                }
                ViewBag.Message = "Что-то пошло не так, обратитесь к администратору.";
                return View(collection);
            }
        }

        // GET: Visitations/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitation visitation = await unitOfWork.Visitations.GetAsync(id);
            if (visitation == null)
            {
                return HttpNotFound();
            }
            return View(visitation);
        }

        // POST: Visitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                unitOfWork.Visitations.Delete(id);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        switch (sqlException.Errors[0].Number)
                        {
                            case 547:
                                return RedirectToAction("Index", "Visitations");
                            default:
                                return View();
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }


        }
    }
}
