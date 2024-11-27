    using Microsoft.Data.SqlClient;

    using System.Data;
    using System.Reflection;

    namespace Clinica.Common
    {
        public class StoredProcedureHelper
        {
            private readonly string _connectionString;
            private readonly ILogger<StoredProcedureHelper> _logger;

            public StoredProcedureHelper(IConfiguration configuration, ILogger<StoredProcedureHelper> logger)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
                _logger = logger;
            }

            // Ejecutar un procedimiento almacenado que retorna una lista de objetos
            public async Task<List<T>> ExecList<T>(string storedProcedureName, List<SqlParameter> parameters = null) where T : new()
            {
                var results = new List<T>();

                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            command.Parameters.AddRange(parameters.ToArray());

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                results.Add(MapTo<T>(reader)); // Mapeo automático
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(storedProcedureName, ex);
                    throw;
                }

                return results;
            }

            // Ejecutar un procedimiento almacenado que retorna una fila mapeada
            public async Task<T> ExecSingleRow<T>(string storedProcedureName, List<SqlParameter> parameters = null) where T : new()
            {
                T result = default;

                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            command.Parameters.AddRange(parameters.ToArray());

                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                result = MapTo<T>(reader); // Mapeo automático
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(storedProcedureName, ex);
                    throw;
                }

                return result;
            }

            // Ejecutar un procedimiento almacenado que retorna un único valor
            public async Task<T> ExecScalar<T>(string storedProcedureName, List<SqlParameter> parameters = null)
            {
                T result = default;

                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            command.Parameters.AddRange(parameters.ToArray());

                        await connection.OpenAsync();

                        result = (T)await command.ExecuteScalarAsync();
                    }
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(storedProcedureName, ex);
                    throw;
                }

                return result;
            }

            // Ejecutar un procedimiento almacenado sin retorno
            public async Task ExecNonQuery(string storedProcedureName, List<SqlParameter> parameters = null)
            {
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    using (var command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        if (parameters != null)
                            command.Parameters.AddRange(parameters.ToArray());

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                }
                catch (Exception ex)
                {
                    await HandleErrorAsync(storedProcedureName, ex);
                    throw;
                }
            }

            // Método auxiliar para mapear un SqlDataReader a un objeto basado en nombres de columnas
            private static T MapTo<T>(SqlDataReader reader) where T : new()
            {
                var obj = new T();
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (!reader.HasColumn(property.Name))
                        continue;

                    var value = reader[property.Name];
                    if (value == DBNull.Value)
                    {
                        if (property.PropertyType.IsValueType && Nullable.GetUnderlyingType(property.PropertyType) == null)
                        {
                            property.SetValue(obj, Activator.CreateInstance(property.PropertyType));
                        }
                        continue;
                    }

                    property.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
                }

                return obj;
            }

            // Método auxiliar para manejar errores
            private async Task HandleErrorAsync(string storedProcedureName, Exception ex)
            {
                _logger.LogError(ex, "Error executing stored procedure {StoredProcedureName}", storedProcedureName);
            }
        }

        public static class SqlDataReaderExtensions
        {
            public static bool HasColumn(this SqlDataReader reader, string columnName)
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
