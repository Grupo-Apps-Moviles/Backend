using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Model.Commands.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;
using Backend_Frock.Stops.Domain.Services.Geographic;


namespace Backend_Frock.Stops.Application.Internal.CommandServices.Geographic
{
    public class DistrictCommandService(IDistrictRepository districtRepository, IUnitOfWork unitOfWork) : IDistrictCommandService
    {
        public async Task<District?> Handle(CreateDistrictCommand command)
        {
            var existingDistrict =
                await districtRepository.FindByIdIntAsync(command.Id);
            if (existingDistrict != null)
            {
                throw new Exception($"District already exists with that Id.");
            }
            var newDistrict = new District(command);
            try
            {
                await districtRepository.AddAsync(newDistrict);
                await unitOfWork.CompleteAsync();
                return newDistrict;
            }
            catch (Exception e)
            {
                // logger?.LogError(e, "Error creating region with name {RegionName} for locality {LocalityId}.", command.Name, command.FkIdLocality);
                return null; // Signal failure to the controller
            }
        }
    }
}
