using AuthorizeNet.Api.Contracts.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReFreshMVC.Models.Interfaces
{
    public interface IAuthorizeNetManager
    {
        createTransactionResponse RunCard(int amount, string expDate, string number);
    }
}
