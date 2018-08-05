using SQLite;
using System;

namespace DentalAssistantXF.Models
{
    public class FinTrade
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PatientId { get; set; }

        public DateTime TradeDate { get; set; }

        public FinTradeType TradeType { get; set; }

        public decimal Ammount { get; set; }

        [Ignore]
        public decimal AmmountForSum
        {
            get { return TradeType == FinTradeType.charge ? Ammount : Ammount * (-1); }
        }

        [Ignore]
        public string Color
        {
            get { return TradeType == FinTradeType.charge ? "red" : "green"; }
        }
    }
}
