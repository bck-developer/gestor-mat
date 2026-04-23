using GestorMat.Application.DTOs.Material;
using GestorMat.Application.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestorMat.API.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class MaterialController(MaterialService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
        => Ok(await service.ObtenerTodosAsyncService());

    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        MaterialDto? material = await service.ObtenerPorIdAsyncService(id);

        if (material == null)
            return NotFound();

        return Ok(material);
    }

    [HttpPost]
    public async Task<IActionResult> Crear(CrearMaterialDto dto)
    {
        if (dto.Id_UnidadMedida <= 0)
            return BadRequest("Unidad de medida inválida");

        await service.CrearAsyncService(dto);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Actualizar(int id, ActualizarMaterialDto dto)
    {
        await service.ActualizarAsyncService(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await service.EliminarAsyncService(id);
        return NoContent();
    }
}
