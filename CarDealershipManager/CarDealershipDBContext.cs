using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;


namespace CarDealershipManager
{
    public class CarDealershipDBContext : DbContext
    {
        public CarDealershipDBContext() : base("name=CarDealershipDB")
        {
        }
               
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
    }

    public class User
    {
        [Key]
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public Role Role { get; set; }
    }

    public class Role
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Manufacturer
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string country { get; set; }

        public override string ToString()
        {
            return name + ", " + country;
        }

    }

    public class Brand
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public int manufacturer_id { get; set; }
        [ForeignKey("manufacturer_id")]
        public Manufacturer Manufacturer { get; set; }

        public override string ToString()
        {
            return name;
        }
    }

    public class Car
    {
        [Key]
        public int id { get; set; }
        public string model { get; set; }
        public int year { get; set; }
        public int price { get; set; }
        public string equipment { get; set; }
        public int brand_id { get; set; }
        [ForeignKey("brand_id")]
        public Brand Brand { get; set; }
        public string photo { get; set; }
        public int mileage { get; set; }
        public string color { get; set; }
        public string engine_type { get; set; }
        public string fuel_type { get; set; }

        public string InfoMash
        {
            get
            {
                return $"{Brand?.Manufacturer?.name} / {Brand?.name} / {model}";
            }
        }

        public string Infotype
        {
            get
            {
                return $"{fuel_type} / {engine_type}";
            }
        }

        public override string ToString()
        {
            return $"{Brand?.name} {model}, {year}";
        }
    }
}
