using MySql.Data.MySqlClient;

namespace ADONET_Practico.Data;

public static class DatabaseInitializer
{
    public static void Initialize(string connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);
        if (string.IsNullOrWhiteSpace(builder.Database))
            throw new InvalidOperationException("La cadena de conexión debe indicar Database.");

        using var appConnection = new MySqlConnection(connectionString);
        using var createTable = new MySqlCommand("""
            CREATE TABLE IF NOT EXISTS Productos (
                Id INT NOT NULL AUTO_INCREMENT,
                Nombre VARCHAR(120) NOT NULL,
                Descripcion VARCHAR(500) NULL,
                Precio DECIMAL(12,2) NOT NULL,
                Existencias INT NOT NULL DEFAULT 0,
                PRIMARY KEY (Id),
                CONSTRAINT CK_Productos_Precio CHECK (Precio > 0),
                CONSTRAINT CK_Productos_Existencias CHECK (Existencias >= 0)
            ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
            """, appConnection);
        appConnection.Open();
        createTable.ExecuteNonQuery();
    }
}
