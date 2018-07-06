﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using RedPetroleum.Models;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using RedPetroleum.Services;

namespace RedPetroleum.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        UnitOfWork unit;
        public ReportsController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        public ReportsController()
        {
            this.unit = new UnitOfWork();
        }

        public ActionResult ReportByDepartment(string departmentId, DateTime? dateValue)
        {
            IEnumerable<Employee> emplist = unit.Employees.GetEmployeesWithRelations();
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            if (User.IsInRole("admin"))
            {
                ViewBag.Departments = new SelectList(
                         unit.Departments.GetAll(),
                         "DepartmentId",
                         "Name"
                     );
                if (departmentId != null)
                {
                    var employeeList = new List<ReportByCompanyViewModel>();

                    ReportByCompanyViewModel model;
                    foreach (Employee employee in emplist)
                    {
                        model = new ReportByCompanyViewModel
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeeName = employee.EFullName,
                            Position = employee.Position.Name,
                            AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                        };

                        employeeList.Add(model);
                    }
                    return View(employeeList);
                }
                return View();
            }
            else
            {
                ViewBag.Departments = new SelectList(
                             unit.Departments.GetAvailableDepartments(User.Identity.GetUserId()),
                             "DepartmentId",
                             "Name"
                         );
                if (departmentId != null)
                {
                    var employeeList = new List<ReportByCompanyViewModel>();

                    ReportByCompanyViewModel model;
                    foreach (Employee employee in emplist)
                    {
                        model = new ReportByCompanyViewModel
                        {
                            EmployeeId = employee.EmployeeId,
                            EmployeeName = employee.EFullName,
                            Position = employee.Position.Name,
                            AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                        };

                        employeeList.Add(model);
                    }
                    return View(employeeList);
                }
                return View();
            }
            ////ViewBag.DepName = unit.Departments.GetDepartmentNameById(ViewBag.Departments);
            //return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByDepartment(string departmentId, DateTime? dateValue)
        {
            if (departmentId == "")
            {
                return PartialView();
            }
            IEnumerable<Employee> employees = unit.Employees
                .GetEmployeesByDepartmentId(Guid.Parse(departmentId), dateValue);
            var empList = new List<ReportByCompanyViewModel>();
            ReportByCompanyViewModel model;
            foreach (var item in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = item.EmployeeId,
                    EmployeeName = item.EFullName,
                    Position = item.Position.Name,
                    AverageMark = item.TaskLists.Select(t => t.AverageMark).Average()
                };

                empList.Add(model);
            }
            return PartialView(empList);
        }

        public ActionResult ReportByCompany(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return View(employeeList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByCompany(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return PartialView(employeeList);
        }

        public ActionResult ReportByDepartmentAverageMark(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Marks = employeeList;
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return View(employeeList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByDepartmentAverageMark(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Marks = employeeList;
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return PartialView(employeeList);
        }

        public ActionResult ReportByConsolidated(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return View(employeeList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByConsolidated(DateTime? dateValue)
        {
            IEnumerable<Employee> employees = unit.Employees.GetEmployeesByTaskDate(dateValue);

            var employeeList = new List<ReportByCompanyViewModel>();

            ReportByCompanyViewModel model;
            foreach (Employee employee in employees)
            {
                model = new ReportByCompanyViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    EmployeeName = employee.EFullName,
                    Department = employee.Department.Name,
                    Position = employee.Position.Name,
                    AdoptionDate = employee.AdoptionDate,
                    AverageMark = employee.TaskLists.Select(t => t.AverageMark).Average()
                };

                employeeList.Add(model);
            }
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            return PartialView(employeeList);
        }

        public ActionResult ReportByInstructionsDG(string departmentId, DateTime? dateValue)
        {
            IEnumerable<TaskList> taskLists = unit.TaskLists.GetTaskListsWithRelations();
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");
            if (User.IsInRole("admin"))
            {
                ViewBag.Departments = new SelectList(
                         unit.Departments.GetAll(),
                         "DepartmentId",
                         "Name"
                     );
                if (departmentId != null)
                {
                    var taskList = new List<ReportByInstructionsDGViewModel>();

                    ReportByInstructionsDGViewModel model;
                    foreach (TaskList item in taskLists)
                    {
                        model = new ReportByInstructionsDGViewModel
                        {
                            TaskName = item.TaskName,
                            AverageMark = item.AverageMark,
                            CommentEmployees = item.CommentEmployees
                        };

                        taskList.Add(model);
                    }
                    return View(taskList);
                }
                return View();

            }
            else
            {
                ViewBag.Departments = new SelectList(
                             unit.Departments.GetAvailableDepartments(User.Identity.GetUserId()),
                             "DepartmentId",
                             "Name"
                         );
                if (departmentId != null)
                {
                    var taskList = new List<ReportByInstructionsDGViewModel>();

                    ReportByInstructionsDGViewModel model;
                    foreach (TaskList item in taskLists)
                    {
                        model = new ReportByInstructionsDGViewModel
                        {
                            TaskName = item.TaskName,
                            AverageMark = item.AverageMark,
                            CommentEmployees = item.CommentEmployees
                        };

                        taskList.Add(model);
                    }
                    return View(taskList);
                }
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialReportByInstructionsDG(string departmentId, DateTime? dateValue)
        {
            if (departmentId == "")
            {
                return PartialView();
            }
            IEnumerable<TaskList> taskLists = unit.TaskLists
                .GetTaskListsByDepartmentId(Guid.Parse(departmentId), dateValue);
            var taskList = new List<ReportByInstructionsDGViewModel>();
            ReportByInstructionsDGViewModel model;
            foreach (var item in taskLists)
            {
                model = new ReportByInstructionsDGViewModel
                {
                    TaskName = item.TaskName,
                    AverageMark = item.AverageMark,
                    CommentEmployees = item.CommentEmployees
                };

                taskList.Add(model);
            }
            return PartialView(taskList);
        }

        public ActionResult GetEmployeeChart(string departmentId)
        {
            ViewBag.Departments = new SelectList(unit.Departments.GetAll(), "DepartmentId", "Name");
            return View();
        }

        public void ExportToExcel(string departmentId, string reportType, DateTime? dateValue)
        {
            Guid? department;

            if (departmentId == "*")
            {
                department = null;
            }
            else
            {
                department = Guid.Parse(departmentId);
            }
            XlsReport report = new XlsReport(unit, department, reportType, dateValue);
            ExcelPackage package = report.FormReport();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=ExcelReport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }
    }
}