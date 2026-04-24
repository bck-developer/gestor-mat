using System.Runtime.CompilerServices;
using GestorMat.Application.Configuracion;

namespace GestorMat.Tests;

/// <summary>
/// Inicializador del módulo de pruebas.
/// Se ejecuta automáticamente antes de cualquier prueba.
/// </summary>
[CompilerGenerated]
internal static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // Registrar mapeos de Mapster antes de ejecutar cualquier prueba
        MappingConfig.RegisterMappings();
    }
}
