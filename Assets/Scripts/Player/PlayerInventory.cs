using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Sistema de inventário do jogador.
/// - 1 slot para item principal (peças do carro)
/// - 2 slots para consumíveis (anti-radiação)
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance { get; private set; }

    [Header("Configurações")]
    [SerializeField] private Transform itemHoldPoint;
    [SerializeField] private float itemDetectionRadius = 1.5f;
    [SerializeField] private LayerMask itemLayer;

    [Header("Inventário")]
    [SerializeField] private Item mainItem;
    [SerializeField] private ConsumableItem[] consumableSlots = new ConsumableItem[2];

    [Header("Eventos")]
    public UnityEvent<Item> OnMainItemChanged;
    public UnityEvent<int, ConsumableItem> OnConsumableChanged;
    public UnityEvent<ConsumableItem> OnConsumableUsed;

    public Item MainItem => mainItem;
    public ConsumableItem[] Consumables => consumableSlots;
    public bool HasMainItem => mainItem != null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        consumableSlots = new ConsumableItem[2];
    }

    /// <summary>
    /// Tenta pegar o item mais próximo.
    /// </summary>
    public bool TryPickupItem()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position,
            itemDetectionRadius,
            itemLayer
        );

        Item closestItem = null;
        float closestDistance = float.MaxValue;

        foreach (var collider in colliders)
        {
            Item item = collider.GetComponent<Item>();
            if (item != null && !item.IsBeingCarried)
            {
                float distance = Vector2.Distance(transform.position, item.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestItem = item;
                }
            }
        }

        if (closestItem != null)
        {
            return PickupItem(closestItem);
        }

        return false;
    }

    /// <summary>
    /// Pega um item específico.
    /// </summary>
    public bool PickupItem(Item item)
    {
        if (item == null) return false;

        // Se for consumível, tenta adicionar aos slots de consumível
        ConsumableItem consumable = item as ConsumableItem;
        if (consumable != null)
        {
            return AddConsumable(consumable);
        }

        // Se for item principal
        if (mainItem != null)
        {
            // Já tem um item principal, não pode pegar outro
            return false;
        }

        mainItem = item;
        mainItem.OnPickup(itemHoldPoint);
        OnMainItemChanged?.Invoke(mainItem);

        AudioManager.Instance?.PlayPickupSound();
        return true;
    }

    /// <summary>
    /// Adiciona um consumível ao inventário.
    /// </summary>
    private bool AddConsumable(ConsumableItem consumable)
    {
        for (int i = 0; i < consumableSlots.Length; i++)
        {
            if (consumableSlots[i] == null)
            {
                consumableSlots[i] = consumable;
                consumable.OnPickup(null); // Consumíveis não seguem o jogador visualmente
                consumable.gameObject.SetActive(false);
                OnConsumableChanged?.Invoke(i, consumable);

                AudioManager.Instance?.PlayPickupSound();
                return true;
            }
        }

        // Slots de consumível cheios
        return false;
    }

    /// <summary>
    /// Solta o item principal.
    /// </summary>
    public void DropMainItem()
    {
        if (mainItem == null) return;

        mainItem.OnDrop(transform.position);
        mainItem = null;
        OnMainItemChanged?.Invoke(null);

        AudioManager.Instance?.PlayDropSound();
    }

    /// <summary>
    /// Usa um consumível do slot especificado.
    /// </summary>
    public bool UseConsumable(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= consumableSlots.Length) return false;
        if (consumableSlots[slotIndex] == null) return false;

        ConsumableItem consumable = consumableSlots[slotIndex];
        consumable.Use(GetComponent<PlayerHealth>());

        OnConsumableUsed?.Invoke(consumable);

        // Remove o consumível do slot
        Destroy(consumable.gameObject);
        consumableSlots[slotIndex] = null;
        OnConsumableChanged?.Invoke(slotIndex, null);

        AudioManager.Instance?.PlayConsumeSound();
        return true;
    }

    /// <summary>
    /// Remove e retorna o item principal (para entregar ao carro).
    /// </summary>
    public Item TakeMainItem()
    {
        Item item = mainItem;
        if (item != null)
        {
            mainItem = null;
            OnMainItemChanged?.Invoke(null);
        }
        return item;
    }

    /// <summary>
    /// Verifica se tem um item principal de um tipo específico.
    /// </summary>
    public bool HasItemOfType(ItemType type)
    {
        return mainItem != null && mainItem.Type == type;
    }

    /// <summary>
    /// Conta quantos consumíveis o jogador tem.
    /// </summary>
    public int GetConsumableCount()
    {
        int count = 0;
        foreach (var c in consumableSlots)
        {
            if (c != null) count++;
        }
        return count;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, itemDetectionRadius);
    }
}
