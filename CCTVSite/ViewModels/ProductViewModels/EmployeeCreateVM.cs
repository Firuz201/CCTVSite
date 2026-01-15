namespace CCTVSite.ViewModels.ProductViewModels
{
    public class EmployeeCreateVM
    {
        public string FullName { get; set; } = string.Empty;

        public int PositionId { get; set; }

        public IFormFile Image { get; set; } = null!;
    }
}
