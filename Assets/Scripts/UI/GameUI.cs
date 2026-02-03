using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI principal do jogo.
/// Mostra inventário, saúde, radiação e status das peças.
/// </summary>
public class GameUI : MonoBehaviour
{
    [Header("Saúde")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image healthFill;
    [SerializeField] private Color healthColorFull = Color.green;
    [SerializeField] private Color healthColorLow = Color.red;

    [Header("Radiação")]
    [SerializeField] private Slider radiationSlider;
    [SerializeField] private Image radiationFill;
    [SerializeField] private Color radiationColorSafe = Color.yellow;
    [SerializeField] private Color radiationColorDanger = Color.red;
    [SerializeField] private GameObject radiationWarningIcon;

    [Header("Inventário - Item Principal")]
    [SerializeField] private GameObject mainItemPanel;
    [SerializeField] private Image mainItemIcon;
    [SerializeField] private TextMeshProUGUI mainItemName;

    [Header("Inventário - Consumíveis")]
    [SerializeField] private GameObject consumable1Panel;
    [SerializeField] private Image consumable1Icon;
    [SerializeField] private TextMeshProUGUI consumable1Key;
    [SerializeField] private GameObject consumable2Panel;
    [SerializeField] private Image consumable2Icon;
    [SerializeField] private TextMeshProUGUI consumable2Key;

    [Header("Status do Carro")]
    [SerializeField] private GameObject carStatusPanel;
    [SerializeField] private Image keyStatusIcon;
    [SerializeField] private Image gasCanStatusIcon;
    [SerializeField] private Image tireStatusIcon;
    [SerializeField] private Image batteryStatusIcon;
    [SerializeField] private Color partMissingColor = new Color(0.3f, 0.3f, 0.3f);
    [SerializeField] private Color partDeliveredColor = Color.green;

    [Header("Mensagens")]
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDisplayTime = 3f;

    [Header("Prompt de Interação")]
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private TextMeshProUGUI interactionPromptText;

    private float messageTimer;

    private void Start()
    {
        // Inscreve nos eventos
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnHealthChanged.AddListener(UpdateHealthUI);
            PlayerHealth.Instance.OnRadiationChanged.AddListener(UpdateRadiationUI);
        }

        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnMainItemChanged.AddListener(UpdateMainItemUI);
            PlayerInventory.Instance.OnConsumableChanged.AddListener(UpdateConsumableUI);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPartDelivered += OnPartDelivered;
        }

        // Inicializa UI
        HideMessage();
        HideInteractionPrompt();
        UpdateCarStatusUI();
    }

    private void OnDestroy()
    {
        if (PlayerHealth.Instance != null)
        {
            PlayerHealth.Instance.OnHealthChanged.RemoveListener(UpdateHealthUI);
            PlayerHealth.Instance.OnRadiationChanged.RemoveListener(UpdateRadiationUI);
        }

        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnMainItemChanged.RemoveListener(UpdateMainItemUI);
            PlayerInventory.Instance.OnConsumableChanged.RemoveListener(UpdateConsumableUI);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPartDelivered -= OnPartDelivered;
        }
    }

    private void Update()
    {
        // Timer para mensagens
        if (messageTimer > 0)
        {
            messageTimer -= Time.deltaTime;
            if (messageTimer <= 0)
            {
                HideMessage();
            }
        }
    }

    private void UpdateHealthUI(float current, float max)
    {
        if (healthSlider != null)
        {
            healthSlider.value = current / max;
        }

        if (healthFill != null)
        {
            float healthPercent = current / max;
            healthFill.color = Color.Lerp(healthColorLow, healthColorFull, healthPercent);
        }
    }

    private void UpdateRadiationUI(float current, float max)
    {
        if (radiationSlider != null)
        {
            radiationSlider.value = current / max;
        }

        if (radiationFill != null)
        {
            float radiationPercent = current / max;
            radiationFill.color = Color.Lerp(radiationColorSafe, radiationColorDanger, radiationPercent);
        }

        if (radiationWarningIcon != null)
        {
            radiationWarningIcon.SetActive(current / max > 0.7f);
        }
    }

    private void UpdateMainItemUI(Item item)
    {
        if (mainItemPanel == null) return;

        if (item != null)
        {
            mainItemPanel.SetActive(true);

            if (mainItemName != null)
            {
                mainItemName.text = item.ItemName;
            }

            // Se o item tem sprite, mostra
            SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
            if (mainItemIcon != null && sr != null && sr.sprite != null)
            {
                mainItemIcon.sprite = sr.sprite;
                mainItemIcon.enabled = true;
            }
        }
        else
        {
            mainItemPanel.SetActive(false);
        }
    }

    private void UpdateConsumableUI(int slotIndex, ConsumableItem consumable)
    {
        GameObject panel = slotIndex == 0 ? consumable1Panel : consumable2Panel;
        Image icon = slotIndex == 0 ? consumable1Icon : consumable2Icon;

        if (panel == null) return;

        if (consumable != null)
        {
            panel.SetActive(true);

            SpriteRenderer sr = consumable.GetComponent<SpriteRenderer>();
            if (icon != null && sr != null && sr.sprite != null)
            {
                icon.sprite = sr.sprite;
                icon.enabled = true;
            }
        }
        else
        {
            panel.SetActive(false);
        }
    }

    private void UpdateCarStatusUI()
    {
        if (GameManager.Instance == null) return;

        if (keyStatusIcon != null)
            keyStatusIcon.color = GameManager.Instance.KeyDelivered ? partDeliveredColor : partMissingColor;

        if (gasCanStatusIcon != null)
            gasCanStatusIcon.color = GameManager.Instance.GasCanDelivered ? partDeliveredColor : partMissingColor;

        if (tireStatusIcon != null)
            tireStatusIcon.color = GameManager.Instance.TireDelivered ? partDeliveredColor : partMissingColor;

        if (batteryStatusIcon != null)
            batteryStatusIcon.color = GameManager.Instance.BatteryDelivered ? partDeliveredColor : partMissingColor;
    }

    private void OnPartDelivered(ItemType partType)
    {
        UpdateCarStatusUI();

        string partName = partType switch
        {
            ItemType.Key => "Chave",
            ItemType.GasCan => "Galão",
            ItemType.Tire => "Pneu",
            ItemType.Battery => "Bateria",
            _ => "Item"
        };

        ShowMessage($"{partName} entregue ao carro!");
    }

    public void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(true);
        }

        if (messageText != null)
        {
            messageText.text = message;
        }

        messageTimer = messageDisplayTime;
    }

    public void HideMessage()
    {
        if (messagePanel != null)
        {
            messagePanel.SetActive(false);
        }
    }

    public void ShowInteractionPrompt(string text)
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }

        if (interactionPromptText != null)
        {
            interactionPromptText.text = text;
        }
    }

    public void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }
}
