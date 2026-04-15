namespace Backend_Frock.Stops.Interfaces.REST.Resources.Geographic
{
    public record DistrictResource(
        int Id,
        string Name,
        int FkIdProvince // Foreign key to the Province entity (e.g., "prov-1" for the province this district belongs to)
        );
}
