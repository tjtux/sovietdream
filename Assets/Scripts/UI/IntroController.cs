using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Controlador da cena de introdução.
/// Placeholder para a cutscene que será criada pelos artistas.
/// </summary>
public class IntroController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI introText;
    [SerializeField] private Button skipButton;
    [SerializeField] private float autoSkipTime = 10f;

    [Header("Texto da Intro")]
    [TextArea(5, 10)]
    [SerializeField] private string introMessage =
        "Soviet Dream\n\n" +
        "Ano de 1986. Você acordou em Pripyat.\n" +
        "A radiação aumenta a cada segundo.\n\n" +
        "Encontre as peças do carro e escape\n" +
        "antes que seja tarde demais.\n\n" +
        "[Pressione ESPAÇO ou clique para continuar]";

    private float timer;
    private bool skipped;

    private void Start()
    {
        if (introText != null)
        {
            introText.text = introMessage;
        }

        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipIntro);
        }

        timer = autoSkipTime;
    }

    private void Update()
    {
        if (skipped) return;

        // Skip com qualquer tecla ou clique
        if (Input.anyKeyDown || Input.GetMouseButtonDown(0))
        {
            SkipIntro();
            return;
        }

        // Auto-skip após tempo
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SkipIntro();
        }
    }

    private void SkipIntro()
    {
        if (skipped) return;
        skipped = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnIntroComplete();
        }
        else
        {
            // Fallback se GameManager não existir
            UnityEngine.SceneManagement.SceneManager.LoadScene("Crossroads");
        }
    }
}
