using System.Data;
using System.Data.Common;

namespace WeChooz.TechAssessment.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<DbConnection> OpenConnectionAsync(CancellationToken ct);
}
