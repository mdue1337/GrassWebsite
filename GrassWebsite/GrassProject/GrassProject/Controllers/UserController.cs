using GrassDatabase;
using Microsoft.AspNetCore.Mvc;

namespace test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly SqliteDataAccess _db;

        public UserController(ILogger<UserController> logger, SqliteDataAccess db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet("GetAllUsers")]
        public IActionResult Get()
        {
            return Ok(_db.LoadUser());
        }

        [HttpGet("GetSpecificUser")]
        public IActionResult GetSpecific(int userId)
        {
            try
            {
                bool userExist = _db.CheckIfUserExist(userId);

                if (userExist == false)
                {
                    throw new Exception("UserId does not exist");
                }

                return Ok(_db.LoadUserSpecific(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetUserJobsForAll")]
        public IActionResult UserJob()
        {
            try
            {
                return Ok(_db.GetUserJobsTabel());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserJobsForSpecific")]
        public IActionResult UserJobSpecific(int userId)
        {
            try
            {
                bool userExist = _db.CheckIfUserExist(userId);

                if (userExist == false)
                {
                    throw new Exception("UserId does not exist");
                }

                return Ok(_db.GetUserJobsTabelSpecific(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetJobInfo")]
        public IActionResult JobInfo(int jobId)
        {
            try
            {
                bool userExist = _db.CheckIfJobExist(jobId);

                if (userExist == false)
                {
                    throw new Exception("JobId does not exist");
                }

                return Ok(_db.GetJobDetailsTabel(jobId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateBalance")]
        public IActionResult UpdateBalance(int id, int balance)
        {
            try
            {
                bool userExist = _db.CheckIfUserExist(id);

                if (userExist == false)
                {
                    throw new Exception("UserId does not exist");
                }
                if (balance <= 0) 
                {
                    throw new Exception("Balance must be greater than 0 or equal to zero");
                }
                _logger.LogInformation("Updating info");
                _db.UpdateBalance(id, balance);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update balance");
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("WithdrawBalance")]
        public IActionResult WithdrawBalance(int balance, int id)
        {
            try
            {
                bool userExist = _db.CheckIfUserExist(id);

                if (userExist == false)
                {
                    throw new Exception("UserId does not exist");
                }
                if (balance <= 0)
                {
                    throw new Exception("Balance must be greater than 0 or equal to zero");
                }
                _db.WithdrawBalance(balance, id);
                return Ok("Withdrew " + balance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update balance");
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddJob")]
        public IActionResult Job(int userId, int day, string month, int year, int jobTime)
        {
            try
            {
                bool userExist = _db.CheckIfUserExist(userId);

                if (userExist == false)
                {
                    throw new Exception("UserId does not exist");
                }
                long id = _db.AddJob(new JobDetails()
                {
                    Day = day,
                    Month = month,
                    Year = year,
                    JobTime = jobTime
                });
                _db.UpdateBalanceBy50(userId);
                _db.JobUser(userId, id);

                return Ok("Created a job with the JobId: " + id + ". Done by the user with the Id: " + userId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}