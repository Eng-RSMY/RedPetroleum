using RedPetroleum.Models.Entities;
using RedPetroleum.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using X.PagedList;
using System.Data.Entity;

namespace RedPetroleum.Models.Repositories
{
    public class VisitationRepository : IRepository<Visitation>
    {
        private ApplicationDbContext db;

        public VisitationRepository(ApplicationDbContext context) => db = context;

        public void Create(Visitation item) => db.Visitations.Add(item);

        public void Delete(Guid id)
        {
            Visitation visitation = db.Visitations.Find(id);
            if (visitation != null)
                db.Visitations.Remove(visitation);
        }

        public Visitation Get(Guid id) => db.Visitations.Find(id);

        public IEnumerable<Visitation> GetAll() => db.Visitations.Include(x => x.Employee).Include(x => x.Employee.Department).OrderBy(x => x.Employee.EFullName);

        public IPagedList<Visitation> GetAllIndex(int pageNumber, int pageSize, string search)
        {
            return db.Visitations.Include(x=>x.Employee).Include(x=>x.Employee.Department).Where(x=>x.Employee.EFullName.Contains(search) || search == null).OrderBy(z => z.Employee.EFullName).ToPagedList(pageNumber, pageSize);
        }

        public async Task<Visitation> GetAsync(Guid? id) => await db.Visitations.FindAsync(id);

        public void Update(Visitation item)
        {
            db.Entry(item).State = EntityState.Modified;
        } 
        public string GetDepartmentName (Visitation visitation)
        {
            var visit = db.Visitations.Include(e => e.Employee.Department).Where(x => x.Id == visitation.Id).SingleOrDefault();
            var department = db.Departments.Where(x => x.DepartmentId == visit.Employee.Department.DepartmentId).SingleOrDefault();
            return department == null ? "Нет" : department.Name;
        }
        public string GetEmployeeName (Visitation visitation)
        {
            var visit = db.Visitations.Include(e => e.Employee).Where(x => x.Id == visitation.Id).SingleOrDefault();
            var employee = db.Employees.Where(x => x.EFullName == visit.Employee.EFullName).SingleOrDefault();
            return employee == null ? "Нет" : employee.EFullName;
        }
    }
}