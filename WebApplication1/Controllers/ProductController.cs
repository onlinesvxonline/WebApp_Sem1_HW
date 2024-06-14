using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        [HttpPost]
        public ActionResult<int> AddProduct(string name, string description, decimal price)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                if (storageContext.Products.Any(p => p.Name == name))
                    return StatusCode(409);

                var product = new Product() { Name = name, Description = description, Price = price };
                storageContext.Products.Add(product);
                storageContext.SaveChanges();
                return Ok(product.Id);
            }
        }

        [HttpDelete]
        public ActionResult<string> DeleteProduct(int productId)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                var product = storageContext.Products.Find(productId);
                if (product == null)
                    return NotFound($"ProductID ={productId} не найден");

                storageContext.Products.Remove(product);
                storageContext.SaveChanges();
                return Ok($"ProductID ={productId} удалён!");
            }
        }

        [HttpPut]
        public ActionResult<string> UpdateProductPrice(int productId, decimal price)
        {
            using (StorageContext storageContext = new StorageContext())
            {
                var product = storageContext.Products.Find(productId);
                if (product == null)
                    return NotFound($"ProductID ={productId} не найден");

                product.Price = price;
                storageContext.SaveChanges();
                return Ok($"Цена ProductID ={productId} обновлена на {price}");
            }
        }

        [HttpGet("get_products")]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            IEnumerable<Product> list;
            using (StorageContext storageContext = new StorageContext())
            {
                list = storageContext.Products.Select(p => new Product { Id = p.Id, Name = p.Name, Description = p.Description, Price = p.Price }).ToList();
                return Ok(list);
            }
        }
    }
}