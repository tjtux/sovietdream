using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// O carro que o jogador precisa consertar para escapar.
/// Recebe as peças e permite a fuga quando todas são entregues.
/// </summary>
public class Car : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite brokenCarSprite;
    [SerializeField] private Sprite repairedCarSprite;

    [Header("Indicadores de Peças")]
    [SerializeField] private GameObject keyIndicator;
    [SerializeField] private GameObject gasCanIndicator;
    [SerializeField] private GameObject tireIndicator;
    [SerializeField] private GameObject batteryIndicator;

    [Header("Interação")]
    [SerializeField] private float interactionRadius = 2f;

    [Header("Eventos")]
    public UnityEvent OnPartReceived;
    public UnityEvent OnCarRepaired;
    public UnityEvent OnCarEntered;

    private bool isPlayerNearby;

    private void Start()
    {
        UpdateVisuals();

        // Inscreve nos eventos do GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPartDelivered += OnPartDelivered;
            GameManager.Instance.OnAllPartsDelivered += OnAllPartsDeliveredHandler;
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPartDelivered -= OnPartDelivered;
            GameManager.Instance.OnAllPartsDelivered -= OnAllPartsDeliveredHandler;
        }
    }

    private void Update()
    {
        CheckPlayerProximity();
    }

    private void CheckPlayerProximity()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        isPlayerNearby = distance <= interactionRadius;
    }

    /// <summary>
    /// Chamado quando o jogador tenta interagir com o carro.
    /// </summary>
    public void Interact()
    {
        if (!isPlayerNearby) return;

        PlayerInventory inventory = PlayerInventory.Instance;
        if (inventory == null) return;

        // Se o jogador tem um item principal, tenta entregar ao carro
        if (inventory.HasMainItem)
        {
            Item item = inventory.MainItem;
            TryDeliverPart(item);
        }
        // Se o carro está consertado e não tem item, entra no carro
        else if (GameManager.Instance != null && GameManager.Instance.AllPartsDelivered)
        {
            EnterCar();
        }
    }

    /// <summary>
    /// Tenta entregar uma peça ao carro.
    /// </summary>
    private void TryDeliverPart(Item item)
    {
        if (item == null) return;

        ItemType type = item.Type;

        // Verifica se é uma peça válida para o carro
        if (type != ItemType.Key && type != ItemType.GasCan &&
            type != ItemType.Tire && type != ItemType.Battery)
        {
            return;
        }

        // Tenta entregar a peça
        if (GameManager.Instance != null && GameManager.Instance.DeliverPart(type))
        {
            // Remove o item do jogador e destrói
            PlayerInventory inventory = PlayerInventory.Instance;
            if (inventory != null)
            {
                Item deliveredItem = inventory.TakeMainItem();
                if (deliveredItem != null)
                {
                    Destroy(deliveredItem.gameObject);
                }
            }

            OnPartReceived?.Invoke();
            AudioManager.Instance?.PlayInteractSound();
            UpdateVisuals();
        }
    }

    /// <summary>
    /// Entra no carro e vence o jogo.
    /// </summary>
    private void EnterCar()
    {
        OnCarEntered?.Invoke();
        GameManager.Instance?.WinGame();
    }

    /// <summary>
    /// Atualiza os visuais do carro baseado nas peças entregues.
    /// </summary>
    private void UpdateVisuals()
    {
        if (GameManager.Instance == null) return;

        // Atualiza indicadores de peças
        if (keyIndicator != null)
            keyIndicator.SetActive(GameManager.Instance.KeyDelivered);

        if (gasCanIndicator != null)
            gasCanIndicator.SetActive(GameManager.Instance.GasCanDelivered);

        if (tireIndicator != null)
            tireIndicator.SetActive(GameManager.Instance.TireDelivered);

        if (batteryIndicator != null)
            batteryIndicator.SetActive(GameManager.Instance.BatteryDelivered);

        // Atualiza sprite do carro
        if (spriteRenderer != null)
        {
            if (GameManager.Instance.AllPartsDelivered && repairedCarSprite != null)
            {
                spriteRenderer.sprite = repairedCarSprite;
            }
            else if (brokenCarSprite != null)
            {
                spriteRenderer.sprite = brokenCarSprite;
            }
        }
    }

    private void OnPartDelivered(ItemType partType)
    {
        UpdateVisuals();
    }

    private void OnAllPartsDeliveredHandler()
    {
        OnCarRepaired?.Invoke();
        UpdateVisuals();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
