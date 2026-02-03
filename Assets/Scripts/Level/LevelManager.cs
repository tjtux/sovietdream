using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Gerencia o carregamento de fases e posicionamento do jogador.
/// Singleton que persiste entre cenas.
/// </summary>
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Configurações")]
    [SerializeField] private float fadeTime = 0.5f;

    private string pendingSpawnPointId;
    private bool isLoading;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    /// <summary>
    /// Carrega uma nova fase.
    /// </summary>
    public void LoadLevel(string sceneName, string spawnPointId = "")
    {
        if (isLoading)
            return;

        pendingSpawnPointId = spawnPointId;
        StartCoroutine(LoadLevelCoroutine(sceneName));
    }

    private IEnumerator LoadLevelCoroutine(string sceneName)
    {
        isLoading = true;

        // Aqui poderia adicionar fade out
        yield return new WaitForSeconds(fadeTime);

        // Carrega a cena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        isLoading = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Posiciona o jogador no spawn point correto
        if (!string.IsNullOrEmpty(pendingSpawnPointId))
        {
            SpawnPoint[] spawnPoints = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);

            foreach (var sp in spawnPoints)
            {
                if (sp.SpawnId == pendingSpawnPointId)
                {
                    PositionPlayerAtSpawnPoint(sp);
                    break;
                }
            }

            pendingSpawnPointId = "";
        }
    }

    private void PositionPlayerAtSpawnPoint(SpawnPoint spawnPoint)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && spawnPoint != null)
        {
            player.transform.position = spawnPoint.transform.position;
        }
    }

    /// <summary>
    /// Retorna o nome da cena atual.
    /// </summary>
    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Recarrega a cena atual.
    /// </summary>
    public void ReloadCurrentLevel()
    {
        LoadLevel(GetCurrentSceneName());
    }
}
