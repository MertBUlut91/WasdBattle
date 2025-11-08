using UnityEngine;

namespace WasdBattle.Data
{
    /// <summary>
    /// Combo verilerini tutan ScriptableObject.
    /// Her skill'in kendine ait bir combo dizisi vardır.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCombo", menuName = "WasdBattle/Combo Data")]
    public class ComboData : ScriptableObject
    {
        [Header("Combo Sequence")]
        [Tooltip("WASD tuş dizisi")]
        public KeyCode[] comboSequence;
        
        [Header("Timing")]
        [Tooltip("Her tuş için izin verilen süre (saniye)")]
        public float timeWindowPerKey = 0.5f;
        
        [Tooltip("Toplam combo süresi (saniye)")]
        public float totalDuration = 3f;
        
        [Header("Difficulty")]
        [Tooltip("Combo zorluğu (1-10)")]
        [Range(1, 10)]
        public int difficulty = 1;
        
        public int ComboLength => comboSequence != null ? comboSequence.Length : 0;
        
        /// <summary>
        /// Combo'nun string temsilini döndürür (debug için)
        /// </summary>
        public string GetComboString()
        {
            if (comboSequence == null || comboSequence.Length == 0)
                return "Empty";
            
            string result = "";
            foreach (var key in comboSequence)
            {
                result += key.ToString() + " ";
            }
            return result.Trim();
        }
        
        /// <summary>
        /// Random bir WASD combo dizisi oluşturur
        /// </summary>
        public static KeyCode[] GenerateRandomCombo(int length)
        {
            KeyCode[] wasdKeys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
            KeyCode[] combo = new KeyCode[length];
            
            for (int i = 0; i < length; i++)
            {
                combo[i] = wasdKeys[Random.Range(0, wasdKeys.Length)];
            }
            
            return combo;
        }
    }
}

