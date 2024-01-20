using NuGet.Protocol.Plugins;

namespace CarVilla.Areas.ViewModels
{
    public class UpdateProductVm
    {
        public int Id { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
