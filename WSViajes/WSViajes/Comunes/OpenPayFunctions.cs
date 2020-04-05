using System;
using Openpay;
using Openpay.Entities;
using Openpay.Entities.Request;
using System.Collections.Generic;
using System.Configuration;
using WSViajes.Models.Request;


namespace WSViajes.Comunes
{
    public class OpenPayFunctions
    {
        OpenpayAPI openpayAPI;

        public OpenPayFunctions()
        {
            bool OPNPY_PROD = Convert.ToBoolean(ConfigurationManager.AppSettings["OPNPY_PROD"]);
            openpayAPI = new OpenpayAPI(ConfigurationManager.AppSettings["OPNPY_KEY"], ConfigurationManager.AppSettings["OPNPY_MERCHANT"], OPNPY_PROD);
        }

        public Customer CreateCustomer(string pNombre, string pApellido, string pCorreo)
        {
            try
            {
                Customer customer = new Customer();
                customer.Name = pNombre;
                customer.LastName = pApellido;
                customer.Email = pCorreo;
                /*customer.Address = new Address();
                customer.Address.Line1 = "line 1";
                customer.Address.PostalCode = "12355";
                customer.Address.City = "Queretaro";
                customer.Address.CountryCode = "MX";
                customer.Address.State = "Queretaro";*/

                return openpayAPI.CustomerService.Create(customer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteCustomer(string pCustomerId)
        {
            try
            {
                openpayAPI.CustomerService.Delete(pCustomerId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Charge CreateCharge(string pIdCustomer, string pTokenId, string pDescripcion, decimal pMonto, string pSessionID )
        {
            string customer_id = pIdCustomer; //"awzhvjquygrvcinbzvyc";

            ChargeRequest request = new ChargeRequest
            {
                Method = "card",
                SourceId = pTokenId,//"ksqtpbpcgqxjm6ows4lp";
                Description = pDescripcion, // "Testing from .Net";
                Amount = pMonto,//new Decimal(9.99);
                Currency = "MXN",
                DeviceSessionId = pSessionID //"kR1MiQhz2otdIuUlQkbEyitIqVMiI16f";
            };


            return openpayAPI.ChargeService.Create(customer_id, request);
        }

        public Card CreateCard(CreaTarjetaOpenPayRequest pRequest, string pCustomerId)
        {
            //string customer_id = "awzhvjquygrvcinbzvyc";

            Card request = new Card
            {
                HolderName = pRequest.HolderName.Trim(),
                CardNumber = pRequest.CardNumber.Trim(),
                Cvv2 = pRequest.Cvv2.Trim(),
                ExpirationMonth = pRequest.ExpirationMonth.Trim(),
                ExpirationYear = pRequest.ExpirationYear.Trim()
            };
            
            request.DeviceSessionId = pRequest.DeviceSessionId;
            //Address address = new Address();
            //address.City = "Queretaro";
            //address.CountryCode = "MX";
            //address.State = "Queretaro";
            //address.PostalCode = "79125";
            //address.Line1 = "Av. Pie de la cuesta #12";
            //address.Line2 = "Desarrollo San Pablo";
            //address.Line3 = "Qro. Qro.";
            //request.Address = address;

            return openpayAPI.CardService.Create(pCustomerId, request);
        }

        public List<Card> GetListCardCustomers(string pCustomerId, int pSearchOffset = 0, int pSearchLimit = 100)
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

        public void DeleteCustomerCard(string pCustomerId, string pCardId)
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


