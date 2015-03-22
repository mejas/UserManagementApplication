
namespace UserManagementApplication.Data.Providers.Interfaces
{
    public interface IDataSecurityProvider
    {
        string GenerateHash(string input, string salt);
        string GenerateSalt();
    }
}
