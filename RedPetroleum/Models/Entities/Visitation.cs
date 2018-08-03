using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class Visitation
    {
        public Guid Id { get; set; }
        public Guid? EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime Date { get; set; }
        public double? VisitMark { get; set; }

    }
}