using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            EmployeeProcessor ep = new EmployeeProcessor();
            ep.getProcessedEmployees();
            ep.getTopEarningEmployees();
            ep.getAllStatesWithMedianData();
            Employee e = ep.GetByEmployeeId("1");
            string employeeString = e.Id + ", " + e.FirstName + ", " + e.LastName + ", " + e.Hours + ", " + e.Residence + ", "
                + e.StartDate + ", " + e.Type + ", " + e.Salary + ", " + e.GrossSalary + ", " + e.NetSalary;
            Console.WriteLine(employeeString);
            Console.Write("press enter to exit...");
        }
    }
}
