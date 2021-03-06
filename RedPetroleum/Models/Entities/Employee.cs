﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Entities
{
    public class Employee
    {
        public Guid EmployeeId{ get; set; }

        [Display(Name = "ФИО Сотрудника")]
        [StringLength(100, ErrorMessage = "Длина строки не должна превышать 100 символов")]
        public string EFullName { get; set; }
        
        public Guid DepartmentId { get; set; }
        
        public Guid PositionId { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateBorn { get; set; }

        [Display(Name = "Дата принятия")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime AdoptionDate { get; set; }

        [Display(Name = "Уволен")]
        public bool Dismissed { get; set; }
        public ICollection<TaskList> TaskLists { get; set; }
        public Department Department { get; set; }
        public Position Position { get; set; }
    }
}