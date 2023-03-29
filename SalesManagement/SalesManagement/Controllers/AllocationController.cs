using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SalesManagement.Model;
using System.Data;

namespace SalesManagement.Controllers
{
    [Route("controller")]
    [ApiController]
    public class AllocationController : Controller
    {
        private readonly IConfiguration _configuration;
        public AllocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetAllocation")]
        public ActionResult<IList<Allocation>> GetAllocation()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Allocation> allocations = new List<Allocation>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "select * from ProductAllocation";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allocations.Add(new Allocation
                            {
                                AllocationID = Convert.ToInt64(reader["AllocationID"]),
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                ProductID = Convert.ToInt64(reader["ProductID"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            });
                        }
                    }
                }
            }
            return allocations;
        }

        [HttpPost]
        [Route("PostAllocation")]

        public ActionResult<IList<Allocation>> PostAllocation(Allocation all)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Allocation> allocations = new List<Allocation>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                bool productid = false;
                bool personid = false;
                using (SqlCommand cmd = new SqlCommand("select COUNT(*) from product where ProductID = '" + all.ProductID + @"'", connect))
                {
                    cmd.Parameters.AddWithValue("ProductID", all.ProductID);
                    productid = (int)cmd.ExecuteScalar() > 0;
                }
                using (SqlCommand cmd = new SqlCommand("select COUNT(*) from SalesPerson where PersonID = '" + all.PersonID + @"'", connect))
                {
                    cmd.Parameters.AddWithValue("PersonID", all.PersonID);
                    personid = (int)cmd.ExecuteScalar() > 0;
                }
                if (productid || personid)
                {
                    string query = "InsertAllocation";
                    using (SqlCommand command = new SqlCommand(query))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@PersonID", all.PersonID);
                        command.Parameters.AddWithValue("@ProductID", all.ProductID);
                        command.Parameters.AddWithValue("@Quantity", all.Quantity);
                        command.Parameters.AddWithValue("@createdBy", all.CreatedBy);
                        command.Parameters.AddWithValue("@ModifiedBy", all.ModifiedBy);
                        command.Connection = connect;
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
                else
                {
                    return BadRequest("Inavild information");
                }
            }
            return allocations;
        }

        [HttpPut]
        [Route("Put")]

        public ActionResult<IList<Allocation>> PutAllocation(Allocation all)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Allocation> allocations = new List<Allocation>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "UpdateAllocation";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@AllocationID", all.AllocationID);
                    command.Parameters.AddWithValue("@PersonID", all.PersonID);
                    command.Parameters.AddWithValue("@ProductID", all.ProductID);
                    command.Parameters.AddWithValue("@Quantity", all.Quantity);
                    command.Parameters.AddWithValue("@CreatedBy", all.CreatedBy);
                    command.Parameters.AddWithValue("@ModifiedBy", all.ModifiedBy);
                    command.Connection = connect;
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
            return allocations;
        }

        [HttpDelete]
        [Route("Delete")]

        public ActionResult<Allocation> DeleteAllocation(int allocationID)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            Allocation allocations = new Allocation();
            try
            {
                using (SqlConnection connect = new SqlConnection(connection))
                {
                    string query = @"DELETE  FROM ProductAllocation
                               WHERE AllocationID = '" + allocationID + @"' 
                               ";
                    using (SqlCommand command = new SqlCommand(query))
                    {
                        command.Connection = connect;
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest ("its have a forrign key refernce");
            }
            return allocations;
        }
        [HttpGet]
        [Route("getID")]
        public Allocation getId(int ID)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            Allocation allocation = new Allocation();
            using (SqlConnection connect = new(connection))
            {
                string query = "sp_GetAllocation";
                using (SqlCommand Cmd = new(query))
                {
                    Cmd.Connection = connect;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@AllocationID", ID);
                    connect.Open();
                    using (SqlDataReader reader = Cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allocation = new Allocation
                            {
                                AllocationID = Convert.ToInt64(reader["AllocationID"]),
                                PersonID = Convert.ToInt64(reader["PersonID"]),
                                ProductID = Convert.ToInt64(reader["ProductID"]),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)reader["CreatedDate"],
                                ModifiedDate = (DateTime)reader["ModifiedDate"]
                            };
                        }
                    }
                    connect.Close();
                }
                return allocation;
            }
        }
    }
}
