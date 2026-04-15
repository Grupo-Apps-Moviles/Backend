using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Model.Queries.Geographic;

namespace Backend_Frock.Stops.Domain.Services.Geographic
{
    /// <summary>
    /// Servicio para consultas relacionadas con la estructura jerárquica geográfica.
    /// </summary>
    public interface ILocationQueryService
    {
        /// <summary>
        /// Obtiene todas las regiones.
        /// </summary>
        Task<IEnumerable<Region>> Handle(GetAllRegionsQuery query);

        /// <summary>
        /// Obtiene todas las provincias por región.
        /// </summary>
        Task<IEnumerable<Province>> Handle(GetProvincesByFkIdRegionQuery query);

        /// <summary>
        /// Obtiene todos los distritos por provincia.
        /// </summary>
        Task<IEnumerable<District>> Handle(GetDistrictsByFkIdProvinceQuery query);


        //ESTO DESPUES
        /// <summary>
        /// Obtiene la jerarquía geográfica completa.
        /// </summary>
        //Task<LocationHierarchyDto> Handle(GetLocationHierarchyQuery query);

        /// <summary>
        /// Obtiene la jerarquía geográfica para una localidad específica.
        /// </summary>
        //Task<LocalityHierarchyDto> Handle(GetLocationHierarchyByLocalityIdQuery query);
    }
}
