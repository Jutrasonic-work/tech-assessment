using System.Data;
using System.Globalization;
using Dapper;
using WeChooz.TechAssessment.Domain.Courses;
using WeChooz.TechAssessment.Domain.Sessions;

namespace WeChooz.TechAssessment.Infrastructure.Data;

internal static class DapperTypeHandlers
{
    private static readonly object Gate = new();
    private static bool _registered;

    /// <summary>
    /// Enregistre les type handlers Dapper pour les enums domaine. Doit être appelé une seule fois au démarrage de l'application.
    /// </summary>
    public static void Register()
    {
        lock (Gate)
        {
            if (_registered)
            {
                return;
            }

            SqlMapper.AddTypeHandler(new EnumTypeHandler<CseAudience>());
            SqlMapper.AddTypeHandler(new EnumTypeHandler<SessionDeliveryMode>());
            _registered = true;
        }
    }

    /// <summary>
    /// Mappe les colonnes entières (tinyint, int, etc.) vers des enums domaine pour la matérialisation des records Dapper.
    /// </summary>
    private sealed class EnumTypeHandler<T> : SqlMapper.TypeHandler<T>
        where T : struct, Enum
    {
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            var underlying = Enum.GetUnderlyingType(typeof(T));
            parameter.Value = Convert.ChangeType(value, underlying, CultureInfo.InvariantCulture);
        }

        public override T Parse(object value)
        {
            var n = Convert.ToInt32(value, CultureInfo.InvariantCulture);
            return (T)Enum.ToObject(typeof(T), n);
        }
    }
}
