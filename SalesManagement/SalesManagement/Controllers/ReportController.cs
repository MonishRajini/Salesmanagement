using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SalesManagement.Model;
using System.Data;

namespace SalesManagement.Controllers
{
    [ApiController]
    [Route("controlller")]
    public class ReportController : Controller
    {
        private readonly IConfiguration _configuration;

        public ReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Get")]

        public ActionResult<IList<Report>> Get()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Report> reports = new List<Report>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "Select * from REPORT";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reports.Add(new Report
                            {
                                ReportID = Convert.ToInt64(reader["ReportID"]),
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                ProductID = Convert.ToInt64(reader["ProductID"]),
                                AllocationID = Convert.ToInt64(reader["AllocationID"]),
                                SoldQuanity = Convert.ToInt32(reader["Soldquantity"]),
                                DateTime = (DateTime)reader["Date"],
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            });
                        }
                    }

                }
            }
            return reports;
        }

        [HttpPost]
        [Route("Post")]
        public ActionResult<IList<Report>> post(Report report)
        {
            string connection = _configuration.GetValue<string
                >("ConnectionStrings:SalesManagement");
            List <Report > reports= new List<Report>();
            int allocationquantity = 0;
            using (SqlConnection con = new(connection))
            {
                string qurey2 = "select Quantity  from ProductAllocation where AllocationID =" + report .AllocationID + @"";

                using (SqlCommand com = new(qurey2))
                {
                    com.Connection = con;
                    con.Open();
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allocationquantity = Convert.ToInt32(reader["Quantity"]);
                        }
                        con.Close();
                    }
                }
            }
            using (SqlConnection connect =new SqlConnection (connection))
            {
                connect.Open();
                bool exist=false;
                using (SqlCommand cmd = new SqlCommand("select COUNT(*) from ProductAllocation where ProductID = '" + report.ProductID + @"'AND PersonID='" + report.PersonID + @"'AND AllocationID ='" + report.AllocationID + @"'", connect))
                {
                    cmd.Parameters.AddWithValue("ProductID", report.ProductID);
                    cmd.Parameters.AddWithValue("PersonID", report.PersonID);
                    cmd.Parameters.AddWithValue("AllocationID", report.AllocationID);
                    exist = (int)cmd.ExecuteScalar() > 0;
                }
                if (allocationquantity < report.SoldQuanity)
                {
                    return BadRequest("Invaild Quantity");
                }
                else if (exist)
                {
                    string query = "InsertReport";
                    using (SqlCommand command = new SqlCommand(query))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("PersonID", report.PersonID);
                        command.Parameters.AddWithValue("ProductID", report.ProductID);
                        command.Parameters.AddWithValue("AllocationID", report.AllocationID);
                        command.Parameters.AddWithValue("Soldquantity", report.SoldQuanity);
                        command.Parameters.AddWithValue("Date", report.DateTime);
                        command.Parameters.AddWithValue("CreatedBy", report.CreatedBy);
                        command.Parameters.AddWithValue("ModifiedBy", report.ModifiedBy);
                        command.Connection = connect;
                        command.ExecuteNonQuery();
                        connect.Close();

                    }
                }
                else
                {
                    return BadRequest("INvaild ID ");
                }
            }
            return reports;
        }
        [HttpPut]
        [Route("PUT")]

        public ActionResult<IList<Report>>  put(Report report)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Report> reports= new List<Report>();
            using (SqlConnection connect=new SqlConnection (connection))
            {
                string query = "UpsertReport";
                using (SqlCommand command=new SqlCommand (query))
                {
                    command.CommandType =CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("ReportID", report.ReportID);
                    command.Parameters.AddWithValue("PersonID", report.PersonID);
                    command.Parameters.AddWithValue("ProductID", report.ProductID);
                    command.Parameters.AddWithValue("Soldquantity", report.SoldQuanity);
                    command.Parameters.AddWithValue("Date", report.DateTime);
                    command.Parameters.AddWithValue("CreatedBy", report.CreatedBy);
                    command.Parameters.AddWithValue("ModifiedBy", report.ModifiedBy);
                    command.Connection = connect;
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
            return reports;
        }

        [HttpDelete]
        [Route("DELETE")]
        public ActionResult<IList<Report>> Delete(int ID)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Report> reports= new List<Report>();
            using (SqlConnection connect=new SqlConnection (connection))
            {
                string query = @"DELETE  FROM REPORT
                               WHERE ReportID = '" + ID  + @"' 
                               ";
                using (SqlCommand command =new SqlCommand(query ))
                {
                    command.Connection = connect;
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
            return reports;
        }

        [HttpGet]
        [Route("GetAchievers")]

        public ActionResult<IList<Achievers>> GetAchievers()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Achievers> achievers = new List<Achievers>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "Achievers";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            achievers.Add(new Achievers
                            {
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                PersonName  = reader["PersonName"].ToString(),
                                TotalsoldQuantity = Convert.ToInt32(reader["total_sold"])
                            });
                        }
                    }
                    connect.Close();
                }
            }
            return achievers;
        }
        [HttpGet]
        [Route("MonthAchievers")]

        public ActionResult<IList<Month>> MonthAchievers()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Month > months  = new List<Month >();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "MonthAchievers";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            months .Add(new Month 
                            {
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                PersonName = reader["PersonName"].ToString(),
                                Date = (DateTime)reader["date"],
                                SoldQuantity = Convert.ToInt32(reader["Soldquantity"]),
                            });
                        } 
                    }
                    connect.Close();
                }
            }
            return months;
        }

        [HttpGet]
        [Route("TOP1ACHIEVRES")]

        public ActionResult<IList<TopAchievers>> TOP1ACHIEVRES()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<TopAchievers> topAchievers  = new List<TopAchievers>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "TOP1ACHIEVRES";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            topAchievers.Add(new TopAchievers
                            {
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                ProductID = Convert.ToInt64(reader["ProductID"]),
                                PersonName = reader["PersonName"].ToString(),
                                Date = (DateTime )reader["date"],
                                SoldQuantity = Convert.ToInt32(reader["total_sold"]),
                            });
                        }
                    }
                    connect.Close();
                }
            }
            return topAchievers;
        }

        [HttpGet]
        [Route("getID")]
        public Report  getId(int ID)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            Report report  = new Report();
            using (SqlConnection connect = new(connection))
            {
                string query = "sp_GetReportID";
                using (SqlCommand Cmd = new(query))
                {
                    Cmd.Connection = connect;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@ReportID", ID);
                    connect.Open();
                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            report = new Report
                            {
                                ReportID = Convert.ToInt64(reader["ReportID"]),
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                ProductID = Convert.ToInt64(reader["ProductID"]),
                                AllocationID = Convert.ToInt64(reader["AllocationID"]),
                                SoldQuanity = Convert.ToInt32(reader["Soldquantity"]),
                                DateTime = (DateTime)reader["Date"],
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            };
                        }
                    }
                    connect.Close();
                }
                return report;
            }
        }
    }
}
