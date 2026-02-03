using UnityEngine;

/// <summary>
/// Inicializa o jogo e configura sprites placeholder se necessário.
/// Este script deve estar em um GameObject na primeira cena carregada.
/// </summary>
public class GameBootstrap : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private bool useGeneratedSprites = true;
    [SerializeField] private int spriteSize = 32;

    [Header("Referências (Opcional)")]
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite carSprite;

    private static bool isInitialized;

    private void Awake()
    {
        if (isInitialized)
        {
            Destroy(gameObject);
            return;
        }

        isInitialized = true;
        DontDestroyOnLoad(gameObject);

        InitializeGame();
    }

    private void InitializeGame()
    {
        // Garante que os managers existam
        EnsureManagersExist();

        // Configura sprites se necessário
        if (useGeneratedSprites)
        {
            SetupPlaceholderSprites();
        }
    }

    private void EnsureManagersExist()
    {
        // GameManager
        if (GameManager.Instance == null)
        {
            GameObject gmObj = new GameObject("GameManager");
            gmObj.AddComponent<GameManager>();
            DontDestroyOnLoad(gmObj);
        }

        // AudioManager
        if (AudioManager.Instance == null)
        {
            GameObject amObj = new GameObject("AudioManager");
            amObj.AddComponent<AudioManager>();
            DontDestroyOnLoad(amObj);
        }

        // LevelManager
        if (LevelManager.Instance == null)
        {
            GameObject lmObj = new GameObject("LevelManager");
            lmObj.AddComponent<LevelManager>();
            DontDestroyOnLoad(lmObj);
        }
    }

    private void SetupPlaceholderSprites()
    {
        // Configura sprite do jogador se não tiver
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                if (playerSprite != null)
                {
                    sr.sprite = playerSprite;
                }
                else
                {
                    sr.sprite = PlaceholderSpriteGenerator.CreateCharacter(
                        spriteSize, spriteSize * 2,
                        new Color(0.3f, 0.5f, 0.3f), // Corpo verde militar
                        new Color(0.9f, 0.75f, 0.6f) // Cabeça cor de pele
                    );
                }
            }
        }

        // Configura sprites de itens
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = PlaceholderSpriteGenerator.CreateItemSprite(item.Type, spriteSize);
            }
        }

        // Configura sprite do carro
        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);
        foreach (var car in cars)
        {
            SpriteRenderer sr = car.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                if (carSprite != null)
                {
                    sr.sprite = carSprite;
                }
                else
                {
                    sr.sprite = PlaceholderSpriteGenerator.CreateCar(
                        spriteSize * 2, spriteSize,
                        new Color(0.6f, 0.2f, 0.2f), // Corpo vermelho escuro
                        new Color(0.1f, 0.1f, 0.1f)  // Rodas pretas
                    );
                }
            }
        }
    }
}
