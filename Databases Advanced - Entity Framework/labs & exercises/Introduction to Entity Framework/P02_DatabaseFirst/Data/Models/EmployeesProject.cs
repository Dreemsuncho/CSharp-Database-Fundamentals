namespace P02_DatabaseFirst.Data.Models
{
    public partial class EmployeesProject
    {
        public int EmployeeId { get; set; }
        public int ProjectId { get; set; }

        public Employee Employee { get; set; }
        public Project Project { get; set; }
    }
}
