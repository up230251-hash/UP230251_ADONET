using ADONET_Practico.Models;
using MySql.Data.MySqlClient;

namespace ADONET_Practico.Repositories;

public sealed class ProductoRepository(string connectionString) : IProductoRepository
{
    private const string SelectColumns = "Id, Nombre, Descripcion, Precio, Existencias";

    public List<Producto> ObtenerTodos()
    {
        var productos = new List<Producto>();
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand($"SELECT {SelectColumns} FROM Productos ORDER BY Id", connection);
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read()) productos.Add(LeerProducto(reader));
        return productos;
    }

    public Producto? ObtenerPorId(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand($"SELECT {SelectColumns} FROM Productos WHERE Id = @Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();
        using var reader = command.ExecuteReader();
        return reader.Read() ? LeerProducto(reader) : null;
    }

    public int Insertar(Producto producto)
    {
        const string sql = "INSERT INTO Productos (Nombre, Descripcion, Precio, Existencias) " +
                           "VALUES (@Nombre, @Descripcion, @Precio, @Existencias)";
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand(sql, connection);
        AgregarDatos(command, producto);
        connection.Open();
        command.ExecuteNonQuery();
        return checked((int)command.LastInsertedId);
    }

    public bool Actualizar(Producto producto)
    {
        const string sql = "UPDATE Productos SET Nombre=@Nombre, Descripcion=@Descripcion, " +
                           "Precio=@Precio, Existencias=@Existencias WHERE Id=@Id";
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand(sql, connection);
        AgregarDatos(command, producto);
        command.Parameters.AddWithValue("@Id", producto.Id);
        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    public bool Eliminar(int id)
    {
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand("DELETE FROM Productos WHERE Id=@Id", connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    public List<Producto> Buscar(string termino)
    {
        var productos = new List<Producto>();
        using var connection = new MySqlConnection(connectionString);
        using var command = new MySqlCommand($"SELECT {SelectColumns} FROM Productos " +
                                           "WHERE Nombre LIKE @Termino OR Descripcion LIKE @Termino ORDER BY Nombre", connection);
        command.Parameters.AddWithValue("@Termino", $"%{termino}%");
        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read()) productos.Add(LeerProducto(reader));
        return productos;
    }

    private static void AgregarDatos(MySqlCommand command, Producto producto)
    {
        command.Parameters.AddWithValue("@Nombre", producto.Nombre);
        command.Parameters.AddWithValue("@Descripcion", (object?)producto.Descripcion ?? DBNull.Value);
        command.Parameters.AddWithValue("@Precio", producto.Precio);
        command.Parameters.AddWithValue("@Existencias", producto.Existencias);
    }

    private static Producto LeerProducto(MySqlDataReader reader) => new()
    {
        Id = reader.GetInt32(0),
        Nombre = reader.GetString(1),
        Descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
        Precio = reader.GetDecimal(3),
        Existencias = reader.GetInt32(4)
    };
}
