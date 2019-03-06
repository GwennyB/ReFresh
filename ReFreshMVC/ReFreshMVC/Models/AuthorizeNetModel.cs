using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models
{
    public class AuthorizeNetModel
    {
        public string ApiLoginID { get; set; }
        public string ApiTransactionKey { get; set; }

        public AuthorizeNetModel(string apiLoginID, string apiTransactionKey)
        {
            ApiLoginID = apiLoginID;
            ApiTransactionKey = apiTransactionKey;
        }
        public createTransactionResponse RunCard()
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var creditCard = new creditCardType
            {
                cardNumber = "4111111111111111",
                expirationDate = "0719"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = creditCard };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),   // charge the card
                amount = 133.45m,
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            // instantiate the contoller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            return response;

            //if (response.messages.resultCode == messageTypeEnum.Ok)
            //{
            //    if (response.transactionResponse != null)
            //    {
            //        Console.WriteLine("Success, Auth Code : " + response.transactionResponse.authCode);
            //        return response.transactionResponse.authCode;
            //    }
            //    return "false";
            //}
            //else
            //{
            //    if (response.transactionResponse != null)
            //    {
            //        return "Transaction Error : " + response.transactionResponse.errors[0].errorCode + " " + response.transactionResponse.errors[0].errorText;
            //    }
            //    return "Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text;
            //}

        }
    }
}
