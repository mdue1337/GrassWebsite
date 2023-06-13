using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GrassDatabase
{
    public class SqliteDataAccess
    {
        private string ConnectionString;

        public SqliteDataAccess(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public List<UserModel> LoadUser()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                var output = cnn.Query<UserModel>("SELECT * FROM USERS", new DynamicParameters());
                return output.ToList();
            }
        }

        public List<UserModel> LoadUserSpecific(int userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "SELECT * FROM USERS WHERE ID = @Id";
                var output = cnn.Query<UserModel>(sql, new {Id = userId });
                return output.ToList();
            }
        }

        public void UpdateBalance(int id, int balance)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "UPDATE USERS SET Balance = @Balance WHERE Id = @UserId";
                cnn.Execute(sql, new { Balance = balance, UserId = id });
            }
        }

        public void WithdrawBalance(int balance, int userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "UPDATE USERS SET Balance = Balance - @Balance WHERE Id = @UserId";
                cnn.Execute(sql, new { Balance = balance, UserId = userId });
            }
        }

        public long AddJob(JobDetails job)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "INSERT INTO JOBDETAILS (Day, Month, Year, JobTime) VALUES (@Day, @Month, @Year, @JobTime) RETURNING JOBID";
                var x = cnn.Query(sql, new { Day = job.Day, Month = job.Month, Year = job.Year, JobTime = job.JobTime });
                var jobId = x.FirstOrDefault().JobId;
                return jobId;
            }
        }

        public void UpdateBalanceBy50(int userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "UPDATE USERS SET Balance = Balance + 50 WHERE Id = @UserId";
                cnn.Execute(sql, new { UserId = userId });
            }
        }

        public void JobUser(int userId, long jobId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "INSERT INTO USERJOBS (UserId, JobId) VALUES (@UserId, @JobId)";
                cnn.Execute(sql, new { UserId = userId, JobId = jobId });
            }
        }

        public bool CheckIfUserExist(int userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM USERS WHERE Id = @Id";
                var num = cnn.QueryFirstOrDefault<int>(sql, new { Id = userId });

                if (num == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CheckIfJobExist(int jobId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "SELECT COUNT(*) FROM JOBDETAILS WHERE JobId = @JobId";
                var num = cnn.QueryFirstOrDefault<int>(sql, new { JobId = jobId });

                if (num == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public List<UserJobs> GetUserJobsTabelSpecific(int userId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "SELECT * FROM USERJOBS WHERE UserId = @UserId";
                var num = cnn.Query<UserJobs>(sql, new { Userid = userId });
                return num.ToList();
            }
        }

        public List<UserJobs> GetUserJobsTabel()
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "SELECT * FROM USERJOBS";
                var num = cnn.Query<UserJobs>(sql, new DynamicParameters());
                return num.ToList();
            }
        }

        public List<JobDetails> GetJobDetailsTabel(int jobId)
        {
            using (IDbConnection cnn = new SQLiteConnection(ConnectionString))
            {
                string sql = "SELECT * FROM JOBDETAILS WHERE JobId = @JobId";
                var num = cnn.Query<JobDetails>(sql, new {JobId = jobId});
                return num.ToList();
            }
        }
    }
}
