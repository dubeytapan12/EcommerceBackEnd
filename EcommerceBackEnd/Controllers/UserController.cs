using EcommerceBackEnd.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [Route("registration")]
        public Response register(Users users)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            return dal.register(users, connection);
        }

        [HttpPost]
        [Route("login")]
        public Response login(UsersLogin users)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            return dal.login(users, connection);
        }
        [HttpPost]
        [Route("change-password")]
        public Response ChangePassword(ChangePassword changePassword)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            return dal.ChangePassword(changePassword.email,changePassword.currentPassword,changePassword.NewtPassword, connection);
        }

        [HttpPost]
        [Route("updateProfile")]
        public Response updateProfile(Users users)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            return dal.updateProfile(users, connection);
        }

        [HttpPost]
        [Route("addAddress")]
        public Response addAddress(UserAddress userAddress)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            return dal.AddUpdateAddress(userAddress, connection, "ADD");
        }

        [HttpPost]
        [Route("updateAddress")]
        public Response updateAddress(UserAddress userAddress)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            return dal.AddUpdateAddress(userAddress, connection, "UPDATE");
        }
        [HttpGet]
        [Route("get-cart/{email}")]
        public List<CardWishListResponse> GetCarts(string email)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            List<CardWishListResponse> response = dal.cartList(email, connection);
            return response;
        }
        [HttpGet]
        [Route("get-wishlist/{email}")]
        public List<CardWishListResponse> GetWishList(string email)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            List<CardWishListResponse> response = dal.WishList(email, connection);
            return response;
        }

        [HttpPost]
        [Route("addToCart")]
        public Response addToCart(Cart cart)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.addToCart(cart, connection, "ADD");
            return response;
        }

        [HttpDelete]
        [Route("removeFromCart/{cartId}")]
        public Response removeFromCart(int cartId)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
             dal.RemoveFromCart(cartId, connection);
            return new Response() { StatusMessage = "removed from cart", StatusCode = 200 };
        }
        [HttpDelete]
        [Route("removeFromWishList/{wishId}")]
        public Response removeFromWishList(int wishId)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            dal.RemoveFromWishList(wishId, connection);
            return new Response() { StatusMessage = "removed from wishlist", StatusCode = 200 };
        }

        [HttpPost]
        [Route("addToWishlist")]
        public Response addToWishlist(WishList wishList)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.addToWishList(wishList, connection, "ADD");
            return response;
        }

        [HttpPost]
        [Route("removeFromWishlist")]
        public Response removeFromWishlist(WishList wishList)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.addToWishList(wishList, connection, "REMOVE");
            return response;
        }
    }
}
