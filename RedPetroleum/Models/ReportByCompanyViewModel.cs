﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models
{
    public class ReportByCompanyViewModel
    {
        public Guid EmployeeId;
        public string EmployeeName;
        public string Department;
        public string Position;
        public DateTime AdoptionDate;
        public double? AverageMark;
    }

    public class ReportByInstructionsDGViewModel
    {
        public string Department;
        public string TaskName;
        public double? AverageMark;
        public string CommentEmployees;
    }
}