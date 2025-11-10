using UnityEngine;
using UnityEditor;
using WasdBattle.Data;

/// <summary>
/// Custom inspector for ItemData to show salvage preview
/// </summary>
[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Default inspector
            DrawDefaultInspector();
            
            ItemData itemData = (ItemData)target;
            
            // DEBUG: Script çalışıyor mu?
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("=== CUSTOM EDITOR ACTIVE ===", EditorStyles.boldLabel);
            
            // Salvage Preview
            if (itemData.canBeSalvaged && itemData.craftingMaterials != null && itemData.craftingMaterials.Length > 0)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Salvage Preview", EditorStyles.boldLabel);
                
                EditorGUILayout.HelpBox(
                    $"Bu item eritildiğinde şu materyaller geri dönecek:\n" +
                    $"(Crafting maliyetinin %{itemData.salvageReturnRate * 100:F0}'i)",
                    MessageType.Info
                );
                
                var salvageMats = itemData.GetSalvageMaterials();
                
                EditorGUI.indentLevel++;
                foreach (var material in salvageMats)
                {
                    if (material.amount > 0)
                    {
                        EditorGUILayout.LabelField($"• {material.materialType}", $"{material.amount}");
                    }
                }
                EditorGUI.indentLevel--;
                
                EditorGUILayout.Space(5);
                
                // Crafting vs Salvage karşılaştırması
                EditorGUILayout.LabelField("Crafting Cost vs Salvage Return:", EditorStyles.miniLabel);
                EditorGUI.indentLevel++;
                for (int i = 0; i < itemData.craftingMaterials.Length; i++)
                {
                    var craftMat = itemData.craftingMaterials[i];
                    var salvageMat = salvageMats[i];
                    
                    if (craftMat.amount > 0)
                    {
                        EditorGUILayout.LabelField(
                            $"{craftMat.materialType}:",
                            $"{craftMat.amount} → {salvageMat.amount}",
                            EditorStyles.miniLabel
                        );
                    }
                }
                EditorGUI.indentLevel--;
            }
            else if (itemData.canBeSalvaged && (itemData.craftingMaterials == null || itemData.craftingMaterials.Length == 0))
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.HelpBox(
                    "Bu item eritilebilir olarak işaretlenmiş ama crafting materyali yok.\n" +
                    "Salvage için önce Crafting Materials ekleyin.",
                    MessageType.Warning
                );
            }
        }
    }

