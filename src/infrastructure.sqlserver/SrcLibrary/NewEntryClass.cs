using System;
using System.Data.SqlClient;

public class NewEntryClass
{
    private readonly string _connectionString = "Server=healthspanmd.database.windows.net;Database=HealthspanMDMarketing;user id=marketing; password=h3992-MK.2993pzjj7;";

    struct entry
    {
        public string name;
        public string email;
        public int healthAge;
    }

    public void insertNewEntry(string name, string email, int healthAge)
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

    public entry[] getEntries()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
        List<entry> entries = new List<entry>();
            connection.Open();
             using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        entry e = new entry();
                        e.name = reader.GetString(0);
                        e.email = reader.GetString(1);
                        e.healthAge = reader.GetInt32(2);
                        entries.Add(e);
                    }
                }
            }
            connection.Close();
        

        // Convert the list to an array and return it.
        return entries.ToArray();
        }


}