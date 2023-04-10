using System;
using System.Data.SqlClient;

public class NewEntryClass
{
    private readonly string _connectionString;

    public NewEntryClass(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertNewEntry(string name, string email, int healthAge, DateTime submissionDate)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"INSERT INTO TableName (Name, Email, HealthAge, SubmissionDate) 
                                        VALUES (@Name, @Email, @HealthAge, @SubmissionDate)";

                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@HealthAge", healthAge);
                command.Parameters.AddWithValue("@SubmissionDate", submissionDate);

                command.ExecuteNonQuery();
            }
        }
    }
}