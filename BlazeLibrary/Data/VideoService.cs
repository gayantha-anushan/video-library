using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace BlazeLibrary.Data
{
    public class VideoService
    {
        private String connectionString = "Data Source=DESKTOP-U76GMVG\\SQLEXPRESS;Initial Catalog=bl_360_sample;Integrated Security=True";
        public Task<List<Video>> GetAllVideos()
        {
            List<Video> list = new List<Video>();
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("getAllVideos",conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                while(sdr.Read())
                {
                    Video vid = new Video();
                    vid.Id = sdr.GetInt32(0);
                    vid.Description = sdr.GetString(2);
                    vid.Title = sdr.GetString(1);
                    try
                    {
                        vid.borrowed_id = sdr.GetInt32(3);
                    }catch(Exception ex)
                    {
                        vid.borrowed_id = 0;
                    }
                    list.Add(vid);
                }
            }
            return Task.FromResult(list);
        }
        public bool isBorrowed(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("getBorrowedIdVideos", conn);
                SqlParameter sp = new SqlParameter {
                    ParameterName = "@vid",
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(sp);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    int borrowed_id;
                    try
                    {
                        borrowed_id = sdr.GetInt32(0);
                    }
                    catch (Exception ex)
                    {
                        borrowed_id = 0;
                    }
                    if(borrowed_id > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public int isReserved(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("getReservedUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter spm = new SqlParameter { 
                    ParameterName = "@vid",
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                    Direction = ParameterDirection.Input
                };
                cmd.Parameters.Add(spm);
                int count = 0;
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    count++;
                }
                return count;
            }
        }
        public List<int> GetReservedMembers(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("getReservedUsers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter spm = new SqlParameter { 
                    ParameterName = "@vid",
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                    Direction = ParameterDirection.Input
                };
                List<int> users = new List<int>();
                cmd.Parameters.Add(spm);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    users.Add(sdr.GetInt32(0));
                }
                return users;
            }
        }
        public bool InsertVideo(Video video)
        {
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("InsertNewVideo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@title",
                    Value = video.Title,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                });
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@description",
                    SqlDbType = SqlDbType.Text,
                    Value = video.Description,
                    Direction = ParameterDirection.Input,
                });
                int res = cmd.ExecuteNonQuery();
                if(res > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool DeleteVideo(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("deleteVideo", conn);
                    //try
                    SqlParameter spm = new SqlParameter
                    {
                        ParameterName = "@vid",
                        Value = id,
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Input,
                    };
                    cmd.Parameters.Add(spm);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //{
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    //}catch(Exception ex)
                    //{
                    //   return false;
                    //}
                }
            }catch (Exception ex)
            {
                return false;
            }
        }

        public bool LendVideo(int videoId,int memberId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("borrowVideo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vid",
                    Value = videoId,
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Input
                });

                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@mid",
                    Value = memberId,
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
        public bool ReturnVideo(int videoId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("returnVideo", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@vid",
                    Value = videoId,
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
