﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using X.PagedList;
using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;

namespace RedPetroleum.Models.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private ApplicationDbContext db;

        public EmployeeRepository(ApplicationDbContext context) => db = context;

        public void Create(Employee item) => db.Employees.Add(item);

        public void Delete(Guid id)
        {
            Employee emp = db.Employees.Find(id);
            if (emp != null)
                db.Employees.Remove(emp);
        }

        public Employee Get(Guid id) => db.Employees.Find(id);

        public IEnumerable<Employee> GetAll() => db.Employees.Include(e => e.Department).Include(e => e.Position);

        public IEnumerable<Employee> GetAllWithoutRelations()
        {
            return db.Employees;
        }

        public IPagedList<Employee> GetAllIndex(int pageNumber, int pageSize, string search) => db.Employees.Where(x => x.EFullName.Contains(search) || search == null).Include(e => e.Department).Include(e => e.Position).OrderBy(x=>x.EFullName).ToPagedList(pageNumber, pageSize);

        public async Task<Employee> GetAsync(Guid? id) => await db.Employees.FindAsync(id);

        public void Update(Employee item) => db.Entry(item).State = EntityState.Modified;
        public async Task<IEnumerable<Employee>> GetAllAsync() => await db.Employees.Include(e => e.Department).Include(e => e.Position).ToListAsync();

        public IEnumerable<Employee> GetEmployeesWithPositions()
        {
            return db.Employees.Include(p => p.Position).Include(e => e.Department);
        }

        public IEnumerable<Employee> GetEmployeesByDepartmentId(Guid id, DateTime? taskDate)
        {
            return db.Employees
                .Include(p => p.Position)
                .Include(t=>t.TaskLists)
                .Include(d=>d.Department)
                .Where(e => e.DepartmentId == id).Where(e =>
                    ((DateTime)e.TaskLists.FirstOrDefault().TaskDate) == taskDate
                ); 
        }

        public IEnumerable<Employee> GetEmployeesWithRelations()
        {
            return db.Employees
                .Include(e => e.Department)
                .Include(t=> t.TaskLists)
                .Include(p => p.Position);          
        }

        public string GetEmployeeNameById(Guid id)
        {
            return db.Employees
                .SingleOrDefault(e => e.EmployeeId == id)
                .EFullName;

        }

        public IEnumerable<Employee> GetAvailableEmployees(string id)
        {
            var currentUser = db.Users.Find(id);
            return db.Employees
                .Where(e =>
                    e.DepartmentId.ToString() == currentUser.DepartmentId
                );
        }

        public IEnumerable<Employee> GetEmployeesByTaskDate(DateTime? taskDate)
        {
            return taskDate == null 
                ? db.Employees.Include(t => t.TaskLists).Include(p => p.Position).Include(d => d.Department)
                .Where(e =>
                    ((DateTime)e.TaskLists.FirstOrDefault().TaskDate).Month == DateTime.Now.Month &&
                    ((DateTime)e.TaskLists.FirstOrDefault().TaskDate).Year == DateTime.Now.Year
                ) 
                : db.Employees.Include(t => t.TaskLists).Include(p => p.Position).Include(d => d.Department)
                .Where(e =>
                    ((DateTime)e.TaskLists.FirstOrDefault().TaskDate) == taskDate
                );
        }
    }
}