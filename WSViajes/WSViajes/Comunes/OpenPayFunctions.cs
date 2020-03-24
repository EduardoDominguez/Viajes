using System;
using Openpay;
using Openpay.Entities;
using Openpay.Utils;
using Openpay.Entities.Request;
using System.Collections.Generic;

namespace WSViajes.Comunes
{
    public class OpenPayFunctions
    {
        OpenpayAPI openpayAPI;

        public OpenPayFunctions()
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

        public void createCharge()
        {
            string customer_id = "awzhvjquygrvcinbzvyc";

            ChargeRequest request = new ChargeRequest();
            request.Method = "card";
            request.SourceId = "ksqtpbpcgqxjm6ows4lp";
            request.Description = "Testing from .Net";
            request.Amount = new Decimal(9.99);
            request.Currency = "MXN";
            request.DeviceSessionId = "kR1MiQhz2otdIuUlQkbEyitIqVMiI16f";


            Charge charge = openpayAPI.ChargeService.Create(customer_id, request);
        }

        public void createCard()
        {
            string customer_id = "awzhvjquygrvcinbzvyc";

            Card request = new Card();
            request.HolderName = "Juan Perez Ramirez";
            request.CardNumber = "4111111111111111";
            request.Cvv2 = "110";
            request.ExpirationMonth = "12";
            request.ExpirationYear = "20";
            request.DeviceSessionId = "kR1MiQhz2otdIuUlQkbEyitIqVMiI16f";
            Address address = new Address();
            address.City = "Queretaro";
            address.CountryCode = "MX";
            address.State = "Queretaro";
            address.PostalCode = "79125";
            address.Line1 = "Av. Pie de la cuesta #12";
            address.Line2 = "Desarrollo San Pablo";
            address.Line3 = "Qro. Qro.";
            request.Address = address;

            request = openpayAPI.CardService.Create(customer_id, request);
        }

        public List<Card> getListCardCustomers(string pCustomerId, int pSearchOffset = 0, int pSearchLimit = 100)
        {
            try
            {
                SearchParams request = new SearchParams();
                //request.CreationGte = new Datetime(2014, 5, 1);
                //request.CreationLte = new DateTime(2014, 5, 15);
                request.Offset = pSearchOffset;
                request.Limit = pSearchLimit;

                List<Card> cards = openpayAPI.CardService.List(pCustomerId, request);

                return cards;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        public void deleteCustomerCard(string pCustomerId, string pCardId)
        {
            try
            {
                openpayAPI.CardService.Delete(pCustomerId, pCardId);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}


