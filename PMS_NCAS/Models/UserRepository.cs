using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Configuration;
using PMS_NCAS.Models;
using System.Web.Mvc;

namespace PMS_NCAS.Models
{
    public class UserRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public bool Register(User user)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (FirstName, LastName, Username, Email, PasswordHash, Role, ContactNumber) " +
                               "VALUES (@FirstName, @LastName, @Username, @Email, @PasswordHash, @Role, @ContactNumber)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FirstName", user.FirstName); 
                cmd.Parameters.AddWithValue("@LastName", user.LastName);   
                cmd.Parameters.AddWithValue("@Username", user.Username);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", user.Password);
                cmd.Parameters.AddWithValue("@Role", user.Role);
                cmd.Parameters.AddWithValue("@ContactNumber", user.ContactNumber);

                con.Open();
                int result = cmd.ExecuteNonQuery();
                return result > 0;
            }
        }

        public bool IsUsernameAvailable(string username)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0; 
            }
        }

        internal bool Register(ViewTemplateUserControl model)
        {
            throw new NotImplementedException();
        }

        public User Login(string username, string password)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM Users WHERE Username = @Username";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);

                con.Open();
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string storedPasswordHash = reader.GetString("PasswordHash");

                        if (BCrypt.Net.BCrypt.Verify(password, storedPasswordHash)) 
                        {
                            return new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                Username = reader.GetString("Username"),
                                Email = reader.GetString("Email"),
                                Role = reader.GetString("Role"), 
                                Password = storedPasswordHash
                            };
                        }
                    }
                }
            }
            return null;
        }

    }
}