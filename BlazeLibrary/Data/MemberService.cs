using System.Data.SqlClient;
using System.Data;

namespace BlazeLibrary.Data
{
    public class MemberService
    {
        private String connectionString = "Data Source=DESKTOP-U76GMVG\\SQLEXPRESS;Initial Catalog=bl_360_sample;Integrated Security=True";

        public Task<List<Member>> getAllMembers()
        {
            List<Member> list = new List<Member>();
            using(SqlConnection connection= new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("getAllMembers", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Member m = new Member();
                    m.Id = reader.GetInt32(0);
                    m.FirstName = reader.GetString(1);
                    m.LastName = reader.GetString(2);
                    m.Telephone = reader.GetString(3);
                    list.Add(m);
                }
            }
            return Task.FromResult(list);
        }
        public bool InsertMember(Member member)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("createMember", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@fname",
                    SqlDbType = SqlDbType.Text,
                    Value = member.FirstName,
                    Direction = ParameterDirection.Input,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@lname",
                    SqlDbType = SqlDbType.Text,
                    Value = member.LastName,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@tele",
                    SqlDbType = SqlDbType.Text,
                    Value = member.Telephone,
                    Direction = ParameterDirection.Input,
                });
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool RemoveMember(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("deleteMember", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@mid",
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                    Direction = ParameterDirection.Input,
                });
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
