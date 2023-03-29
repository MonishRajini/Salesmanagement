using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SalesManagement.Model;
using System.Data;

namespace SalesManagement.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("GetProduct")]

        public ActionResult<IList<Product>> GetProduct()
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Product> products = new List<Product>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Product";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.Connection = connect;
                    connect.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductID = Convert.ToInt32(reader["ProductID"]),
                                ProductName = reader["ProductName"].ToString(),
                                UnitPrice = Convert.ToDecimal(reader["UnitPrice"]),
                                CreatedBy = reader["CreatedBy"].ToString(),
                                ModifiedBy = reader["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)(reader["CreatedDate"]),
                                ModifiedDate = (DateTime)(reader["ModifiedDate"])
                            });
                        }
                    }
                    connect.Close();
                }
            }
            return products;
        }

        [HttpPost]
        [Route("PostProduct")]
        public ActionResult<IList<Product>> PostProduct(Product pro)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Product> products = new List<Product>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                bool exist = false;
                using (SqlCommand command1 = new SqlCommand("select count(*) from product where  ProductName = '" + pro.ProductName + @"'", connect))
                {
                    command1.Parameters.AddWithValue("ProductName", pro.ProductName);
                    exist = (int)command1.ExecuteScalar() > 0;
                }
                if (exist)
                {
                    return BadRequest("ProductName is already exists");
                }
                else
                {
                    string query = "InsertProduce";
                    using (SqlCommand command = new SqlCommand(query))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProductName", pro.ProductName);
                        command.Parameters.AddWithValue("@UnitPrice", pro.UnitPrice);
                        command.Parameters.AddWithValue("@CreatedBy", pro.CreatedBy);
                        command.Parameters.AddWithValue("@ModifiedBy", pro.ModifiedBy);
                        command.Parameters.AddWithValue("@CreatedDate", pro.CreatedDate);
                        command.Connection = connect;

                        command.ExecuteNonQuery();
                        connect.Close();
                    }
                }
            }
            return products;
        }

        [HttpPut]
        [Route("Put")]

        public ActionResult<IList<Product>> PutProduct(Product pro)
        {
            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Product> products = new List<Product>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                string query = "PutProduct";
                using (SqlCommand command = new SqlCommand(query))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ProductID", pro.ProductID);
                    command.Parameters.AddWithValue("@ProductName", pro.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", pro.UnitPrice);
                    command.Parameters.AddWithValue("@CreatedBy", pro.CreatedBy);
                    command.Parameters.AddWithValue("@ModifiedBy", pro.ModifiedBy);
                    command.Connection = connect;
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                }
            }
            return products;
        }

        [HttpDelete]
        [Route("Delete")]

        public ActionResult<IList<Product>> DeleteProduct( int Id)
        {

            string connection = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            List<Product> products = new List<Product>();
            using (SqlConnection connect = new SqlConnection(connection))
            {
                connect.Open();
                bool productID = false;
                using (SqlCommand cmd = new SqlCommand("select COUNT(*) from Product where ProductID = '" + Id  + @"'", connect))
                {
                    cmd.Parameters.AddWithValue("ProductID",Id );
                    productID = (int)cmd.ExecuteScalar() > 0;
                }
                if (productID)
                {
                    string query = @"DELETE  FROM Product
                               WHERE ProductID = '" + Id + @"' 
                               ";
                    using (SqlCommand com = new(query))
                    {
                        com.Connection = connect;
                        com.ExecuteNonQuery();
                        connect.Close();
                    }
                }
                else
                {
                    return BadRequest("Invaild ProductID");
                }
            }
            return products;
        }
            
        
        [HttpGet]
        [Route("getID")]
        public Product getId(int ID)
        {
            string ProductDB = _configuration.GetValue<string>("ConnectionStrings:SalesManagement");
            Product products = new Product();
            using (SqlConnection connection = new(ProductDB))
            {
                string query = "sp_GetAllProducts";
                using (SqlCommand Cmd = new(query))
                {
                    Cmd.Connection = connection;
                    Cmd.CommandType = CommandType.StoredProcedure;
                    Cmd.Parameters.AddWithValue("@ProductID", ID);

                    connection.Open();
                    using (SqlDataReader sdr = Cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            products = new Product
                            {
                                ProductID = Convert.ToInt32(sdr["ProductID"]),
                                ProductName = sdr["Productname"].ToString(),
                                UnitPrice = Convert.ToDecimal(sdr["UnitPrice"]),
                                CreatedBy = sdr["CreatedBy"].ToString(),
                                ModifiedBy = sdr["ModifiedBy"].ToString(),
                                CreatedDate = (DateTime)(sdr["CreatedDate"]),
                                ModifiedDate = (DateTime)(sdr["ModifiedDate"])
                            };
                        }
                    }
                    connection.Close();
                }
                return products;
            }
        }
    }
}
