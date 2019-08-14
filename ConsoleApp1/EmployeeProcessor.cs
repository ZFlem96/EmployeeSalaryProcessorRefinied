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
        private string[] stateTaxCheck1 = { "UT", "WY", "NV" };
        private string[] stateTaxCheck2 = { "CO", "ID", "AZ", "OR" };
        private string[] stateTaxCheck3 = { "WA", "NM", "TX" };
        private string fileDir = "./Files/";
        private static double federalTax = .15;
        

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
                DateTime startDate = DateTime.Parse(employeeLine[5]);
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
                netSalary = processSalary(grossSalary, residence);
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
            taxCut = formatValue(taxCut);
            processedSalary = salary - taxCut;
            processedSalary = formatValue(processedSalary);
            return processedSalary;
        }

        private double getStateTax(string residence)
        {
            double stateTax = 0;
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
                double salaryRegularOT = salary * regularOvertimeRate;
                salaryRegularOT = formatValue(salaryRegularOT);
                double salaryExtendedOT = salary * extendedOvertimeRate;
                salaryExtendedOT = formatValue(salaryExtendedOT);
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
                processedRegurlar = formatValue(processedRegurlar);
                double processedRegularOvertime = (salaryRegularOT * timeAndHalfHours);
                processedRegularOvertime = formatValue(processedRegularOvertime);
                double processExtededOvertime = (salaryExtendedOT * extendedTime);
                processExtededOvertime = formatValue(processExtededOvertime);
                double processedOvertime = processedRegularOvertime + processExtededOvertime;
                processedOvertime = formatValue(processedOvertime);
                processedSalary = processedRegurlar + processedOvertime;
            }
            processedSalary = formatValue(processedSalary);
            return processedSalary;
        }

        private double formatValue(double processedRegurlar)
        {
            string format = ""+processedRegurlar;
            processedRegurlar = double.Parse(format);
            string[] formatArray = format.Split('.');
            if (formatArray.Length==2)
            {
                int length = formatArray[1].Length;
                bool MoreThan2Decimals = length > 2;
                if (MoreThan2Decimals)
                {
                    processedRegurlar = double.Parse(string.Format("{0:0.00}", processedRegurlar));
                }
            }
            return processedRegurlar;
        }
        //make sure its by highest gross
        //output: employee id, first name, last name, gross pay, federal tax, state tax, net pay
        public void getProcessedEmployees() {
            if (employees == null)
            {
                ProcessEmployees();
            }
            string fileName = "ProcessedEmployees.txt";
            string file = fileDir + fileName;
            List<Employee> employeesListedByGross = employees.OrderByDescending(x => x.GrossSalary).ToList();
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

        //output: first name, last name, number of years worked, gross pay
        public void getTopEarningEmployees() {
            if (employees == null)
            {
                ProcessEmployees();
            }
            string fileName = "TopEarningEmployees.txt";
            string file = fileDir + fileName;
            List<Employee> employeesListedByGross = employees.OrderByDescending(x => x.GrossSalary).ToList();
            List<Employee> topEmployees = new List<Employee>();
            List<string> lines = new List<string>();
            string line = "";
            double topLimit = .15;
            double top =   employeesListedByGross.Count*topLimit;
            for (int x = 0; x < top;x++)
            {
                Employee e = employeesListedByGross[x];
                topEmployees.Add(e);
            }
            List<Employee> topEmployeesAlphabetically = topEmployees.OrderByDescending(x => x.GrossSalary).OrderByDescending(x => x.StartDate).OrderBy(x => x.LastName).OrderBy(x => x.FirstName).ToList();
            int currentYear = DateTime.Now.Year;
            string currentYearTxt = ""+currentYear;
            int currentYearLast2Digits = int.Parse("" +currentYearTxt[currentYearTxt.Length-2] + currentYearTxt[currentYearTxt.Length - 1]);
            int startYear = 0;
            int totalYears = 0;
            for (int x = 0; x < topEmployeesAlphabetically.Count;x++)
            {
                Employee e = topEmployeesAlphabetically[x];
                startYear = e.StartDate.Year;
                totalYears = currentYear - startYear;
                line = e.FirstName + ", " + e.LastName + ", " + totalYears + " year(s), " + e.GrossSalary;
                lines.Add(line);
            }
            File.WriteAllLines(file, lines);
        }

        //median time worked, median net pay, and total state taxes 
        // output should be state, median time worked, median net pay, state taxes
        public void getAllStatesWithMedianData() {
            if (employees == null)
            {
                ProcessEmployees();
            }
            string fileName = "AllStatesByMedian.txt";
            string file = fileDir + fileName;
            List<string> lines = new List<string>();
            string line = "";
            string state = "";
            int medianHours = 0;
            double medianNetPay = 0;
            double totalStateTaxes = getTotalStateTaxes();
            string totalTaxesFromStates = "";
            List<String> states = getAllStates();
            List<Employee> employeesFromState = new List<Employee>();
            for (int x = 0; x < states.Count;x++)
            {
                state = states[x];
                employeesFromState = getAllEmployeesFromState(state);
                medianHours = getMedianHourByState(employeesFromState);
                medianNetPay = getMedianNetPayByState(employeesFromState);
                totalTaxesFromStates = getTotalStateTaxesByState(totalStateTaxes,state,employeesFromState);
                line = state + ", " + medianHours + ", " + medianNetPay + ", " + totalTaxesFromStates;
                lines.Add(line);
            }
            File.WriteAllLines(file, lines);
        }

        private double getTotalStateTaxes()
        {
            double totalStateTax = 0;
            for (int x = 0; x < employees.Count; x++)
            {
                Employee tmp = employees[x];
                totalStateTax += getStateTax(tmp.Residence);
            }
            return totalStateTax;
        }

        private List<Employee> getAllEmployeesFromState(string state)
        {
            List<Employee> employeesFromState = new List<Employee>();
            Employee e = new Employee();
            for (int x = 0; x < employees.Count;x++)
            {
                e = employees[x];
                if (e.Residence.Equals(state))
                {
                    employeesFromState.Add(e);
                }
            }
            return employeesFromState;
        }

        private List<string> getAllStates()
        {
            List<string> states = new List<string>();
            for (int x = 0; x < stateTaxCheck1.Length;x++)
            {
                states.Add(stateTaxCheck1[x]);
            }
            for (int x = 0; x < stateTaxCheck2.Length; x++)
            {
                states.Add(stateTaxCheck2[x]);
            }
            for (int x = 0; x < stateTaxCheck3.Length; x++)
            {
                states.Add(stateTaxCheck3[x]);
            }
            states.Sort();
            return states;
        }

        private string getTotalStateTaxesByState(double total,string state, List<Employee> employeesFromState)
        {
            double result = 0;
            double stateTax = getStateTax(state);
            double totalFromState = stateTax*employeesFromState.Count;
            result = totalFromState / total;
            result = formatValue(result);
            string percentageOfTSTString = result.ToString("#0.##%");
            return percentageOfTSTString;
        }

        private double getMedianNetPayByState(List<Employee> original)
        {
            double medianNetPay = 0;
            List<Employee> sortedByNetPay = original.OrderBy(x => x.NetSalary).ToList();
            List<double> allNetPay = new List<double>();
            for (int x = 0; x < sortedByNetPay.Count; x++)
            {
                Employee tmp = sortedByNetPay[x];
                allNetPay.Add(tmp.NetSalary);
            }
            int median = allNetPay.Count / 2;
            medianNetPay = allNetPay[median];
            return medianNetPay;
        }

        private int getMedianHourByState(List<Employee> original)
        {
            int medianHour = 0;
            List<Employee> sortedByHours = original.OrderBy(x => x.Hours).ToList();
            List<int> allHours = new List<int>();
            for (int x = 0; x < sortedByHours.Count;x++)
            {
                Employee tmp = sortedByHours[x];
                allHours.Add(tmp.Hours);
            }
            int median = allHours.Count / 2;
            medianHour = allHours[median];
            return medianHour;
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
