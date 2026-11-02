using System.ComponentModel.DataAnnotations;

namespace ADONET_Practico.Models;

public sealed class Producto
{
    public int Id { get; set; }

    [Required, StringLength(120, MinimumLength = 2)]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Descripcion { get; set; }

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Precio { get; set; }

    [Range(0, 1000000)]
    public int Existencias { get; set; }
}
