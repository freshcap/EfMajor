using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EfMajor.Test
{
    [Table("Customer")]
    public class Customer
    {
        public Customer()
        {
            CustomerAddresses = new List<CustomerAddress>();
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<CustomerAddress> CustomerAddresses { get; set; }
    }

    [Table("CustomerAddress")]
    public class CustomerAddress
    {
        public string CustomerId { get; set; }

        public string AddressId { get; set; }

        public Customer Customer { get; set; }

        public Address Address { get; set; }
    }

    [Table("Address")]
    public class Address
    {
        public string Id { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string PostalCode { get; set; }
    }
}
