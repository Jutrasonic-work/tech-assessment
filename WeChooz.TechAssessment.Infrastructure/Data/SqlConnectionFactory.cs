using System.Data;
using Microsoft.Data.SqlClient;
using WeChooz.TechAssessment.Application.Abstractions.Data;

namespace WeChooz.TechAssessment.Infrastructure.Data;

public sealed class SqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
