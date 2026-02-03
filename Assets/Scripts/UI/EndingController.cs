using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controlador da cena de ending.
/// Placeholder para a cutscene que será criada pelos artistas.
/// </summary>
public class EndingController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI endingText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    [Header("Texto do Ending")]
    [TextArea(5, 10)]
    [SerializeField] private string endingMessage =
        "Você Escapou!\n\n" +
        "Você conseguiu consertar o carro e fugir de Pripyat\n" +
        "antes que a radiação o consumisse.\n\n" +
        "Mas a que custo?\n\n" +
        "[Placeholder para cutscene dos artistas]";

    private void Start()
    {
        if (endingText != null)
        {
            endingText.text = endingMessage;
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }

        // Para a música normal e toca música de vitória
        AudioManager.Instance?.PlayVictoryMusic();
    }

    private void RestartGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Crossroads");
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
