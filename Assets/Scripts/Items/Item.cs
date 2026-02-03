using UnityEngine;

/// <summary>
/// Representa um item que pode ser coletado e carregado pelo jogador.
/// </summary>
public class Item : MonoBehaviour
{
    [Header("Configurações do Item")]
    [SerializeField] private string itemName = "Item";
    [SerializeField] private ItemType itemType = ItemType.Generic;
    [SerializeField] private Sprite itemSprite;

    private Transform holdPoint;
    private Collider2D itemCollider;
    private SpriteRenderer spriteRenderer;
    private bool isBeingCarried;

    public bool IsBeingCarried => isBeingCarried;
    public string ItemName => itemName;
    public ItemType Type => itemType;

    private void Awake()
    {
        itemCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (itemSprite != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = itemSprite;
        }
    }

    private void Update()
    {
        // Segue o ponto de ancoragem quando está sendo carregado
        if (isBeingCarried && holdPoint != null)
        {
            transform.position = holdPoint.position;
        }
    }

    /// <summary>
    /// Chamado quando o jogador pega o item.
    /// </summary>
    public void OnPickup(Transform newHoldPoint)
    {
        isBeingCarried = true;
        holdPoint = newHoldPoint;

        // Desativa o collider para não colidir com o jogador
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }

        // Coloca o item na frente do jogador (sorting order maior)
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 10;
        }
    }

    /// <summary>
    /// Chamado quando o jogador solta o item.
    /// </summary>
    public void OnDrop(Vector2 dropPosition)
    {
        isBeingCarried = false;
        holdPoint = null;
        transform.position = dropPosition;

        // Reativa o collider
        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        // Volta ao sorting order normal
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 0;
        }
    }
}
