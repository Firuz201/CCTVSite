using CCTVSite.Models.Common;

namespace CCTVSite.Models
{
    public class Position : BaseEntity
    {
        public string Title { get; set; } = null!;

        public ICollection<Employee> Employees { get; set; } = [];
    }
}
