using UnityEngine;
using WasdBattle.Data;
using WasdBattle.Core;

namespace WasdBattle.Managers
{
    /// <summary>
    /// Item salvage (eritme) işlemlerini yöneten manager
    /// </summary>
    public class SalvageManager : MonoBehaviour
    {
        public static SalvageManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        /// <summary>
        /// Item'ı salvage et (erit) ve materyalleri geri ver
        /// </summary>
        /// <param name="itemData">Eritilecek item</param>
        /// <param name="count">Kaç adet eritilecek (varsayılan: 1)</param>
        /// <returns>İşlem başarılı mı?</returns>
        public bool SalvageItem(ItemData itemData, int count = 1)
        {
            if (itemData == null)
            {
                Debug.LogError("[SalvageManager] ItemData is null!");
                return false;
            }
            
            if (!itemData.canBeSalvaged)
            {
                Debug.LogWarning($"[SalvageManager] {itemData.itemName} cannot be salvaged!");
                return false;
            }
            
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
            {
                Debug.LogError("[SalvageManager] PlayerData not found!");
                return false;
            }
            
            // Inventory'de yeterli item var mı kontrol et
            int availableCount = playerData.GetItemCount(itemData.itemId);
            if (availableCount < count)
            {
                Debug.LogWarning($"[SalvageManager] Not enough {itemData.itemName} in inventory! (Have: {availableCount}, Need: {count})");
                return false;
            }
            
            // Salvage materyallerini al
            var salvageMaterials = itemData.GetSalvageMaterials();
            if (salvageMaterials.Length == 0)
            {
                Debug.LogWarning($"[SalvageManager] {itemData.itemName} has no salvage materials!");
                return false;
            }
            
            // Item'ı inventory'den çıkar
            playerData.RemoveItem(itemData.itemId, count);
            
            // Materyalleri ekle (count ile çarp)
            foreach (var material in salvageMaterials)
            {
                if (material.amount > 0)
                {
                    int totalAmount = material.amount * count;
                    playerData.ModifyMaterial(material.materialType, totalAmount);
                    Debug.Log($"[SalvageManager] Gained {material.materialType} x{totalAmount}");
                }
            }
            
            // Save
            GameManager.Instance?.SavePlayerData();
            
            Debug.Log($"[SalvageManager] Successfully salvaged {itemData.itemName} x{count}");
            return true;
        }
        
        /// <summary>
        /// Item'ın salvage edilebilir olup olmadığını kontrol et
        /// </summary>
        public bool CanSalvageItem(ItemData itemData, int count = 1)
        {
            if (itemData == null || !itemData.canBeSalvaged)
                return false;
            
            var playerData = GameManager.Instance?.CurrentPlayerData;
            if (playerData == null)
                return false;
            
            int availableCount = playerData.GetItemCount(itemData.itemId);
            return availableCount >= count;
        }
        
        /// <summary>
        /// Salvage preview - Ne kadar materyal kazanılacağını göster
        /// </summary>
        public string GetSalvagePreview(ItemData itemData, int count = 1)
        {
            if (itemData == null || !itemData.canBeSalvaged)
                return "Cannot be salvaged";
            
            var salvageMaterials = itemData.GetSalvageMaterials();
            if (salvageMaterials.Length == 0)
                return "No materials returned";
            
            string preview = $"Salvaging {itemData.itemName} x{count} will give:\n";
            foreach (var material in salvageMaterials)
            {
                if (material.amount > 0)
                {
                    int totalAmount = material.amount * count;
                    preview += $"• {material.materialType}: {totalAmount}\n";
                }
            }
            
            return preview.TrimEnd('\n');
        }
    }
}

