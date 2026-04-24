using Mapster;
using GestorMat.Application.DTOs;
using GestorMat.Application.DTOs.Material;
using GestorMat.Application.DTOs.MovimientoMaterial;
using GestorMat.Domain.Entidades;

namespace GestorMat.Application.Configuracion;

/// <summary>
/// Configuración centralizada de mapeos entre entidades de dominio y DTOs.
/// Mapster proporciona mapeos type-safe y de alto rendimiento.
/// </summary>
public static class MappingConfig
{
    public static void RegisterMappings()
    {
        // Material mappings - Mapeo explícito de la propiedad anidada UnidadMedida.Nombre
        TypeAdapterConfig<Material, MaterialDto>
            .NewConfig()
            .Map(dest => dest.UnidadMedida, src => src.UnidadMedida != null ? src.UnidadMedida.Nombre : string.Empty)
            .Compile();

        // Usuario mappings - Mapeo explícito de la propiedad anidada Rol.RolName
        TypeAdapterConfig<Usuario, UsuarioDto>
            .NewConfig()
            .Map(dest => dest.Rol, src => src.Rol != null ? src.Rol.RolName : string.Empty)
            .Compile();

        // UnidadMedida mappings - Mapeo directo, todas las propiedades coinciden
        TypeAdapterConfig<UnidadMedida, UnidadMedidaDto>
            .NewConfig()
            .Compile();

        // Deposito mappings - Mapeo directo, todas las propiedades coinciden
        TypeAdapterConfig<Deposito, DepositoDto>
            .NewConfig()
            .Compile();

        // Rol mappings - Mapeo de Rol.Id_Rol a RolDto.IdRol
        TypeAdapterConfig<Rol, RolDto>
            .NewConfig()
            .Map(dest => dest.IdRol, src => src.Id_Rol)
            .Compile();

        // MovimientoMaterial mappings - Mapeo directo sin propiedades de navegación
        TypeAdapterConfig<MovimientoMaterial, MovimientoMaterialDto>
            .NewConfig()
            .Compile();
    }
}

