﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Employee
    {
       
      

        private string id;
        private string firstName;
        private string lastName;
        private char type;
        private double salary;
        private double grossSalary;
        private double netSalary;
        private string startDate;
        private string residence;
        private int hours;

        public string Id { get => id; set => id = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public char Type { get => type; set => type = value; }
        public double Salary { get => salary; set => salary = value; }
        public double GrossSalary { get => grossSalary; set => grossSalary = value; }
        public double NetSalary { get => netSalary; set => netSalary = value; }
        public string StartDate { get => startDate; set => startDate = value; }
        public string Residence { get => residence; set => residence = value; }
        public int Hours { get => hours; set => hours = value; }

        public Employee() { }
        

    }
}
