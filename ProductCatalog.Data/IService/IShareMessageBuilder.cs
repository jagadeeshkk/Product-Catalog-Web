using System;
using System.Collections.Generic;
using System.Text;

namespace ProductCatalog.Web.Business.IService
{
    public interface IShareMessageBuilder
    {
        string BuildAddToListMessage(string title, string price, string? city);
    }
}
