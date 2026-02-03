using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

/// <summary>
/// Controlador unificado do jogador.
/// Gerencia input para movimento, inventário e interações.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInventory))]
[RequireComponent(typeof(PlayerHealth))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Interação")]
    [SerializeField] private float interactionRadius = 1.5f;

    private Rigidbody2D rb;
    private PlayerInventory inventory;
    private PlayerHealth health;
    private Vector2 moveInput;

    // Input Actions
    private InputAction moveAction;
    private InputAction pickupAction;
    private InputAction dropAction;
    private InputAction interactAction;
    private InputAction useConsumable1Action;
    private InputAction useConsumable2Action;

    // Zonas de interação próximas
    private List<IInteractable> nearbyInteractables = new List<IInteractable>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inventory = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();

        SetupInputActions();
    }

    private void SetupInputActions()
    {
        // Movimento - tenta usar PlayerInput, senão cria manualmente
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
        }

        // E - Pegar item
        pickupAction = new InputAction("Pickup", binding: "<Keyboard>/e");
        pickupAction.AddBinding("<Gamepad>/buttonNorth");
        pickupAction.Enable();

        // Q - Soltar item
        dropAction = new InputAction("Drop", binding: "<Keyboard>/q");
        dropAction.AddBinding("<Gamepad>/buttonWest");
        dropAction.Enable();

        // Espaço - Interagir
        interactAction = new InputAction("Interact", binding: "<Keyboard>/space");
        interactAction.AddBinding("<Gamepad>/buttonSouth");
        interactAction.Enable();

        // 1 - Usar consumível slot 1
        useConsumable1Action = new InputAction("UseConsumable1", binding: "<Keyboard>/1");
        useConsumable1Action.AddBinding("<Gamepad>/leftShoulder");
        useConsumable1Action.Enable();

        // 2 - Usar consumível slot 2
        useConsumable2Action = new InputAction("UseConsumable2", binding: "<Keyboard>/2");
        useConsumable2Action.AddBinding("<Gamepad>/rightShoulder");
        useConsumable2Action.Enable();
    }

    private void OnDestroy()
    {
        pickupAction?.Disable();
        dropAction?.Disable();
        interactAction?.Disable();
        useConsumable1Action?.Disable();
        useConsumable2Action?.Disable();
    }

    private void Update()
    {
        ReadMovementInput();
        HandleActionInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadMovementInput()
    {
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }
        else
        {
            // Fallback para input legado
            moveInput = Vector2.zero;

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                moveInput.y = 1f;
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                moveInput.y = -1f;

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                moveInput.x = 1f;
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                moveInput.x = -1f;
        }

        if (moveInput.magnitude > 1f)
            moveInput.Normalize();
    }

    private void HandleActionInput()
    {
        // E - Pegar item
        if (pickupAction.WasPressedThisFrame())
        {
            inventory.TryPickupItem();
        }

        // Q - Soltar item
        if (dropAction.WasPressedThisFrame())
        {
            inventory.DropMainItem();
        }

        // Espaço - Interagir com o cenário
        if (interactAction.WasPressedThisFrame())
        {
            TryInteract();
        }

        // 1 - Usar consumível slot 1
        if (useConsumable1Action.WasPressedThisFrame())
        {
            inventory.UseConsumable(0);
        }

        // 2 - Usar consumível slot 2
        if (useConsumable2Action.WasPressedThisFrame())
        {
            inventory.UseConsumable(1);
        }
    }

    private void Move()
    {
        if (health != null && health.IsDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void TryInteract()
    {
        // Primeiro, tenta interagir com zonas de interação próximas
        foreach (var interactable in nearbyInteractables)
        {
            if (interactable != null && interactable.CanInteract())
            {
                interactable.Interact();
                AudioManager.Instance?.PlayInteractSound();
                return;
            }
        }

        // Se não há zonas, tenta interagir com o carro
        Car car = FindNearestCar();
        if (car != null)
        {
            car.Interact();
        }
    }

    private Car FindNearestCar()
    {
        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);
        Car nearest = null;
        float nearestDistance = float.MaxValue;

        foreach (var car in cars)
        {
            float distance = Vector2.Distance(transform.position, car.transform.position);
            if (distance < interactionRadius && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearest = car;
            }
        }

        return nearest;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null && !nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
            interactable.OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponent<IInteractable>();
        if (interactable != null)
        {
            nearbyInteractables.Remove(interactable);
            interactable.OnPlayerExit();
        }
    }

    public Vector2 GetMoveDirection()
    {
        return moveInput;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
