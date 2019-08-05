using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApp1
{
    class EmployeeProcessor
    {
        private List<Employee> employees;
        private string fileDir = "../Files/";
        private static double federalTax = .15;
        private List<Employee> Employees { get => employees; set => employees = value; }


        private void ProcessEmployees()
        {
            string fileName = "Employees.txt";
            string file = fileDir + fileName;
            StreamReader sr = new StreamReader(file);
            string line = "";
            employees = new List<Employee>();
            while ((line = sr.ReadLine()) != null)
            {
                string[] employeeLine = line.Split(',');
                Employee employee = new Employee();
                string id = employeeLine[0];
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
                double grossSalary = 0, netSalary = 0;
                if (type == 'H')
                {
                    grossSalary = processHourly(salary, hours);
                } else
                {
                    grossSalary = salary;
                }
                employee.GrossSalary = grossSalary;
                netSalary = processSalary(salary, residence);
                employee.NetSalary = netSalary;
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
        //output: employee id, first name, last name, gross pay, federal tax, state tax, net pay
        public void getProcessedEmployees() {
            if (employees.Count == 0)
            {
                ProcessEmployees();
            }
            string fileName = "ProcessedEmployees.txt";
            string file = fileDir + fileName;
            List<Employee> employeesListedByGross = getEmployeeListByGross();
            List<string> lines = new List<string>();
            string line = "";
            string fedTax = "15%";
            string stateTaxTxt = "";
            string gsTxt = "";
            string nsTxt = "";
            double stateTax = 0;
            for (int x = 0;x< employeesListedByGross.Count;x++)
            {
                Employee e = employeesListedByGross[x];
                stateTax = getStateTax(e.Residence);
                //.05||.065||.07
                if (stateTax == .05)
                {
                    stateTaxTxt = "5%";
                } else if (stateTax == .065)
                {
                    stateTaxTxt = "6.5%";
                } else if (stateTax == .07)
                {
                    stateTaxTxt = "7%";
                }
                gsTxt = string.Format("{0:0.00}", e.GrossSalary);
                nsTxt = string.Format("{0:0.00}", e.NetSalary);
                line = e.Id+", "+e.FirstName+", " +e.LastName+", "+ gsTxt + ", " +fedTax + ", " +stateTaxTxt + ", " + nsTxt;
                lines.Add(line);
            }
            File.WriteAllLines(file, lines);
        }

        private List<Employee> getEmployeeListByGross()
        {
            List<Employee> tmpList = employees;
            List<Employee> employeesListedByGross = new List<Employee>();
            double largestGross = 0;
            int employeeIndex = 0;
            while (tmpList.Count>0)
            {
                largestGross = tmpList[0].GrossSalary;
                employeeIndex = 0;
                for (int x = 0; x < tmpList.Count;x++)
                {
                    Employee tmp = tmpList[x];
                    if (largestGross < tmp.GrossSalary)
                    {
                        largestGross = tmp.GrossSalary;
                        employeeIndex = x;
                    }
                }
                employeesListedByGross.Add(tmpList[employeeIndex]);
                tmpList.RemoveAt(employeeIndex);
            }
            return employeesListedByGross;
        }
        //output: first name, last name, number of years worked, gross pay
        public void getTopEarningEmployees() {
            if (employees.Count == 0)
            {
                ProcessEmployees();
            }
            string fileName = "TopEarningEmployees.txt";
            string file = fileDir + fileName;
            List<Employee> employeesListedByGross = getEmployeeListByGross();
            List<Employee> topEmployees = new List<Employee>();
            List<string> lines = new List<string>();
            string line = "";
            int topLimit = 15;
            int top = employeesListedByGross.Count / topLimit;
            for (int x = 0; x < top;x++)
            {
                Employee e = employeesListedByGross[x];
                topEmployees.Add(e);
            }
            List<Employee> topEmployeesAlphabetically = getTopEmployeesAlphabetically(topEmployees);
            int currentYear = DateTime.Now.Year;
            string currentYearTxt = ""+currentYear;
            int currentYearLast2Digits = int.Parse("" +currentYearTxt[currentYearTxt.Length-2] + currentYearTxt[currentYearTxt.Length - 1]);
            int startYear = 0;
            int startYear2Digits = 0;
            int totalYears = 0;
            string startYearTxt = "";
            for (int x = 0; x < topEmployeesAlphabetically.Count;x++)
            {
                Employee e = topEmployeesAlphabetically[x];
                startYearTxt = ""+e.StartDate[e.StartDate.Length - 2] + e.StartDate[e.StartDate.Length - 1];
                startYear2Digits = int.Parse(startYearTxt);
                if (startYear2Digits < currentYearLast2Digits)
                {
                    startYearTxt = 20 + startYearTxt;
                } else if (startYear2Digits > currentYearLast2Digits)
                {
                    startYearTxt = 19 + startYearTxt;
                }
                startYear = int.Parse(startYearTxt);
                totalYears = currentYear - startYear;
                line = e.FirstName + ", " + e.LastName + ", " + totalYears + " year(s), " + e.GrossSalary;
                lines.Add(line);
            }
            File.WriteAllLines(file, lines);
        }

        private List<Employee> getTopEmployeesAlphabetically(List<Employee> topEmployees)
        {
            List<Employee> topEmployeesAlphabetically = new List<Employee>();
            List<Employee> topEmployeesLast = topEmployees.OrderBy(x => x.LastName).ToList();
            topEmployeesAlphabetically = topEmployees.OrderBy(x => x.FirstName).ToList();
            return topEmployeesAlphabetically;
        }

        public void getAllStatesByMedian() {
            if (employees.Count == 0)
            {
                ProcessEmployees();
            }
            string fileName = "AllStatesByMedian.txt";
            string file = fileDir + fileName;
            List<string> lines = new List<string>();
            string line = "";
            File.WriteAllLines(file, lines);
        }

        public Employee GetByEmployeeId(string employeeId)
        {
            Employee employee = new Employee();
            for (int x = 0;x<employees.Count;x++)
            {
                Employee tmp = employees[x];
                if (tmp.Id.Equals(employeeId))
                {
                    employee = tmp;
                }
            }
            return employee;
        }
    }

    
}
