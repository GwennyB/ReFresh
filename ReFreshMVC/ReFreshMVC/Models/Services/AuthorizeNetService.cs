using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ReFreshMVC.Models.Interfaces;

namespace ReFreshMVC.Models.Services
{
    public class AuthorizeNetService : IAuthorizeNetManager
    {
        private IConfiguration _configuration;

        public AuthorizeNetService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public createTransactionResponse RunCard(int orderAmount, string expDate, string number)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;



            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = _configuration.GetConnectionString("AuthorizeNet:ClientId"),
                ItemElementName = ItemChoiceType.transactionKey,
                Item = _configuration.GetConnectionString("Authorize:TransactionKey"),
            };

            var creditCard = new creditCardType
            {
                cardNumber = number,
                expirationDate = expDate

            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            // Set Max for Payment
            if (orderAmount > 4000)
                orderAmount = 4000;

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),   // charge the card
                amount = orderAmount,
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            return response;

        }
    }
}
