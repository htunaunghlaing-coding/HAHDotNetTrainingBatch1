using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HAHDotNetTrainingBatch1.MiniPerfumeManagementSystemWithAdo;

public class PerfumeService
{
    public void Read()
    {
        string connectionStr = "Data Source=.;Initial Catalog=HAHDotNetTrainingBatch1;User Id=sa;Password=sa@123;";
        SqlConnection connection = new SqlConnection(connectionStr);
        connection.Open();

        string query = @"SELECT [id]
      ,[name]
      ,[company]
      ,[ml]
      ,[price]
      ,[delFlag]
  FROM [dbo].[tbl_perfume] Where DelFlag = 0";

        SqlCommand cmd = new SqlCommand(query, connection);
        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adapter.Fill(dt);

        connection.Close();

        foreach (DataRow dr in dt.Rows)
        {
            Console.WriteLine(dr["id"]);
            Console.WriteLine(dr["name"]);
            Console.WriteLine(dr["company"]);
            Console.WriteLine(dr["ml"]);
            Console.WriteLine(dr["price"]);
        }
    }

    public void create()
    {
        Console.WriteLine("Perfume Name : ");
        var perfumeName = Console.ReadLine();

        Console.WriteLine("Perfume's Company : ");
        var company = Console.ReadLine();

        Console.WriteLine("Perfume's Ml size : ");
        var mlSize = Console.ReadLine();

        Console.WriteLine("Perfume's Price : ");
        var price = Console.ReadLine();

        if (string.IsNullOrEmpty(perfumeName) ||
           string.IsNullOrEmpty(company) ||
           string.IsNullOrEmpty(mlSize) ||
           string.IsNullOrEmpty(price))
        {
            Console.WriteLine("All fields are required. Pls Filling.");
            return;
        }

        var connectionStr = "Data Source=.;Initial Catalog=HAHDotNetTrainingBatch1;User Id=sa;Password=sa@123;";
        SqlConnection connection = new SqlConnection(connectionStr);
        connection.Open();

        string validationQuery = $@"SELECT COUNT(1) FROM [dbo].[tbl_perfume]
                               WHERE name = @perfumeName
                                 AND company = @company
                                 AND ml = @ml
                                 AND price = @price
                                 AND delFlag = 0";

        SqlCommand cmd1 = new SqlCommand(validationQuery, connection);
        cmd1.Parameters.AddWithValue("@perfumeName", perfumeName);
        cmd1.Parameters.AddWithValue("@company", company);
        cmd1.Parameters.AddWithValue("@ml", mlSize);
        cmd1.Parameters.AddWithValue("@price", price);

        int count1 = (int)cmd1.ExecuteScalar();

        if (count1 > 0)
        {
            Console.WriteLine("This perfume is already exists.Pls Filling Other Perfume.Thanks!");
            return;
        }

        string insertQuery = $@"INSERT INTO [dbo].[tbl_perfume]
           ([name]
           ,[company]
           ,[ml]
           ,[price]
           ,[delFlag])
     VALUES
           (@perfumeName
           ,@company
           ,@mlSize
           ,@price
           ,0)";

        SqlCommand cmd2 = new SqlCommand(insertQuery, connection);
        cmd2.Parameters.AddWithValue("@perfumeName", perfumeName);
        cmd2.Parameters.AddWithValue("@company", company);
        cmd2.Parameters.AddWithValue("@mlSize", mlSize);
        cmd2.Parameters.AddWithValue("@price", price);

        int result = cmd2.ExecuteNonQuery();

        Console.WriteLine(result == 1 ? "Create Perfume Succeessfully." : "Create Perfume Fail.");

        connection.Close();
    }
}
