﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using RedPetroleum.Services;

namespace RedPetroleum.Controllers
{
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

        public ActionResult ReportByDepartment()
        {
            IEnumerable<Employee> emplist = unit.Employees.GetEmployeesWithPositions();
            ViewBag.Departments = new SelectList(
                unit.Departments.GetAvailableDepartments(User.Identity.GetUserId()),
                "DepartmentId",
                "Name"
            );

            return View(emplist);
        }

        public void ExportToExcel(string departmentId, string reportType)
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


            XlsReport report = new XlsReport(unit, department, reportType);
            ExcelPackage package = report.FormReport();


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=ExcelReport.xlsx");
            Response.BinaryWrite(package.GetAsByteArray());
            Response.End();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetEmployeesByDepartment(string departmentId)
        {
            
            Guid id = Guid.Parse(departmentId);
            IEnumerable<Employee> employees = unit.Employees
                .GetEmployeesByDepartmentId(id);

            return PartialView(employees);
        }

        public ActionResult ReportByCompany()
        {
            IEnumerable<Employee> emplist = unit.Employees.GetEmployeesWithPositions();
            ViewBag.Departments = new SelectList(
                unit.Departments.GetAll(),
                "DepartmentId",
                "Name"
            );

            return View(emplist);
        }
    }
}