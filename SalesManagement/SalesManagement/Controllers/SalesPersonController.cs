using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SalesManagement.Model;
using System.Data;

namespace SalesManagement.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalesPersonController : Controller
    {
        private readonly IConfiguration _configuration;
        public SalesPersonController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("Get")]
        public ActionResult<IList<SalesPerson>> GetPerson()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<SalesPerson> person = new List<SalesPerson>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "select * from SalesPerson";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person.Add(new SalesPerson
                            {
                                PersonID = Convert.ToInt32(reader["PersonID"]),
                                PersonName = reader["PersonName"].ToString(),
                                city = reader["city"].ToString(),
                                state = reader["State"].ToString(),
                                PhoneNumber = Convert.ToInt64(reader["ContactNumber"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            });
                        }
                    }
                }
            }
            return person;
        }

        [HttpPost]
        [Route("Post")]

        public ActionResult<IList<SalesPerson>> PostPerson(SalesPerson person)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<SalesPerson> persons = new List<SalesPerson>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "InsertSalesPerson";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonName", person.PersonName);
                    command.Parameters.AddWithValue("@city", person.city);
                    command.Parameters.AddWithValue("@state", person.state);
                    command.Parameters.AddWithValue("@contactNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@CreatedBy", person.CreatedBy);
                    command.Parameters.AddWithValue("@ModifiedBy", person.ModifiedBy);
                    command.Connection = connect;
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }

            }
            return persons;
        }

        [HttpPut]
        [Route("Put")]

        public ActionResult<IList<SalesPerson>> PutPerson(SalesPerson person)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<SalesPerson> persons = new List<SalesPerson>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "PutSalesPerson";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PersonID", person.PersonID);
                    command.Parameters.AddWithValue("@PersonName", person.PersonName);
                    command.Parameters.AddWithValue("@city", person.city);
                    command.Parameters.AddWithValue("@state", person.state);
                    command.Parameters.AddWithValue("@contactNumber", person.PhoneNumber);
                    command.Parameters.AddWithValue("@CreatedBy", person.CreatedBy);
                    command.Parameters.AddWithValue("@ModifiedBy", person.ModifiedBy);
                    command.Connection = connect;
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
            return persons;
        }

        [HttpDelete]
        [Route("Delete")]

        public ActionResult<SalesPerson> DeletePerson(int ID)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            SalesPerson person = new SalesPerson();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                bool personID = false;
                using (SqlCommand cmd = new SqlCommand("select COUNT(*) from SalesPerson where PersonID = '" + ID + @"'", connect))
                {
                    cmd.Parameters.AddWithValue("PersonID",ID);
                    personID = (int)cmd.ExecuteScalar() > 0;
                }
                if (personID)
                {
                    string query = @"DELETE  FROM SalesPerson
                               WHERE PersonID = '" + personID + @"' 
                               ";
                    using (SqlCommand command = new SqlCommand(query))
                    {
                        command.Connection = connect;
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
                else
                {
                    return BadRequest("Invail PersonID");
                }
            }
            return person;
        }


        [HttpGet]
        [Route("getID")]
        public SalesPerson GetId(int ID)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            SalesPerson person = new SalesPerson();
            using (SqlConnection connect = new(connection))
            {
                string query = "sp_Getsalesperson";
                using (SqlCommand Cmd = new(query))
                {
                    Cmd.Connection = connect;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@PersonID", ID);
                    connect.Open();
                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            person = new SalesPerson
                            {
                                PersonID = Convert.ToInt32(reader["PersonID"]),
                                PersonName = reader["PersonName"].ToString(),
                                city = reader["city"].ToString(),
                                state = reader["State"].ToString(),
                                PhoneNumber = Convert.ToInt64(reader["ContactNumber"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)(reader["CreatedDate"]),
                                ModifiedDate = (DateTime)(reader["ModifiedDate"])
                            };
                        }
                    }
                    connect.Close();
                }
                return person;
            }

        }
    }
}

