using UnityEngine;
using UnityEngine.SceneManagement;
using WasdBattle.Core;

public class BootSceneController : MonoBehaviour
{
    [SerializeField] private float _minLoadTime = 2f;
    
    private async void Start()
    {
        float startTime = Time.time;
        
        // GameManager'ın başlamasını bekle
        while (GameManager.Instance == null || GameManager.Instance.CurrentPlayerData == null)
        {
            await System.Threading.Tasks.Task.Yield();
        }
        
        // Minimum yükleme süresi
        float elapsed = Time.time - startTime;
        if (elapsed < _minLoadTime)
        {
            await System.Threading.Tasks.Task.Delay((int)((_minLoadTime - elapsed) * 1000));
        }
        
        // Ana menüye geç
        SceneManager.LoadScene("MainMenuScene");
    }
}