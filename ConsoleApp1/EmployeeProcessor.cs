using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleApp1
{
    class EmployeeProcessor
    {
        private List<Employee> employees;
        private string fileDir = "../Files/Employees.txt";
        private static int federalTax = 15;
        internal List<Employee> Employees { get => employees; set => employees = value; }
        private List<double> grossSalaryList;

        public Employee GetEmployeeById(char employeeId)
        {
            Employee employee = new Employee();
            for (int x = 0; x < employees.Count; x++)
            {
                Employee tmp = employees[x];
                if (tmp.Id==employeeId)
                {
                    employee = tmp;
                    break;
                }
            }
            return employee;
        }


        public void ProcessEmployees()
        {
            StreamReader sr = new StreamReader(fileDir);
            string line = "";
            employees = new List<Employee>();
            while ((line = sr.ReadLine()) != null)
            {
                string[] employeeLine = line.Split(',');
                Employee employee = new Employee();
                char id = employeeLine[0].ToCharArray()[0];
                string firstName = employeeLine[1];
                string lastName = employeeLine[2];
                char type = employeeLine[3].ToCharArray()[0];
                double salary = double.Parse(employeeLine[4]);
                string startDate = employeeLine[5];
                string residence = employeeLine[6];
                int hours = int.Parse(employeeLine[7]);
                employee.Id = id;
                employee.FirstName = firstName;
                employee.LastName = lastName;
                employee.Type = type;
                employee.Salary = salary;
                employee.StartDate = startDate;
                employee.Residence = residence;
                employee.Hours = hours;
                if (type == 'H')
                {
                    salary = processHourly(salary, hours);
                }
                grossSalaryList.Add(salary);
                salary = processSalary(salary, residence);
                employee.Salary = salary;
                employees.Add(employee);
            }
        }

        private double processSalary(double salary, string residence )
        {
            double processedSalary = 0;
            double totalTax = federalTax;
            double stateTax = getStateTax(residence);
            totalTax += stateTax;
            double taxCut = salary * totalTax;
            processedSalary = salary - taxCut;
            return processedSalary;
        }

        private double getStateTax(string residence)
        {
            double stateTax = 0;
            string[] stateTaxCheck1 = { "UT","WY","NV" };
            string[] stateTaxCheck2 = { "CO", "ID", "AZ", "OR " };
            string[] stateTaxCheck3 = { "WA", "NM", "TX" };
            for (int x = 0;x<stateTaxCheck1.Length;x++)
            {
                if (stateTaxCheck1[x].Equals(residence))
                {
                    stateTax = .05;
                }
            }
            if (stateTax==0)
            {
                for (int x = 0; x < stateTaxCheck2.Length; x++)
                {
                    if (stateTaxCheck2[x].Equals(residence))
                    {
                        stateTax = .065;
                    }
                }
            }
            if (stateTax==0)
            {
                for (int x = 0; x < stateTaxCheck3.Length; x++)
                {
                    if (stateTaxCheck3[x].Equals(residence))
                    {
                        stateTax = .07;
                    }
                }
            }
            
            return stateTax;
        }

        private double processHourly(double salary, int hours)
        {
            int regularHours = 80;
            double processedSalary = 0;
            double regularOvertimeRate = 1.5, extendedOvertimeRate = 1.75;
            if (hours <= regularHours)
            {
                processedSalary = (salary * hours); 
            } else
            {
                int totalOvertime = hours - regularHours;
                int timeAndHalfHours = 0, extendedTime = 0;
                double salaryRegularOT = salary+(salary*regularOvertimeRate);
                double salaryExtendedOT = salary + (salary * extendedOvertimeRate);
                if (totalOvertime > 10)
                {
                    timeAndHalfHours = 10;
                    extendedTime = totalOvertime - timeAndHalfHours;
                } else
                {
                    timeAndHalfHours = hours - regularHours;
                    
                }
                int remainder = hours - totalOvertime;
                double processedRegurlar = (salary * remainder);
                double processedRegularOvertime = (salaryRegularOT * timeAndHalfHours);
                double processExtededOvertime = (salaryExtendedOT * extendedTime);
                double processedOvertime = processedRegularOvertime + processExtededOvertime;
                processedSalary = processedRegurlar + processedOvertime;
            }
            return processedSalary;
        }

        public void getProcessedEmployees() { }

        public void getTopEarningEmployees() { }

        public void getAllStatesByMedian() { }
    }

    
}
