using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Gerenciador principal do jogo.
/// Controla o estado do jogo, transições de cena e progresso.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configurações de Cena")]
    [SerializeField] private string introSceneName = "IntroScene";
    [SerializeField] private string mainSceneName = "Crossroads";
    [SerializeField] private string endingSceneName = "EndingCutscene";

    [Header("Estado do Jogo")]
    [SerializeField] private bool keyDelivered;
    [SerializeField] private bool gasCanDelivered;
    [SerializeField] private bool tireDelivered;
    [SerializeField] private bool batteryDelivered;

    public bool KeyDelivered => keyDelivered;
    public bool GasCanDelivered => gasCanDelivered;
    public bool TireDelivered => tireDelivered;
    public bool BatteryDelivered => batteryDelivered;

    public bool AllPartsDelivered => keyDelivered && gasCanDelivered && tireDelivered && batteryDelivered;

    // Eventos
    public System.Action<ItemType> OnPartDelivered;
    public System.Action OnAllPartsDelivered;
    public System.Action OnGameWon;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        ResetProgress();
    }

    /// <summary>
    /// Inicia o jogo a partir da intro.
    /// </summary>
    public void StartGame()
    {
        ResetProgress();
        SceneManager.LoadScene(introSceneName);
    }

    /// <summary>
    /// Pula a intro e vai direto para o jogo.
    /// </summary>
    public void SkipToGame()
    {
        ResetProgress();
        SceneManager.LoadScene(mainSceneName);
    }

    /// <summary>
    /// Chamado quando a intro termina.
    /// </summary>
    public void OnIntroComplete()
    {
        SceneManager.LoadScene(mainSceneName);
    }

    /// <summary>
    /// Entrega uma peça ao carro.
    /// </summary>
    public bool DeliverPart(ItemType partType)
    {
        bool delivered = false;

        switch (partType)
        {
            case ItemType.Key:
                if (!keyDelivered)
                {
                    keyDelivered = true;
                    delivered = true;
                }
                break;
            case ItemType.GasCan:
                if (!gasCanDelivered)
                {
                    gasCanDelivered = true;
                    delivered = true;
                }
                break;
            case ItemType.Tire:
                if (!tireDelivered)
                {
                    tireDelivered = true;
                    delivered = true;
                }
                break;
            case ItemType.Battery:
                if (!batteryDelivered)
                {
                    batteryDelivered = true;
                    delivered = true;
                }
                break;
        }

        if (delivered)
        {
            OnPartDelivered?.Invoke(partType);
            AudioManager.Instance?.PlayPartDeliveredSound();

            if (AllPartsDelivered)
            {
                OnAllPartsDelivered?.Invoke();
            }
        }

        return delivered;
    }

    /// <summary>
    /// Verifica se uma peça específica foi entregue.
    /// </summary>
    public bool IsPartDelivered(ItemType partType)
    {
        return partType switch
        {
            ItemType.Key => keyDelivered,
            ItemType.GasCan => gasCanDelivered,
            ItemType.Tire => tireDelivered,
            ItemType.Battery => batteryDelivered,
            _ => false
        };
    }

    /// <summary>
    /// Conta quantas peças foram entregues.
    /// </summary>
    public int GetDeliveredPartsCount()
    {
        int count = 0;
        if (keyDelivered) count++;
        if (gasCanDelivered) count++;
        if (tireDelivered) count++;
        if (batteryDelivered) count++;
        return count;
    }

    /// <summary>
    /// Chamado quando o jogador entra no carro com todas as peças.
    /// </summary>
    public void WinGame()
    {
        OnGameWon?.Invoke();
        AudioManager.Instance?.PlayVictorySound();

        // Transição para a cutscene final
        StartCoroutine(TransitionToEnding());
    }

    private IEnumerator TransitionToEnding()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(endingSceneName);
    }

    /// <summary>
    /// Reinicia o jogo.
    /// </summary>
    public void RestartGame()
    {
        ResetProgress();
        SceneManager.LoadScene(mainSceneName);
    }

    /// <summary>
    /// Reseta o progresso do jogo.
    /// </summary>
    public void ResetProgress()
    {
        keyDelivered = false;
        gasCanDelivered = false;
        tireDelivered = false;
        batteryDelivered = false;
    }

    /// <summary>
    /// Carrega um quarteirão específico.
    /// </summary>
    public void LoadDistrict(string districtSceneName, string spawnPointId = "")
    {
        LevelManager.Instance?.LoadLevel(districtSceneName, spawnPointId);
    }

    /// <summary>
    /// Retorna ao cruzamento principal.
    /// </summary>
    public void ReturnToCrossroads(string spawnPointId = "")
    {
        LevelManager.Instance?.LoadLevel(mainSceneName, spawnPointId);
    }
}
