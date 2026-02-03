using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla o movimento do jogador em 4 direções (cima, baixo, esquerda, direita).
/// Usa o novo Input System da Unity.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Busca a ação de movimento do Input System
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            moveAction = playerInput.actions["Move"];
        }
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ReadInput()
    {
        if (moveAction != null)
        {
            // Lê input do novo Input System
            moveInput = moveAction.ReadValue<Vector2>();
        }
        else
        {
            // Fallback para input legado (WASD/Setas)
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

        // Normaliza para evitar movimento diagonal mais rápido
        if (moveInput.magnitude > 1f)
            moveInput.Normalize();
    }

    private void Move()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    /// <summary>
    /// Retorna a direção atual do movimento (útil para animações).
    /// </summary>
    public Vector2 GetMoveDirection()
    {
        return moveInput;
    }
}
