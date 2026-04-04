using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace WeChooz.TechAssessment.Infrastructure.Data;

public sealed class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
    public async Task<DbConnection> OpenConnectionAsync(CancellationToken ct)
    {
        var conn = new SqlConnection(connectionString);
        await conn.OpenAsync(ct);
        return conn;
    }
}
