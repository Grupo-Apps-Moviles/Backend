using Backend_Frock.Stops.Domain.Model.Queries.Geographic;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;

namespace Backend_Frock.Stops.Domain.Services.Geographic
{
    public interface IProvinceQueryService
    {
        Task<IEnumerable<Province>> Handle(GetAllProvincesQuery query);

        /// <summary>
        ///     Handle the GetProvincesByFkIdRegionQuery.
        /// </summary>
        /// <remarks>
        ///     This method handles the GetProvincesByFkIdRegionQuery. It returns all provinces for the given region ID.
        /// </remarks>
        /// <param name="query">The GetProvincesByFkIdRegionQuery query</param>
        /// <returns>An IEnumerable containing the Province objects for the specified region</returns>
        Task<IEnumerable<Province>> Handle(GetProvincesByFkIdRegionQuery query);

        Task<Province?> Handle(GetProvinceByIdQuery query);
    }
}
