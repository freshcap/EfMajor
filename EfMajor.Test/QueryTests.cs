using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EfMajor.Test
{
    public class QueryTests
    {
        [Fact]
        public void GetOne_GetsOne()
        {
            using (var uow = BasicUow.Uow)
            {
                uow.Add(TestData.Customer_Fred);
                uow.Save();

                var result = uow.Customers.FirstOrDefault(x => x.Id == TestData.Customer_Fred.Id);

                Assert.Equal(TestData.Customer_Fred, result);
            }
        }

        [Fact]
        public void GetMany_GetsMany()
        {
            using (var uow = BasicUow.Uow)
            {
                uow.Add(TestData.Customer_Fred);
                uow.Add(TestData.Customer_Wilma);
                uow.Add(TestData.Customer_Barney);
                uow.Save();

                var result = uow.Customers.Where(x => x.LastName == "Flintstone").ToList();

                Assert.Equal(2, result.Count);
            }
        }

        [Fact]
        public void ManyToMany_Include_GetsChildren()
        {
            using (var uow = BasicUow.Uow)
            {
                uow.Add(TestData.Customer_Fred);
                uow.Add(TestData.AddressA);
                uow.Add(TestData.AddressB);
                uow.Add(TestData.CustomerAddress(TestData.Customer_Fred.Id, TestData.AddressA.Id));
                uow.Add(TestData.CustomerAddress(TestData.Customer_Fred.Id, TestData.AddressB.Id));
                uow.Save();

                var result = uow.Customers
                    .Include(x => x.CustomerAddresses).ThenInclude(x => x.Address)
                    .FirstOrDefault(x => x.Id == TestData.Customer_Fred.Id);

                Assert.Equal(2, result.CustomerAddresses.Count());

                var addresses = result.CustomerAddresses.Select(x => x.Address).OrderBy(x => x.Id).ToArray();
                Assert.Equal(TestData.AddressA.Id, addresses[0].Id);
                Assert.Equal(TestData.AddressB.Id, addresses[1].Id);
            }
        }
    }
}
