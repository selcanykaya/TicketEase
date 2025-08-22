using System;
using System.Threading.Tasks;
using TicketEase.Business.Exceptions;
using TicketEase.Business.Types;
using TicketEase.Data.Entities;
using TicketEase.Data.Repositories;
using TicketEase.Data.UnitOfWork;

namespace TicketEase.Business.Operations.Setting
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _settingRepository;

        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> settingRepository)
        {
            _unitOfWork = unitOfWork;
            _settingRepository = settingRepository;
        }

        public async Task<ServiceMessage> ToggleMaintenance()
        {
            var setting = await _settingRepository.Get(x => x.Id == 1);
            if (setting == null)
                throw new NotFoundException("Setting not found.");

            try
            {
                setting.MaintenanceMode = !setting.MaintenanceMode;
                await _settingRepository.UpdateAsync(setting);
                await _unitOfWork.SaveChangesAsync();

                return new ServiceMessage
                {
                    Success = true,
                    Message = $"Maintenance mode is now {(setting.MaintenanceMode ? "ON" : "OFF")}."
                };
            }
            catch (Exception ex)
            {
                
                throw new InternalServerException("An error occurred while updating maintenance mode.", ex);
            }
        }

        public async Task<bool> IsMaintenanceMode()
        {
            var setting = await _settingRepository.Get(x => x.Id == 1);
            if (setting == null)
                throw new NotFoundException("Setting not found.");

            return setting.MaintenanceMode;
        }
    }
}
