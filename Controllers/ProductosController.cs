using ADONET_Practico.Models;
using ADONET_Practico.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ADONET_Practico.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ProductosController(IProductoRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<List<Producto>>(StatusCodes.Status200OK)]
    public ActionResult<List<Producto>> ObtenerTodos() => Ok(repository.ObtenerTodos());

    [HttpGet("{id:int}")]
    [ProducesResponseType<Producto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Producto> ObtenerPorId(int id)
    {
        var producto = repository.ObtenerPorId(id);
        return producto is null ? NotFound() : Ok(producto);
    }

    [HttpGet("buscar")]
    [ProducesResponseType<List<Producto>>(StatusCodes.Status200OK)]
    public ActionResult<List<Producto>> Buscar([FromQuery] string termino)
    {
        if (string.IsNullOrWhiteSpace(termino)) return BadRequest(new { mensaje = "El parámetro termino es obligatorio." });
        return Ok(repository.Buscar(termino.Trim()));
    }

    [HttpPost]
    [ProducesResponseType<Producto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Producto> Insertar([FromBody] Producto producto)
    {
        producto.Id = repository.Insertar(producto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = producto.Id }, producto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Actualizar(int id, [FromBody] Producto producto)
    {
        if (id != producto.Id) return BadRequest(new { mensaje = "El id de la ruta debe coincidir con el id del producto." });
        return repository.Actualizar(producto) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Eliminar(int id) => repository.Eliminar(id) ? NoContent() : NotFound();
}
