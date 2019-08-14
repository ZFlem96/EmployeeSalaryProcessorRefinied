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
            Console.WriteLine(e.ToString());
            Console.Write("press enter to exit...");
            Console.ReadLine();
        }
    }
}
