using System;
using System.Data.SqlClient;

public class NewEntryClass
{
    private readonly string _connectionString;

    public NewEntryClass(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertNewEntry(string name, string email, int healthAge)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO HealthSpanMD_ClientList (Name, Email, HealthAge) 
                                        VALUES (@Name, @Email, @HealthAge)";

                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@HealthAge", healthAge);

                command.ExecuteNonQuery();
            }
        }
    }
}