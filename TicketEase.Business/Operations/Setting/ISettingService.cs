using System.Threading.Tasks;
using TicketEase.Business.Types;

namespace TicketEase.Business.Operations.Setting
{
    public interface ISettingService
    {
        /// <summary>
        /// Sistem bakım modunu açar/kapatır.
        /// Başarısız durumlarda NotFoundException veya InternalServerException fırlatır.
       
        Task<ServiceMessage> ToggleMaintenance();

        /// <summary>
        /// Sistemin bakım modunda olup olmadığını döner.
        /// Eğer setting bulunamazsa NotFoundException fırlatır.
      
        Task<bool> IsMaintenanceMode();
    }
}
