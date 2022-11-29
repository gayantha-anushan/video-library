using System.Data.SqlClient;
using System.Data;

namespace BlazeLibrary.Data
{
    public class ReservationService
    {
        private String connectionString = "Data Source=DESKTOP-U76GMVG\\SQLEXPRESS;Initial Catalog=bl_360_sample;Integrated Security=True";
        public Task<List<ExtendedReservation>> GetAllReservations()
        {
            List<ExtendedReservation> list = new List<ExtendedReservation>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("getAllReservatons", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    ExtendedReservation er = new ExtendedReservation();
                    er.VideoId = sdr.GetInt32(0);
                    er.MemberId = sdr.GetInt32(1);
                    er.startDate = sdr.GetDateTime(2);
                    er.endDate = sdr.GetDateTime(3);
                    er.MemberFirstName = sdr.GetString(4);
                    er.MemberLastName = sdr.GetString(5);
                    er.VideoName = sdr.GetString(6);
                    list.Add(er);
                }
            }
            return Task.FromResult(list);
        }
        public bool ReserveVideo(Reservatio reservation)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("reserveVideo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vid",
                    Value = reservation.VideoId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@mid",
                    Value = reservation.UserId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@st_dt",
                    Value = reservation.StartDate,
                    SqlDbType = SqlDbType.Date,
                    Direction = ParameterDirection.Input,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@du_dt",
                    Value = reservation.DueDate,
                    SqlDbType = SqlDbType.Date,
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
        public bool DeleteReservation(int VideoId,int MemberId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("deleteReservations", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vid",
                    Value = VideoId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input,
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@mid",
                    Value = MemberId,
                    SqlDbType = SqlDbType.Int,
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
