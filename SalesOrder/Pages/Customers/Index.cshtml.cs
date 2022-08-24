using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SalesOrder.Pages.Customers
{
    public class IndexModel : PageModel
    {
        public List<CustomerInfo> customerInfos = new List<CustomerInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=tcp:koretechinterview.database.windows.net,143;Initial Catalog=KORESampleDatabase;Persist Security Info=False;User id=koreinterview;Password=;";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT  LTRIM(RTRIM(REPLACE(ISNULL(c1.LastName,' ') + ', '+ISNULL(c1.FirstName,' '),' ',' '))) as Name, LTRIM(RTRIM(REPLACE(ISNULL(Title,' ')+' '+ISNULL(c1.FirstName,' ') + ', '+ISNULL(MiddleName,' ')+ ' ' +ISNULL(c1.LastName,' '),' ',' '))) as FullName,c1.EmailAddress as  Email, c1.Phone as Phone" +
                                 "FROM Customer c1" +
                                 "WHERE NOT EXISTS(SELECT 1 FROM Customer c2 WHERE c1.EmailAddress = c2.EmailAddress and c2.ModifiedDate > c1.ModifiedDate) AND ((Name!='' and Name is NOT NULL) OR (c1.EmailAddress!='' and c1.EmailAddress is NOT NULL) OR (c1.Phone!='' and c1.Phone is NOT NULL))" +
                                 "ORDER BY c1.LastName, c1.FirstName ASC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CustomerInfo customerInfo = new CustomerInfo();
                                customerInfo.Name = reader.GetString(0);
                                customerInfo.FullName = reader.GetString(1);
                                customerInfo.EmailAddress = reader.GetString(2);
                                customerInfo.Phone = reader.GetString(3);

                                customerInfos.Add(customerInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString());
            }
        }

        public class CustomerInfo
        {
            public String Name;
            public String FullName;
            public String EmailAddress;
            public String Phone;

        }
    }
}
