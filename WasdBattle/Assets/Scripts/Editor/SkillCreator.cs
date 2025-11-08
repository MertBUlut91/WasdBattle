using UnityEngine;
using UnityEditor;
using WasdBattle.Data;

namespace WasdBattle.Editor
{
    /// <summary>
    /// Temel skill'leri oluşturmak için editor tool
    /// </summary>
    public class SkillCreator : EditorWindow
    {
        [MenuItem("WasdBattle/Create Default Skills")]
        public static void CreateDefaultSkills()
        {
            // Klasör oluştur
            string folderPath = "Assets/_Project/ScriptableObjects/Skills";
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/_Project/ScriptableObjects", "Skills");
            }
            
            // Combo klasörü
            string comboFolderPath = folderPath + "/Combos";
            if (!AssetDatabase.IsValidFolder(comboFolderPath))
            {
                AssetDatabase.CreateFolder(folderPath, "Combos");
            }
            
            // Temel combo'lar oluştur
            ComboData fastCombo = CreateCombo("FastCombo", new KeyCode[] { KeyCode.W, KeyCode.W, KeyCode.D }, 0.4f, 2f, comboFolderPath);
            ComboData heavyCombo = CreateCombo("HeavyCombo", new KeyCode[] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.W }, 0.5f, 4f, comboFolderPath);
            ComboData specialCombo = CreateCombo("SpecialCombo", new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D }, 0.45f, 3f, comboFolderPath);
            
            // Temel skill'ler oluştur
            CreateFastSkill(folderPath, fastCombo);
            CreateHeavySkill(folderPath, heavyCombo);
            CreateSpecialSkill(folderPath, specialCombo);
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("[SkillCreator] Created default skills and combos!");
        }
        
        private static ComboData CreateCombo(string name, KeyCode[] sequence, float timePerKey, float totalDuration, string path)
        {
            ComboData combo = ScriptableObject.CreateInstance<ComboData>();
            combo.comboSequence = sequence;
            combo.timeWindowPerKey = timePerKey;
            combo.totalDuration = totalDuration;
            combo.difficulty = sequence.Length;
            
            AssetDatabase.CreateAsset(combo, $"{path}/{name}.asset");
            return combo;
        }
        
        private static void CreateFastSkill(string path, ComboData combo)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            skill.skillId = "skill_fast_strike";
            skill.skillName = "Hızlı Vuruş";
            skill.description = "Hızlı ve düşük maliyetli bir saldırı.";
            skill.skillType = SkillType.Fast;
            skill.staminaCost = 15;
            skill.baseDamage = 20;
            skill.cooldown = 0f;
            skill.comboData = combo;
            skill.rarity = SkillRarity.Common;
            
            AssetDatabase.CreateAsset(skill, $"{path}/FastStrike.asset");
        }
        
        private static void CreateHeavySkill(string path, ComboData combo)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            skill.skillId = "skill_heavy_blow";
            skill.skillName = "Ağır Darbe";
            skill.description = "Yüksek hasar veren ama yavaş bir saldırı.";
            skill.skillType = SkillType.Heavy;
            skill.staminaCost = 35;
            skill.baseDamage = 50;
            skill.cooldown = 3f;
            skill.comboData = combo;
            skill.rarity = SkillRarity.Uncommon;
            
            AssetDatabase.CreateAsset(skill, $"{path}/HeavyBlow.asset");
        }
        
        private static void CreateSpecialSkill(string path, ComboData combo)
        {
            SkillData skill = ScriptableObject.CreateInstance<SkillData>();
            skill.skillId = "skill_stamina_drain";
            skill.skillName = "Enerji Çalma";
            skill.description = "Rakibin enerjisini çalar ve kendine ekler.";
            skill.skillType = SkillType.Special;
            skill.staminaCost = 25;
            skill.baseDamage = 25;
            skill.cooldown = 5f;
            skill.comboData = combo;
            skill.specialEffects = new SkillEffectType[] { SkillEffectType.StaminaDrain };
            skill.effectStrength = 1f;
            skill.rarity = SkillRarity.Rare;
            
            AssetDatabase.CreateAsset(skill, $"{path}/StaminaDrain.asset");
        }
    }
}

