using Entity.Dtos.ModelDtos.Auth.DataAuth;
using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;

namespace LogicDomain.SystemServices
{
    public class LdapService
    {
        private readonly IConfiguration _config;
        public LdapService(IConfiguration config) => _config = config;

        public async Task<bool> Authenticate(string username, string password) // Added async Task
        {
            var ldapHost = "upmdc04";
            var domain = "UPM.COM.MX";
            var fullUsername = $"{username}@{domain}";

            try
            {
                using (var connection = new LdapConnection())
                {
                    // Must await these!
                    await connection.ConnectAsync(ldapHost, LdapConnection.DefaultPort);
                    await connection.BindAsync(fullUsername, password);
                    return connection.Bound;
                }
            }
            catch (LdapException)
            {
                return false;
            }
        }

        public async Task<LdapUserData> AuthenticateAndGetDetails(string username, string password)
        {
            var ldapHost = "upmdc04";
            var domain = "UPM.COM.MX";
            var fullUsername = $"{username}@{domain}";

            try
            {
                using (var connection = new LdapConnection())
                {
                    // 1. Conectar y Autenticar
                    await connection.ConnectAsync(ldapHost, LdapConnection.DefaultPort);
                    await connection.BindAsync(fullUsername, password);

                    var searchFilter = $"(&(objectClass=user)(sAMAccountName={username}))";

                    // Configurar para que siga referencias
                    var opciones = new LdapSearchConstraints { ReferralFollowing = true };

                    var results = await connection.SearchAsync(
                    "DC=UPM,DC=COM,DC=MX", // Ajustado al nombre del dominio
                    LdapConnection.ScopeSub,
                    searchFilter,
                    null,
                    false,
                    opciones
                    );

                    if (await results.HasMoreAsync())
                    {
                        // Obtener el LdapEntry
                        var entry = await results.NextAsync();

                        return new LdapUserData
                        {
                            // Use available helper to get string values safely
                            DisplayName = entry.GetStringValueOrDefault("displayName", username),
                            Email = entry.GetStringValueOrDefault("mail", $"{username}@unipres.com"),
                            Department = entry.GetStringValueOrDefault("department")
                        };
                    }
                    return null;
                }
            }
            catch (LdapException ex)
            {
                // Es mejor relanzar solo 'ex' o manejar el error
                throw;
            }
        }
    }
}
