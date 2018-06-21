﻿using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using PagedList;

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

        public IPagedList<Employee> GetAllIndex(int pageNumber, int pageSize, string search) => db.Employees.Where(x => x.EFullName.Contains(search) || search == null).Include(e => e.Department).Include(e => e.Position).OrderBy(x=>x.EFullName).ToPagedList(pageNumber, pageSize);

        public async Task<Employee> GetAsync(Guid? id) => await db.Employees.FindAsync(id);

        public void Update(Employee item) => db.Entry(item).State = EntityState.Modified;
    }
}