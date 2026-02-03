using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Gerencia interações do jogador: pegar/soltar itens e interagir com o cenário.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Transform itemHoldPoint;
    [SerializeField] private float itemDetectionRadius = 1.5f;
    [SerializeField] private LayerMask itemLayer;

    private Item carriedItem;
    private List<InteractionZone> nearbyZones = new List<InteractionZone>();
    private InputAction pickupAction;
    private InputAction dropAction;
    private InputAction interactAction;

    public bool IsCarryingItem => carriedItem != null;
    public Item CarriedItem => carriedItem;

    private void Awake()
    {
        // Cria ações de input customizadas para este script
        // E - Pegar item
        pickupAction = new InputAction("Pickup", binding: "<Keyboard>/e");
        pickupAction.AddBinding("<Gamepad>/buttonNorth");
        pickupAction.Enable();

        // Q - Soltar item
        dropAction = new InputAction("Drop", binding: "<Keyboard>/q");
        dropAction.AddBinding("<Gamepad>/buttonWest");
        dropAction.Enable();

        // Espaço - Interagir com zona
        interactAction = new InputAction("Interact", binding: "<Keyboard>/space");
        interactAction.AddBinding("<Gamepad>/buttonSouth");
        interactAction.Enable();
    }

    private void OnDestroy()
    {
        // Limpa as ações ao destruir o objeto
        pickupAction?.Disable();
        dropAction?.Disable();
        interactAction?.Disable();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // E - Pegar item
        if (pickupAction.WasPressedThisFrame())
        {
            TryPickupItem();
        }

        // Q - Soltar item
        if (dropAction.WasPressedThisFrame())
        {
            TryDropItem();
        }

        // Espaço - Interagir com cenário
        if (interactAction.WasPressedThisFrame())
        {
            TryInteractWithZone();
        }
    }

    private void TryPickupItem()
    {
        // Só pode pegar se não estiver carregando item
        if (carriedItem != null)
            return;

        // Busca itens próximos
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
            PickupItem(closestItem);
        }
    }

    private void PickupItem(Item item)
    {
        carriedItem = item;
        carriedItem.OnPickup(itemHoldPoint);
    }

    private void TryDropItem()
    {
        if (carriedItem == null)
            return;

        DropItem();
    }

    private void DropItem()
    {
        carriedItem.OnDrop(transform.position);
        carriedItem = null;
    }

    private void TryInteractWithZone()
    {
        // Interage com a primeira zona disponível
        foreach (var zone in nearbyZones)
        {
            if (zone != null && zone.CanInteract())
            {
                zone.Interact();
                break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractionZone zone = other.GetComponent<InteractionZone>();
        if (zone != null && !nearbyZones.Contains(zone))
        {
            nearbyZones.Add(zone);
            zone.OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractionZone zone = other.GetComponent<InteractionZone>();
        if (zone != null)
        {
            nearbyZones.Remove(zone);
            zone.OnPlayerExit();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualiza o raio de detecção de itens no editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, itemDetectionRadius);
    }
}
