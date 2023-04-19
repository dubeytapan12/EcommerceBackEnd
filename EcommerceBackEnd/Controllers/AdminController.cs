using EcommerceBackEnd.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        IWebHostEnvironment _hostEnvironment;
        public AdminController(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
        }

        [HttpPost]
        [Route("addUpdateProduct")]
        public Response addUpdateProduct([FromForm] Product product)
        {
            string dirPath = "D:\\Freelance\\Shopping.UI\\src\\assets\\ProductImages";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            string dataFileName = DateTime.Now.Ticks + "." + Path.GetFileName(product.FormFile.FileName).Split(".")[1];
            string extension = Path.GetExtension(dataFileName);
            string[] allowedExtsnions = new string[] { ".jpeg", ".png", ".gif",".jpg" };

            if (!allowedExtsnions.Contains(extension))
                throw new Exception("Sorry! This file is not allowed, make sure that file having extension as either.jpeg or.png is uploaded.");

            // Make a Copy of the Posted File from the Received HTTP Request
            string saveToPath = Path.Combine(dirPath, dataFileName);

            using (FileStream stream = new FileStream(saveToPath, FileMode.Create))
            {
                product.FormFile.CopyTo(stream);
                product.Image = dataFileName;
            }

           
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.addUpdateProduct(product, connection);
            return response;
        }

        [HttpGet]
        [Route("listProduct")]
        public Response listProduct()
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.listProduct(connection);
            return response;
        }

        [HttpGet]
        [Route("GetProductById/{ProductId}")]
        public Response GetProductById(int ProductId)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.GetProductById(ProductId,connection);
            return response;

        }
        [HttpDelete]
        [Route("Product/{ProductId}")]
        public Response DeleteProductById(int ProductId)
        {
            DAL dal = new DAL();
            SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.DeleteProductById(ProductId, connection);
            return response;

        }

    }
}
