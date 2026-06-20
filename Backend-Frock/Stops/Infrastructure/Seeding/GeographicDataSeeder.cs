// Infrastructure/Seeding/GeographicDataSeeder.cs
using Backend_Frock.Shared.Domain.Repositories;
using Backend_Frock.Stops.Domain.Model.Aggregates.Geographic;
using Backend_Frock.Stops.Domain.Model.Commands.Geographic;
using Backend_Frock.Stops.Domain.Repositories.Geographic;
using Backend_Frock.Stops.Domain.Services;
using Backend_Frock.Stops.Domain.Model.DTOs;
using Microsoft.Extensions.Logging;

namespace Backend_Frock.Stops.Infrastructure.Seeding
{
    /// <summary>
    /// Resultado de un intento de siembra geográfica.
    /// </summary>
    public record GeoSeedResult(bool AlreadySeeded, int Regions, int Provinces, int Districts)
    {
        public bool SeededNow => !AlreadySeeded;
    }

    public class GeographicDataSeeder
    {
        private readonly IGeoImportService _geoImportService;
        private readonly IRegionRepository _regionRepository;
        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GeographicDataSeeder> _logger;

        public GeographicDataSeeder(
            IGeoImportService geoImportService,
            IRegionRepository regionRepository,
            IProvinceRepository provinceRepository,
            IDistrictRepository districtRepository,
            IUnitOfWork unitOfWork,
            ILogger<GeographicDataSeeder> logger)
        {
            _geoImportService = geoImportService;
            _regionRepository = regionRepository;
            _provinceRepository = provinceRepository;
            _districtRepository = districtRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// Importa regiones, provincias y distritos desde la API externa.
        /// Es idempotente: si ya hay regiones cargadas no hace nada (no duplica).
        /// No traga excepciones: si la importación falla, propaga el error para que el
        /// llamador (arranque o endpoint) lo loguee y/o lo reporte.
        /// </summary>
        public async Task<GeoSeedResult> SeedDataAsync()
        {
            _logger.LogInformation("Iniciando la carga de datos geográficos...");

            // Idempotencia: si ya hay regiones, no reimportamos.
            if ((await _regionRepository.ListAsync()).Any())
            {
                _logger.LogInformation("Los datos geográficos ya están cargados. Se omite la importación.");
                return new GeoSeedResult(AlreadySeeded: true, Regions: 0, Provinces: 0, Districts: 0);
            }

            // 1) Traer todo desde la API (propaga si falla tras los reintentos).
            var raw = (await _geoImportService.GetGeoFromApi()).ToList();

            if (raw.Count == 0)
            {
                // No declaramos éxito falso: dejamos constancia y abortamos.
                _logger.LogError("La API geográfica no devolvió datos; no hay nada que sembrar.");
                throw new InvalidOperationException("La API geográfica no devolvió datos para sembrar.");
            }

            // 2) Regiones: agrupamos la lista plana de distritos por los 2 primeros dígitos del CODIGO.
            var regiones = raw
                .Select(x => new { Id = int.Parse(x.CODIGO.Substring(0, 2)), Name = x.NOMBDEP! })
                .DistinctBy(r => r.Id)
                .ToList();

            foreach (var r in regiones)
                await _regionRepository.AddAsync(new Region(new CreateRegionCommand(r.Id, r.Name)));

            // 3) Provincias: 4 primeros dígitos; la región es los 2 primeros.
            var provincias = raw
                .Select(x => new
                {
                    Id = int.Parse(x.CODIGO.Substring(0, 4)),
                    Name = x.NOMBPROV!,
                    RegionId = int.Parse(x.CODIGO.Substring(0, 2))
                })
                .DistinctBy(p => p.Id)
                .ToList();

            foreach (var p in provincias)
                await _provinceRepository.AddAsync(new Province(new CreateProvinceCommand(p.Id, p.Name, p.RegionId)));

            // 4) Distritos: código completo; la provincia es los 4 primeros dígitos.
            var distritos = raw
                .Select(x => new
                {
                    Id = int.Parse(x.CODIGO),
                    Name = x.NOMBDIST!,
                    ProvinceId = int.Parse(x.CODIGO.Substring(0, 4))
                })
                .DistinctBy(d => d.Id)
                .ToList();

            foreach (var d in distritos)
                await _districtRepository.AddAsync(new District(new CreateDistrictCommand(d.Id, d.Name, d.ProvinceId)));

            // 5) Una sola transacción para toda la jerarquía. Si algo falla, propaga (no se traga).
            await _unitOfWork.CompleteAsync();

            // 6) Verificación: no declarar éxito si quedó vacío.
            var persistedRegions = (await _regionRepository.ListAsync()).Count();
            if (persistedRegions == 0)
            {
                _logger.LogError("La siembra geográfica terminó sin persistir regiones.");
                throw new InvalidOperationException("La siembra geográfica no persistió ningún registro.");
            }

            _logger.LogInformation(
                "Carga de datos geográficos completada: {Regions} regiones, {Provinces} provincias, {Districts} distritos.",
                regiones.Count, provincias.Count, distritos.Count);

            return new GeoSeedResult(
                AlreadySeeded: false,
                Regions: regiones.Count,
                Provinces: provincias.Count,
                Districts: distritos.Count);
        }
    }
}
