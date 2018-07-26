using XFPrismDemo.LocalDBs;

namespace DentalAssistantXF.Services
{
    public interface IDatabaseService
    {
        DentalAssistantDB DentalAssistantDB { get; }
    }
}
