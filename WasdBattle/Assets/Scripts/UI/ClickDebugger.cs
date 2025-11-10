using UnityEngine;
using UnityEngine.EventSystems;

namespace WasdBattle.UI
{
    /// <summary>
    /// Geçici debug script - UI elementine tıklanıp tıklanmadığını test eder
    /// </summary>
    public class ClickDebugger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.LogError($"[ClickDebugger] CLICKED: {gameObject.name}");
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.LogWarning($"[ClickDebugger] HOVER ENTER: {gameObject.name}");
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.LogWarning($"[ClickDebugger] HOVER EXIT: {gameObject.name}");
        }
    }
}

