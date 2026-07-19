using ProductCatalog.Web.Utility.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Web.Business.IService
{
    public interface IAddedItemsService
    {
        IReadOnlyList<AddedListItem> Items { get; }
        event Action? OnChange;
        void Add(AddedListItem item);
    }
}
