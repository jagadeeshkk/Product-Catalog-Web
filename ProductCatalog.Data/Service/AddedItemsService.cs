using ProductCatalog.Web.Data.IService;
using ProductCatalog.Web.Utility.Model;

namespace ProductCatalog.Web.Data.Service
{
    public class AddedItemsService : IAddedItemsService
    {
        private readonly List<AddedListItem> _items = new();

        public IReadOnlyList<AddedListItem> Items => _items.AsReadOnly();

        public event Action? OnChange;

        public void Add(AddedListItem item)
        {
            if (item is null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _items.Add(item);
            OnChange?.Invoke();
        }
    }
}
