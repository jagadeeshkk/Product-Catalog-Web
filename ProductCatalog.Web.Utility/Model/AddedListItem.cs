using System.Diagnostics.CodeAnalysis;

namespace ProductCatalog.Web.Utility.Model
{
    [ExcludeFromCodeCoverage]
    public class AddedListItem
    {
        public int ProductId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string ShareMessage { get; set; } = string.Empty;
        public DateTimeOffset AddedAtUtc { get; set; } = DateTimeOffset.UtcNow;
    }
}
