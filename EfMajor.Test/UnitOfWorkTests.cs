using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EfMajor.Test
{
    public class UnitOfWorkTests
    {
        private Customer _customer;

        public UnitOfWorkTests()
        {
            _customer = TestData.Customer_Fred;
        }

        [Fact]
        public void Save_SavesData()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);

                target.Save();

                Assert.True(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public async Task SaveAsync_SavesData()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);

                await target.SaveAsync();

                Assert.True(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void Add_NotExists_AddsEntity()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);
                target.Save();

                Assert.True(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void Add_Exists_Throws()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);
                target.Save();
                Assert.True(target.Exists<Customer>(_customer.Id));

                target.Add(_customer);
                Assert.Throws<DbUpdateException>(() => target.Save());
            }
        }

        [Fact]
        public void Get_Exists_ReturnsEntity()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);
                target.Save();

                var result = target.Get<Customer>(_customer.Id);

                Assert.Equal(_customer.FirstName, result.FirstName);
            }
        }

        [Fact]
        public void Get_NotExists_ReturnsNull()
        {
            using (var target = BasicUow.Uow)
            {
                var result = target.Get<Customer>(_customer.Id);

                Assert.Null(result);
            }
        }

        [Fact]
        public void Exists_DoesExist_ReturnsTrue()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);
                target.Save();

                Assert.True(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void Exists_NotExist_ReturnsFalse()
        {
            using (var target = BasicUow.Uow)
            {
                Assert.False(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void DeleteByEntity_Exists_Deletes()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);
                target.Save();
                Assert.True(target.Exists<Customer>(_customer.Id));

                target.DeleteEntity(_customer);
                target.Save();

                Assert.False(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void DeleteByEntity_NotExists_DoesNotThrow()
        {
            using (var target = BasicUow.Uow)
            {
                Assert.False(target.Exists<Customer>(_customer.Id));

                target.DeleteEntity(_customer);
                target.Save();
                Assert.False(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void DeleteById_Exists_Deletes()
        {
            using (var target = BasicUow.Uow)
            {
                target.Add(_customer);
                target.Save();
                Assert.True(target.Exists<Customer>(_customer.Id));

                target.DeleteId<Customer>(_customer.Id);
                target.Save();

                Assert.False(target.Exists<Customer>(_customer.Id));
            }
        }

        [Fact]
        public void DeleteById_NotExists_DoesNotThrow()
        {
            using (var target = BasicUow.Uow)
            {
                Assert.False(target.Exists<Customer>(_customer.Id));

                target.DeleteId<Customer>(_customer.Id);
                target.Save();

                Assert.False(target.Exists<Customer>(_customer.Id));
            }
        }
    }
}
