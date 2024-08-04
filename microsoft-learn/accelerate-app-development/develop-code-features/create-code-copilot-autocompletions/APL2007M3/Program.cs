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

        // public struct SalesData. Include the following fields: date sold, department name, product ID, quantity sold, unit price
        public struct SalesData
        {
            public DateOnly dateSold;
            public string departmentName;
            public string productID;
            public int quantitySold;
            public double unitPrice;
        }

        /* the GenerateSalesData method returns 1000 SalesData records. It assigns random values to each field of the data structure */
        public SalesData[] GenerateSalesData()
        {
            SalesData[] salesData = new SalesData[1000];
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                salesData[i].dateSold = new DateOnly(2023, random.Next(1, 13), random.Next(1, 29));
                salesData[i].departmentName = "Department" + random.Next(1, 10);
                salesData[i].productID = "Product" + random.Next(1, 100);
                salesData[i].quantitySold = random.Next(1, 100);
                salesData[i].unitPrice = random.Next(1, 1000);
            }
            return salesData;
        }

        public void QuarterlySalesReport(SalesData[] salesData)
        {
            // create a dictionary to store the quarterly sales data
            Dictionary<string, double> quarterlySales = new Dictionary<string, double>();

            // iterate through the sales data and calculate the quarterly sales for each department
            foreach (SalesData data in salesData)
            {
                // calculate the total sales for each department
                double totalSales = data.quantitySold * data.unitPrice;

                // extract the quarter from the date sold
                string quarter = GetQuarter(data.dateSold);

                // add the total sales to the quarterly sales dictionary
                if (quarterlySales.ContainsKey(quarter))
                {
                    quarterlySales[quarter] += totalSales;
                }
                else
                {
                    quarterlySales.Add(quarter, totalSales);
                }
            }

            // print the quarterly sales report
            Console.WriteLine("Quarterly Sales Report");
            Console.WriteLine("----------------------");
            foreach (KeyValuePair<string, double> entry in quarterlySales)
            {
                Console.WriteLine("Quarter: {0}, Total Sales: {1:C}", entry.Key, entry.Value);
            }


        }
        
        public string GetQuarter(DateOnly dateTime)
        {
            if (dateTime.Month >= 1 && dateTime.Month <= 3)
            {
                return "Q1";
            }
            else if (dateTime.Month >= 4 && dateTime.Month <= 6)
            {
                return "Q2";
            }
            else if (dateTime.Month >= 7 && dateTime.Month <= 9)
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