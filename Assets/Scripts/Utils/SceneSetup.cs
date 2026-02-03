using UnityEngine;

/// <summary>
/// Configura uma cena específica com os elementos necessários.
/// Coloque este script em cada cena para garantir setup correto.
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [Header("Tipo de Cena")]
    [SerializeField] private SceneType sceneType = SceneType.Gameplay;

    [Header("Configurações de Gameplay")]
    [SerializeField] private bool spawnPlayer = true;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private bool setupPlaceholderSprites = true;

    public enum SceneType
    {
        Intro,
        Gameplay,
        Ending
    }

    private void Start()
    {
        switch (sceneType)
        {
            case SceneType.Intro:
                SetupIntroScene();
                break;
            case SceneType.Gameplay:
                SetupGameplayScene();
                break;
            case SceneType.Ending:
                SetupEndingScene();
                break;
        }
    }

    private void SetupIntroScene()
    {
        // Esconde UI de gameplay se existir
        GameUI gameUI = FindFirstObjectByType<GameUI>();
        if (gameUI != null)
        {
            gameUI.gameObject.SetActive(false);
        }
    }

    private void SetupGameplayScene()
    {
        // Garante que o jogador existe
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null && spawnPlayer)
        {
            SpawnPlayer();
        }

        // Setup sprites placeholder
        if (setupPlaceholderSprites)
        {
            SetupSprites();
        }

        // Ativa UI de gameplay
        GameUI gameUI = FindFirstObjectByType<GameUI>();
        if (gameUI != null)
        {
            gameUI.gameObject.SetActive(true);
        }
    }

    private void SetupEndingScene()
    {
        // Esconde UI de gameplay
        GameUI gameUI = FindFirstObjectByType<GameUI>();
        if (gameUI != null)
        {
            gameUI.gameObject.SetActive(false);
        }
    }

    private void SpawnPlayer()
    {
        // Cria jogador básico
        GameObject player = new GameObject("Player");
        player.tag = "Player";
        player.layer = 0;

        // Adiciona componentes necessários
        player.AddComponent<SpriteRenderer>();
        Rigidbody2D rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        CircleCollider2D col = player.AddComponent<CircleCollider2D>();
        col.radius = 0.5f;

        player.AddComponent<PlayerInventory>();
        player.AddComponent<PlayerHealth>();
        player.AddComponent<PlayerController>();

        // Cria ponto de segurar item
        GameObject holdPoint = new GameObject("ItemHoldPoint");
        holdPoint.transform.SetParent(player.transform);
        holdPoint.transform.localPosition = new Vector3(0, 0.8f, 0);

        // Posiciona no spawn point
        if (playerSpawnPoint != null)
        {
            player.transform.position = playerSpawnPoint.position;
        }

        // Configura sprite
        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        sr.sprite = PlaceholderSpriteGenerator.CreateCharacter(32, 64,
            new Color(0.3f, 0.5f, 0.3f),
            new Color(0.9f, 0.75f, 0.6f));
    }

    private void SetupSprites()
    {
        // Configura sprites de itens sem sprite
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = PlaceholderSpriteGenerator.CreateItemSprite(item.Type, 32);
            }
        }

        // Configura sprite do carro
        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);
        foreach (var car in cars)
        {
            SpriteRenderer sr = car.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = PlaceholderSpriteGenerator.CreateCar(64, 32,
                    new Color(0.6f, 0.2f, 0.2f),
                    new Color(0.1f, 0.1f, 0.1f));
            }
        }

        // Configura sprite do jogador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = PlaceholderSpriteGenerator.CreateCharacter(32, 64,
                    new Color(0.3f, 0.5f, 0.3f),
                    new Color(0.9f, 0.75f, 0.6f));
            }
        }
    }
}
