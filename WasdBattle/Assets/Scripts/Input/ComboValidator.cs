using UnityEngine;
using Unity.Netcode;

namespace WasdBattle.Input
{
    /// <summary>
    /// Combo doğrulama ve skor hesaplama sınıfı
    /// </summary>
    public class ComboValidator
    {
        /// <summary>
        /// İki combo dizisini karşılaştırır ve sonuç döndürür
        /// </summary>
        public ComboResult Validate(KeyCode[] expectedCombo, KeyCode[] inputCombo)
        {
            if (expectedCombo == null || inputCombo == null)
            {
                return new ComboResult
                {
                    isPerfect = false,
                    accuracy = 0f,
                    correctKeys = 0,
                    totalKeys = expectedCombo?.Length ?? 0,
                    grade = ComboGrade.Failed
                };
            }
            
            int correctKeys = 0;
            int totalKeys = expectedCombo.Length;
            
            // Her tuşu karşılaştır
            int compareLength = Mathf.Min(expectedCombo.Length, inputCombo.Length);
            for (int i = 0; i < compareLength; i++)
            {
                if (expectedCombo[i] == inputCombo[i])
                {
                    correctKeys++;
                }
            }
            
            // Accuracy hesapla
            float accuracy = totalKeys > 0 ? (float)correctKeys / totalKeys : 0f;
            
            // Grade belirle
            ComboGrade grade = DetermineGrade(accuracy);
            
            // Perfect kontrolü
            bool isPerfect = correctKeys == totalKeys && inputCombo.Length == expectedCombo.Length;
            
            return new ComboResult
            {
                isPerfect = isPerfect,
                accuracy = accuracy,
                correctKeys = correctKeys,
                totalKeys = totalKeys,
                grade = grade
            };
        }
        
        /// <summary>
        /// Accuracy'ye göre grade belirler
        /// </summary>
        private ComboGrade DetermineGrade(float accuracy)
        {
            if (accuracy >= 1.0f)
                return ComboGrade.Perfect;
            else if (accuracy >= 0.9f)
                return ComboGrade.Excellent;
            else if (accuracy >= 0.75f)
                return ComboGrade.Good;
            else if (accuracy >= 0.5f)
                return ComboGrade.Partial;
            else
                return ComboGrade.Failed;
        }
    }
    
    /// <summary>
    /// Combo sonucu verisi - Network serializable
    /// </summary>
    public struct ComboResult : INetworkSerializable
    {
        public bool isPerfect;
        public float accuracy; // 0.0 - 1.0
        public int correctKeys;
        public int totalKeys;
        public ComboGrade grade;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref isPerfect);
            serializer.SerializeValue(ref accuracy);
            serializer.SerializeValue(ref correctKeys);
            serializer.SerializeValue(ref totalKeys);
            serializer.SerializeValue(ref grade);
        }
        
        public override string ToString()
        {
            return $"Grade: {grade}, Accuracy: {accuracy:P}, Keys: {correctKeys}/{totalKeys}";
        }
    }
    
    /// <summary>
    /// Combo başarı dereceleri
    /// </summary>
    public enum ComboGrade
    {
        Failed,    // < 50%
        Partial,   // 50-74%
        Good,      // 75-89%
        Excellent, // 90-99%
        Perfect    // 100%
    }
}

