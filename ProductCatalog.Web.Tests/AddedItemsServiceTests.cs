using ProductCatalog.Web.Business.Service;
using ProductCatalog.Web.Utility.Model;

namespace ProductCatalog.Web.Tests
{
    [TestFixture]
    public class AddedItemsServiceTests
    {
        private AddedItemsService _sut = null!;

        [SetUp]
        public void SetUp()
        {
            _sut = new AddedItemsService();
        }

        [Test]
        public void Add_AppendsItemToItems()
        {
            var item = new AddedListItem { ProductId = 1, Title = "Bananas", City = "Chicago", ShareMessage = "msg" };

            _sut.Add(item);

            Assert.That(_sut.Items, Has.Count.EqualTo(1));
            Assert.That(_sut.Items[0].Title, Is.EqualTo("Bananas"));
        }

        [Test]
        public void Add_MultipleItems_PreservesOrder()
        {
            _sut.Add(new AddedListItem { ProductId = 1, Title = "First" });
            _sut.Add(new AddedListItem { ProductId = 2, Title = "Second" });

            Assert.That(_sut.Items, Has.Count.EqualTo(2));
            Assert.That(_sut.Items[0].Title, Is.EqualTo("First"));
            Assert.That(_sut.Items[1].Title, Is.EqualTo("Second"));
        }

        [Test]
        public void Add_RaisesOnChange()
        {
            var raised = false;
            _sut.OnChange += () => raised = true;

            _sut.Add(new AddedListItem { ProductId = 1, Title = "Bananas" });

            Assert.That(raised, Is.True);
        }

        [Test]
        public void Add_NullItem_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _sut.Add(null!));
        }
    }
}
