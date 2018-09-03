
namespace DentalAssistantXF.Models
{
    public enum ToastMessageType
    {
        Success,
        Error
    }

    public enum DentalProcedureType
    {
        Cleaning,
        Whitening,
        Filling,
        RootCanal,
        Cap,
        Bridge,
        Denture,
        Implant,
        Extraction
    }

    public enum DentalProcedureStatus
    {
        InProgress,
        Completed
    }

    public enum FinTradeType : int
    {
        charge = 0,
        payment = 1
    }
}
