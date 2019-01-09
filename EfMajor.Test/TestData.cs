namespace EfMajor.Test
{
    public static class TestData
    {
        public static Customer Customer_Fred = new Customer { Id = "A", FirstName = "Fred", LastName = "Flintstone" };
        public static Customer Customer_Wilma = new Customer { Id = "B", FirstName = "Wilma", LastName = "Flintstone" };
        public static Customer Customer_Barney = new Customer { Id = "C", FirstName = "Barney", LastName = "Rubble" };

        public static Address AddressA = new Address { Id = "A", Address1 = "111 1st Street", State = "CA" };
        public static Address AddressB = new Address { Id = "B", Address1 = "222 2nd Street", State = "CA" };
        public static Address AddressC = new Address { Id = "C", Address1 = "333 3rd Street", State = "NY" };

        public static CustomerAddress CustomerAddress(string customerId, string addressId)
        {
            return new CustomerAddress { CustomerId = customerId, AddressId = addressId };
        }
    }
}
