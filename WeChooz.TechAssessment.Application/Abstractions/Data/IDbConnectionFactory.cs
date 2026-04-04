using System.Data;

namespace WeChooz.TechAssessment.Application.Abstractions.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
