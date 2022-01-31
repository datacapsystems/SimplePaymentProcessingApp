using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePaymentProcessingApp.General
{
    /// <summary>
    /// Helper enum that represents each possible card brand for a credit transaction.
    /// </summary>
    public enum CardBrand
    {
        Visa,
        MasterCard,
        Discover,
        Unknown
    }
}
