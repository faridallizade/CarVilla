using CarVilla.Models.Common;

namespace CarVilla.Models
{
    public class Product:BaseEntity
    {
        public string ImageUrl {  get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
