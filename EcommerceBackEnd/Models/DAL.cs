using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EcommerceBackEnd.Models
{
    public class DAL
    {
        public Response register(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_register", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Username", users.Username);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@Type", "Users");
            cmd.Parameters.AddWithValue("@ActionType", "ADD");
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "User registered successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User registration failed";
            }
            return response;
        }
        public Response login(UsersLogin users, SqlConnection connection)
        {
            SqlDataAdapter da = new SqlDataAdapter("sp_login", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
            da.SelectCommand.Parameters.AddWithValue("@Password", users.Password);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.Username = Convert.ToString(dt.Rows[0]["Username"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Role = Convert.ToString(dt.Rows[0]["Type"]);
                response.StatusCode = 200;
                response.StatusMessage = "User is valid";
                response.User = user;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User is invalid";
                response.User = null;
            }
            return response;
        }
        public Response ChangePassword(string email,string currentPassword,string NewPassword, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("sp_change_password", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@currentPassword", currentPassword);
            cmd.Parameters.AddWithValue("@NewPassword", NewPassword);
            connection.Open();
            string message = cmd.ExecuteScalar().ToString();
            connection.Close();
            return new Response() { StatusMessage = message,StatusCode=200 };
        }
        public Response updateProfile(Users users, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_register", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", users.ID);
            cmd.Parameters.AddWithValue("@Username", users.Username);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email);
            cmd.Parameters.AddWithValue("@ActionType", "UPDATE");
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Record updated successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Some error occured. Try after sometime";
            }

            return response;
        }
        public Response AddUpdateAddress(UserAddress users, SqlConnection connection, string type)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_address", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", users.ID);
            cmd.Parameters.AddWithValue("@Address", users.Address);
            cmd.Parameters.AddWithValue("@ActionType", type);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                if (type == "ADD")
                    response.StatusMessage = "Address added successfully";
                else
                    response.StatusMessage = "Record updated successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Some error occured. Try after sometime";
            }

            return response;
        }
        public string RemoveFromWishList(int wishId, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("sp_removewishlist", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@wishId", wishId);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            return "remove from cart";
        }
        public string RemoveFromCart(int cartId, SqlConnection connection)
        {
            SqlCommand cmd = new SqlCommand("sp_removecart", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@cartID", cartId);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            return "remove from cart";
        }
        public Response addToCart(Cart cart, SqlConnection connection, string type)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddToCart", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", cart.ID);
            cmd.Parameters.AddWithValue("@UserID", cart.UserID);
            cmd.Parameters.AddWithValue("@Quantity", 1);
            cmd.Parameters.AddWithValue("@ProductID", cart.ProductID);
            cmd.Parameters.AddWithValue("@ActionType", type);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                if (type == "ADD")
                    response.StatusMessage = "Item added successfully";
                else
                    response.StatusMessage = "Item removed successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Item could not be added";
            }
            return response;
        }
        public List<CardWishListResponse> cartList(string email, SqlConnection connection)
        {
            Response response = new Response();
            List<CardWishListResponse> listCart = new List<CardWishListResponse>();
            SqlDataAdapter da = new SqlDataAdapter("sp_getAddToCardProducts", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CardWishListResponse obj = new CardWishListResponse();
                obj.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                obj.Name = Convert.ToString(dt.Rows[i]["Name"]);
                obj.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                obj.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                obj.Image = Convert.ToString(dt.Rows[i]["Image"]);
                obj.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                obj.Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"]);
                obj.Email = Convert.ToString(dt.Rows[i]["Email"]);
                listCart.Add(obj);
            }
            return listCart;
        }
        public List<CardWishListResponse> WishList(string email, SqlConnection connection)
        {
            Response response = new Response();
            List<CardWishListResponse> listCart = new List<CardWishListResponse>();
            SqlDataAdapter da = new SqlDataAdapter("sp_getAddToWishListProducts", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@email", email);
            DataTable dt = new DataTable();
            da.Fill(dt);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                CardWishListResponse obj = new CardWishListResponse();
                obj.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                obj.Name = Convert.ToString(dt.Rows[i]["Name"]);
                obj.ProductID = Convert.ToInt32(dt.Rows[i]["ProductID"]);
                obj.UserId = Convert.ToInt32(dt.Rows[i]["UserId"]);
                obj.Image = Convert.ToString(dt.Rows[i]["Image"]);
                obj.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                obj.Email = Convert.ToString(dt.Rows[i]["Email"]);
                listCart.Add(obj);
            }
            return listCart;
        }

        public Response addToWishList(WishList list, SqlConnection connection, string type)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_AddToWishList", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", list.ID);
            cmd.Parameters.AddWithValue("@UserID", list.UserID);
            cmd.Parameters.AddWithValue("@ProductID", list.ProductID);
            cmd.Parameters.AddWithValue("@ActionType", type);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                if (type == "ADD")
                    response.StatusMessage = "Item added to wishlst successfully";
                else
                    response.StatusMessage = "Item removed from wishlst successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Item could not be added";
            }
            return response;
        }
        public Response GetProductById(int productId, SqlConnection connection)
        {
            Response response = new Response();
            Product listWishList = new Product();
            SqlDataAdapter da = new SqlDataAdapter("sp_getProductById", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@productId", productId);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Product objProduct = new Product();
                objProduct.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                objProduct.Name = Convert.ToString(dt.Rows[0]["Name"]);
                objProduct.Image = Convert.ToString(dt.Rows[0]["Image"]);
                objProduct.Price = Convert.ToDecimal(dt.Rows[0]["Price"]);
                objProduct.Quantity = Convert.ToInt32(dt.Rows[0]["Quantity"]);
                response.Product = objProduct;

            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "product by id details are not available";
            }
            return response;
        }
        public Response DeleteProductById(int productId, SqlConnection connection)
        {
            Response response = new Response();
            Product listWishList = new Product();
            SqlCommand cmd = new SqlCommand("sp_deleteProductById", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productId", productId);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();

            response.StatusMessage = "deleted successfully";
            return response;
        }
        public Response addUpdateProduct(Product product, SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_addUpdateProduct", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", product.ID);
            cmd.Parameters.AddWithValue("@Name", product.Name);
            cmd.Parameters.AddWithValue("@Image", product.Image);
            cmd.Parameters.AddWithValue("@Price", product.Price);
            cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            cmd.Parameters.AddWithValue("@Type", product.ID > 0 ? "Update" : "ADD");
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Product inserted successfully";
            }
            return response;
        }
        public Response listProduct(SqlConnection connection)
        {
            Response response = new Response();
            SqlCommand cmd = new SqlCommand("sp_GetProducts", connection);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt = new DataTable();
            List<Product> listProduct = new List<Product>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Product objProduct = new Product();
                    objProduct.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    objProduct.Name = Convert.ToString(dt.Rows[i]["Name"]);
                    objProduct.Image = Convert.ToString(dt.Rows[i]["Image"]);
                    objProduct.Price = Convert.ToDecimal(dt.Rows[i]["Price"]);
                    objProduct.Quantity = Convert.ToInt32(dt.Rows[i]["Quantity"]);
                    listProduct.Add(objProduct);
                }
                if (listProduct.Count > 0)
                {
                    response.StatusCode = 200;
                    response.listProduct = listProduct;
                }
                else
                {
                    response.StatusCode = 100;
                    response.listProduct = null;
                }

            }

            return response;
        }
    }
}
