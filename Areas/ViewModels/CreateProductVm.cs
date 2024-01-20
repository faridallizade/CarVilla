namespace CarVilla.Areas.ViewModels
{
    public class CreateProductVm
    {
        public IFormFile ImageFile { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
    }
}
