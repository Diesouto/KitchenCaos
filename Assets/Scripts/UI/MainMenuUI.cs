using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    private void Awake()
    {
        // Nos aseguramos de que el juego no esté pausado al cargar el menú principal
        Time.timeScale = 1f;

        playButton.onClick.AddListener(() =>
        {
            Loader.LoadScene(Loader.Scene.GameScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
