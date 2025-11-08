using Unity.Netcode;
using UnityEngine;

namespace WasdBattle.Network
{
    /// <summary>
    /// Network işlemleri için yardımcı sınıf
    /// </summary>
    public static class NetworkHelper
    {
        /// <summary>
        /// Client ID'ye göre NetworkObject bulur
        /// </summary>
        public static NetworkObject GetPlayerObject(ulong clientId)
        {
            if (NetworkManager.Singleton == null)
                return null;
            
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                return client.PlayerObject;
            }
            
            return null;
        }
        
        /// <summary>
        /// Local player'ın client ID'sini döndürür
        /// </summary>
        public static ulong GetLocalClientId()
        {
            if (NetworkManager.Singleton == null)
                return 0;
            
            return NetworkManager.Singleton.LocalClientId;
        }
        
        /// <summary>
        /// Bu client server mı kontrol eder
        /// </summary>
        public static bool IsServer()
        {
            return NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer;
        }
        
        /// <summary>
        /// Bu client host mu kontrol eder
        /// </summary>
        public static bool IsHost()
        {
            return NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost;
        }
        
        /// <summary>
        /// Bağlı client sayısını döndürür
        /// </summary>
        public static int GetConnectedClientCount()
        {
            if (NetworkManager.Singleton == null)
                return 0;
            
            return NetworkManager.Singleton.ConnectedClients.Count;
        }
        
        /// <summary>
        /// Network latency'sini döndürür (ms)
        /// </summary>
        public static float GetNetworkLatency()
        {
            if (NetworkManager.Singleton == null)
                return 0f;
            
            // RTT (Round Trip Time) / 2 = One-way latency
            return NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetCurrentRtt(GetLocalClientId()) / 2f;
        }
        
        /// <summary>
        /// Network bağlantısı var mı kontrol eder
        /// </summary>
        public static bool IsConnected()
        {
            return NetworkManager.Singleton != null && 
                   (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient);
        }
        
        /// <summary>
        /// Debug bilgisi döndürür
        /// </summary>
        public static string GetNetworkDebugInfo()
        {
            if (NetworkManager.Singleton == null)
                return "NetworkManager not initialized";
            
            string info = $"Role: {GetNetworkRole()}\n";
            info += $"Client ID: {GetLocalClientId()}\n";
            info += $"Connected Clients: {GetConnectedClientCount()}\n";
            info += $"Latency: {GetNetworkLatency():F1}ms\n";
            
            return info;
        }
        
        /// <summary>
        /// Network rolünü string olarak döndürür
        /// </summary>
        public static string GetNetworkRole()
        {
            if (NetworkManager.Singleton == null)
                return "None";
            
            if (NetworkManager.Singleton.IsHost)
                return "Host";
            else if (NetworkManager.Singleton.IsServer)
                return "Server";
            else if (NetworkManager.Singleton.IsClient)
                return "Client";
            else
                return "Disconnected";
        }
    }
}

