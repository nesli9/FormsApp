﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {

    }

    [HttpGet]
    public IActionResult Index(string searchString, string category)
    {
        var products = Repository.Products;

        if (!String.IsNullOrEmpty(searchString)) //filtreleme işlemi yapılır.
        {
            ViewBag.SearchString = searchString;
            products = products.Where(p => p.Name.ToLower().Contains(searchString)).ToList();
        }

        if (!String.IsNullOrEmpty(category) && category !="0")
        {
            products = products.Where(p => p.CategoryId == int.Parse(category)).ToList();
        }

        // ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");

        var model = new ProductViewModel{
            Products = products,
            Categories = Repository.Categories,
            SelectedCategory = category
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(Product model , IFormFile imageFile)
    {
        var allowedExtensions = new[] {".jpg",".jpeg",".png"};
        var extension = Path.GetExtension(imageFile.FileName);// abc.jpg
        var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}") ; //random jpg dosya ismi oluşturulur.
        var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img", randomFileName); //formdan yüklenen resmi img klasörüne atar

        if (imageFile != null){
            if (!allowedExtensions.Contains(extension)){
                ModelState.AddModelError("","Geçerli bir resim seçiniz.");
            }
        }

        if (ModelState.IsValid) //kullanıcının girdiği değerler doğruysa sayfaya eklenir
        {
            using (var stream = new FileStream(path , FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            model.Image = randomFileName;
            model.ProductId = Repository.Products.Count +1; //formdan alınan veri bilgilerine veri saysı +1 . ıd atanır
            Repository.CreateProduct(model);
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(Repository.Categories,"CategoryId","Name");
        return View(model);
    }

}
