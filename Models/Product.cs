using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FormsApp.Models{
    
    public class Product
    {
        [Display(Name= "Urun Id")]
        public int ProductId { get; set; }

        [Required] //girilmesi zorunlu alan - boş bırakılınca uyarı yazısı verir.
        [StringLength(100)] //100 karakterli bir isim girlimeli
        [Display(Name= "Urun Adı")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0,100000)] //0 ile 100.000 arasında bir değer girilmeli
        [Display(Name= "Fiyat")]
        public decimal? Price { get; set; }

        [Display(Name= "Resim")]
        public string? Image { get; set; }= string.Empty;

        public bool IsActive { get; set; }

        [Display(Name="Category")]
        [Required]
        public int? CategoryId { get; set; }
    }
    
}