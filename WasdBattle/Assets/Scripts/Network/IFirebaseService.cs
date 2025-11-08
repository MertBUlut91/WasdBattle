using System;
using System.Threading.Tasks;
using WasdBattle.Data;

namespace WasdBattle.Network
{
    /// <summary>
    /// Firebase servis interface'i. Firebase SDK kurulduktan sonra implement edilecek.
    /// </summary>
    public interface IFirebaseService
    {
        bool IsInitialized { get; }
        bool IsAuthenticated { get; }
        string CurrentUserId { get; }
        
        Task<bool> InitializeAsync();
        Task<bool> SignInAnonymouslyAsync();
        Task<bool> SignInWithEmailAsync(string email, string password);
        Task<bool> SignUpWithEmailAsync(string email, string password);
        Task SignOutAsync();
        
        Task<PlayerData> LoadPlayerDataAsync(string userId);
        Task<bool> SavePlayerDataAsync(PlayerData data);
        Task<bool> UpdatePlayerStatsAsync(string userId, int deltaElo, int deltaXp, bool won);
    }
}

