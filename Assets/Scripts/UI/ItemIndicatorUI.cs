using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI simples que mostra qual item o jogador está carregando.
/// </summary>
public class ItemIndicatorUI : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPanel;
    [SerializeField] private Text itemNameText; // Use TextMeshProUGUI se preferir TMP

    private PlayerInteraction playerInteraction;

    private void Start()
    {
        playerInteraction = FindFirstObjectByType<PlayerInteraction>();
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (playerInteraction == null)
        {
            if (indicatorPanel != null)
                indicatorPanel.SetActive(false);
            return;
        }

        bool hasItem = playerInteraction.IsCarryingItem;

        if (indicatorPanel != null)
            indicatorPanel.SetActive(hasItem);

        if (hasItem && itemNameText != null && playerInteraction.CarriedItem != null)
        {
            itemNameText.text = playerInteraction.CarriedItem.ItemName;
        }
    }
}
