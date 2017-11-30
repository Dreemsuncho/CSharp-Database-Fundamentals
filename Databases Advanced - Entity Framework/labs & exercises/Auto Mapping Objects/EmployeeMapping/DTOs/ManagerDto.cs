using System.Collections.Generic;

namespace EmployeeMapping.DTOs
{
    public class ManagerDto
    {
        public string FullName  { get; set; }
        public IEnumerable<EmployeeDto> Employees { get; set; }
    }
}
