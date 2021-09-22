using System;
using System.Collections.Generic;
using FDEV.Rules.Demo.Domain.Identity;
using FDEV.Rules.Demo.Domain.Identity.Details;
using FDEV.Rules.Demo.Domain.Shopping;

namespace FDEV.Rules.Demo.Domain.Rules.Contexts
{
    public static class ShoppingData
    {
        public static Customer FullCustomer() => new Customer()
        {
            Name = "SirBuysAlot",
            FirstName = "Test",
            LastName = "Customer",
            Addresses = new Addresses(new Address(AddressType.Civic, "Home", "Shipping and invoice", "LongRoad", "", "123", "4", "Copenhagen", "2300", "", "Denmark")),
            Phones = new Phones(),
            Emails = new Emails(),
            BirthDate = new DateTime(1975, 04, 15)
        };

        public static IEnumerable<Employee> SimpleEmployees(int numberOfEmployees)
        {
            for (int i = 1; i < numberOfEmployees; i++)
            {
                yield return new Employee
                {
                    FirstName = "Carsten",
                    LastName = "Feltman",
                    BirthDate = new DateTime(1975, 4, 15)
                };
            }
        }

        public static IEnumerable<Customer> SimpleCustomers(int numberOfCustomers)
        {
            for (int i = 1; i < numberOfCustomers; i++)
            {
                yield return new Customer
                {
                    FirstName = "Eliane",
                    LastName = "Beth",
                    BirthDate = new DateTime(1956, 5, 30)
                };
            }
        }

        public static class Products
        {
            public static Product ProductA => new Product("ProductA", "C", 50);
            public static Product ProductB => new Product("ProductB", "B", 30);
            public static Product ProductC => new Product("ProductC", "C", 20);
            public static Product ProductD => new Product("ProductD", "D", 15);
            public static Product Customized(string name, string sku, int price) => new Product(name, sku, price);
        };

        public static class Promotions
        {
            public static Promotion PromoA3 => new Promotion("PromoA3", "A3", 50);
            public static Promotion PromoB2 => new Promotion("PromoB2", "B2", 30);
            public static Promotion PromoC1D1 => new Promotion("PromoC1D1", "C1D1", 20);
            public static Promotion CustomPromo(string name, string sku, int price) => new Promotion(name, sku, price);
        };
    }
}