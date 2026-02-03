using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Zona de interação genérica. Pode ser usada para transições de fase ou outras interações.
/// </summary>
public class InteractionZone : MonoBehaviour, IInteractable
{
    public enum ZoneType
    {
        LevelTransition,
        Generic
    }

    [Header("Configurações da Zona")]
    [SerializeField] private ZoneType zoneType = ZoneType.LevelTransition;
    [SerializeField] private bool requiresItem;
    [SerializeField] private ItemType requiredItemType = ItemType.Generic;

    [Header("Transição de Fase")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private string spawnPointId;

    [Header("Eventos")]
    [SerializeField] private UnityEvent onInteract;
    [SerializeField] private UnityEvent onPlayerEnterZone;
    [SerializeField] private UnityEvent onPlayerExitZone;

    private bool playerInZone;

    public bool PlayerInZone => playerInZone;

    /// <summary>
    /// Verifica se a interação pode acontecer.
    /// </summary>
    public bool CanInteract()
    {
        if (!playerInZone)
            return false;

        // Se requer item, verifica se o jogador está carregando o item correto
        if (requiresItem && PlayerInventory.Instance != null)
        {
            if (!PlayerInventory.Instance.HasMainItem)
                return false;

            if (requiredItemType != ItemType.Generic &&
                PlayerInventory.Instance.MainItem.Type != requiredItemType)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Executa a interação.
    /// </summary>
    public void Interact()
    {
        if (!CanInteract())
            return;

        onInteract?.Invoke();

        switch (zoneType)
        {
            case ZoneType.LevelTransition:
                HandleLevelTransition();
                break;
            case ZoneType.Generic:
                // Eventos customizados via UnityEvent
                break;
        }
    }

    private void HandleLevelTransition()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning($"InteractionZone '{gameObject.name}': targetSceneName não definido!");
            return;
        }

        LevelManager.Instance?.LoadLevel(targetSceneName, spawnPointId);
    }

    public void OnPlayerEnter()
    {
        playerInZone = true;
        onPlayerEnterZone?.Invoke();
    }

    public void OnPlayerExit()
    {
        playerInZone = false;
        onPlayerExitZone?.Invoke();
    }

    private void OnDrawGizmos()
    {
        // Visualiza a zona no editor
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);

            if (col is BoxCollider2D box)
            {
                Gizmos.matrix = transform.localToWorldMatrix;
                Gizmos.DrawCube(box.offset, box.size);
            }
            else if (col is CircleCollider2D circle)
            {
                Gizmos.DrawSphere(transform.position + (Vector3)circle.offset, circle.radius);
            }
        }
    }
}
