using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportGenerator
{
    class QuarterlyIncomeReport
    {
        static void Main(string[] args)
        {
            // create a new instance of the class
            QuarterlyIncomeReport report = new QuarterlyIncomeReport();

            // call the GenerateSalesData method
            SalesData[] salesData = report.GenerateSalesData();
            
            // call the QuarterlySalesReport method
            report.QuarterlySalesReport(salesData);
        }

        /* public struct SalesData includes the following fields: date sold, department name, product ID, quantity sold, unit price */
        public struct SalesData
        {
            public DateOnly dateSold;
            public string departmentName;
            public string productID;
            public int quantitySold;
            public double unitPrice;
            public double baseCost;
            public int volumeDiscount;
        }

        /// <summary>
        /// Represents the product departments.
        /// </summary>
        public struct ProdDepartments
        {
            public static string[] DepartmentNames = { "Menswear", "Womenswear", "Childrenswear", "Footwear", "Accessories", "Sportswear", "Outerwear", "Intimates" };
            public static string[] DepartmentAbbreviations = { "MENS", "WMNS", "CHLD", "FOOT", "ACCS", "SPORT", "OUTR", "INTM" };
        }

        public struct ManufacturingSites
        {
            public static string[] manSites = { "US1", "US2", "UK1", "UK2", "FR1", "FR2", "DE1", "DE2", "JP1", "JP2" };
        }

        /* the GenerateSalesData method returns 1000 SalesData records. It assigns random values to each field of the data structure */
        public SalesData[] GenerateSalesData()
        {
            SalesData[] salesData = new SalesData[1000];
            Random random = new Random();

            for (int i = 0; i < 1000; i++)
            {
                salesData[i].dateSold = new DateOnly(2023, random.Next(1, 13), random.Next(1, 29));
                salesData[i].departmentName = ProdDepartments.DepartmentNames[random.Next(0, ProdDepartments.DepartmentNames.Length)];

                int indexOfDept = Array.IndexOf(ProdDepartments.DepartmentNames, salesData[i].departmentName);
                string deptAbb = ProdDepartments.DepartmentAbbreviations[indexOfDept];
                string firstDigit = (indexOfDept + 1).ToString();
                string nextTwoDigits = random.Next(1, 100).ToString("D2");
                string sizeCode = "";
                sizeCode = new string[] { "XS", "S", "M", "L", "XL" }[random.Next(0, 5)];
                string colorCode = new string[] { "BK", "BL", "GR", "RD", "YL", "OR", "WT", "GY" }[random.Next(0, 8)];
                string manufacturingSite = ManufacturingSites.manSites[random.Next(0, ManufacturingSites.manSites.Length)];

                salesData[i].productID = $"{deptAbb}-{firstDigit}{nextTwoDigits}-{sizeCode}-{colorCode}-{manufacturingSite}";
                salesData[i].quantitySold = random.Next(1, 101);
                salesData[i].unitPrice = random.Next(25, 300) + random.NextDouble();
                salesData[i].baseCost = salesData[i].unitPrice * (1 - (random.Next(5, 21) / 100.0));
                salesData[i].volumeDiscount = (int)(salesData[i].quantitySold * 0.1);
            }

            return salesData;
        }

        public void QuarterlySalesReport(SalesData[] salesData)
        {
            // create a dictionary to store the quarterly sales data by department
            Dictionary<string, Dictionary<string, double>> quarterlySalesByDept = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, Dictionary<string, double>> quarterlyProfitByDept = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, Dictionary<string, double>> quarterlyProfitPercentageByDept = new Dictionary<string, Dictionary<string, double>>();

            // iterate through the sales data
            foreach (SalesData data in salesData)
            {
            // calculate the total sales for each quarter and department
            string quarter = GetQuarter(data.dateSold.Month);
            string department = data.departmentName;
            double totalSales = data.quantitySold * data.unitPrice;
            double totalCost = data.quantitySold * data.baseCost;
            double profit = totalSales - totalCost;
            double profitPercentage = (profit / totalSales) * 100;

            if (!quarterlySalesByDept.ContainsKey(quarter))
            {
                quarterlySalesByDept.Add(quarter, new Dictionary<string, double>());
                quarterlyProfitByDept.Add(quarter, new Dictionary<string, double>());
                quarterlyProfitPercentageByDept.Add(quarter, new Dictionary<string, double>());
            }

            if (quarterlySalesByDept[quarter].ContainsKey(department))
            {
                quarterlySalesByDept[quarter][department] += totalSales;
                quarterlyProfitByDept[quarter][department] += profit;
            }
            else
            {
                quarterlySalesByDept[quarter].Add(department, totalSales);
                quarterlyProfitByDept[quarter].Add(department, profit);
            }

            if (!quarterlyProfitPercentageByDept[quarter].ContainsKey(department))
            {
                quarterlyProfitPercentageByDept[quarter].Add(department, profitPercentage);
            }
            }

            // display the quarterly sales report by department
            Console.WriteLine("Quarterly Sales Report by Department");
            Console.WriteLine("------------------------------------");

            // sort the quarterly sales data by quarter
            var sortedQuarterlySalesByDept = quarterlySalesByDept.OrderBy(q => q.Key);

            // display the quarterly sales results by department in order
            foreach (KeyValuePair<string, Dictionary<string, double>> quarter in sortedQuarterlySalesByDept)
            {
            Console.WriteLine("{0}:", quarter.Key);

            var sortedSalesByDept = quarter.Value.OrderBy(d => d.Key);

            foreach (KeyValuePair<string, double> deptSales in sortedSalesByDept)
            {
                string department = deptSales.Key;
                double sales = deptSales.Value;
                double profit = quarterlyProfitByDept[quarter.Key][department];
                double profitPercentage = quarterlyProfitPercentageByDept[quarter.Key][department];

                Console.WriteLine("Department: {0}, Sales: {1}, Profit: {2}, Profit Percentage: {3}%", department, sales.ToString("C"), profit.ToString("C"), profitPercentage.ToString("F2"));
            }

            Console.WriteLine();
            }
        }

        public string GetQuarter(int month)
        {
            if (month >= 1 && month <= 3)
            {
                return "Q1";
            }
            else if (month >= 4 && month <= 6)
            {
                return "Q2";
            }
            else if (month >= 7 && month <= 9)
            {
                return "Q3";
            }
            else
            {
                return "Q4";
            }
        }
    }
}
