using System;
using Openpay;
using Openpay.Entities;

namespace WSViajes.Comunes
{
    public class OpenPay
    {
        OpenpayAPI openpayAPI;

        public OpenPay()
        {
            openpayAPI = new OpenpayAPI("sk_a896b738bdc541a599e75c52df080bb4", "mes0ipoqxgvxnuyu4kpg", false);
        }

        public void createCustomer()
        {
            Customer customer = new Customer();
            customer.Name = "Net Client";
            customer.LastName = "C#";
            customer.Email = "net@c.com";
            customer.Address = new Address();
            customer.Address.Line1 = "line 1";
            customer.Address.PostalCode = "12355";
            customer.Address.City = "Queretaro";
            customer.Address.CountryCode = "MX";
            customer.Address.State = "Queretaro";

            Customer customerCreated = openpayAPI.CustomerService.Create(customer);
        }
    }
}


