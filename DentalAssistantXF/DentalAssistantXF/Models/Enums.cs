
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
        Root_canal,
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
}
