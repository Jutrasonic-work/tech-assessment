using System.Data;
using System.Data.Common;

namespace WeChooz.TechAssessment.Infrastructure.Data;

/// <summary>
/// Factory d'IDbConnection pour l'accès à la base de données. Permet d'abstraire la création et l'ouverture des connexions.
/// </summary>
public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
    Task<DbConnection> OpenConnectionAsync(CancellationToken ct);
}
