using System;
using System.Collections.Generic;
using System.Text;

namespace DentalAssistantXF.Models
{
    public class FinTradeDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string FullName => LastName + " " + FirstName;

        public FinTradeType TradeType { get; set; }

        public decimal Ammount { get; set; }

        public decimal AmmountForSum
        {
            get { return TradeType == FinTradeType.charge ? Ammount : Ammount * (-1); }
        }
    }
}
