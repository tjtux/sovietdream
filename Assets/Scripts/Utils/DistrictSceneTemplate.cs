using UnityEngine;

/// <summary>
/// Template para criar quarteirões em runtime.
/// Use este script na cena para gerar a estrutura básica do quarteirão.
/// </summary>
public class DistrictSceneTemplate : MonoBehaviour
{
    [Header("Configuração do Distrito")]
    [SerializeField] private string districtName = "District";
    [SerializeField] private ItemType carPartType = ItemType.Key;
    [SerializeField] private Vector2 partSpawnPosition = new Vector2(3, 3);
    [SerializeField] private Vector2 consumableSpawnPosition1 = new Vector2(-2, 2);
    [SerializeField] private Vector2 consumableSpawnPosition2 = new Vector2(2, -2);

    [Header("Zona de Retorno")]
    [SerializeField] private string returnSceneName = "Crossroads";
    [SerializeField] private Vector2 exitZonePosition = new Vector2(0, -5);

    private void Start()
    {
        SetupDistrict();
    }

    private void SetupDistrict()
    {
        // Cria a peça do carro se ainda não existir na cena
        if (!HasItemOfType(carPartType))
        {
            CreateCarPart();
        }

        // Cria consumíveis
        CreateConsumables();

        // Cria zona de saída se não existir
        if (FindFirstObjectByType<InteractionZone>() == null)
        {
            CreateExitZone();
        }
    }

    private bool HasItemOfType(ItemType type)
    {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            if (item.Type == type) return true;
        }
        return false;
    }

    private void CreateCarPart()
    {
        GameObject partObj = new GameObject(GetPartName(carPartType));
        partObj.transform.position = partSpawnPosition;
        partObj.layer = 3; // Item layer

        SpriteRenderer sr = partObj.AddComponent<SpriteRenderer>();
        sr.sprite = PlaceholderSpriteGenerator.CreateItemSprite(carPartType, 32);
        sr.sortingOrder = 2;

        CircleCollider2D col = partObj.AddComponent<CircleCollider2D>();
        col.radius = 0.4f;

        Item item = partObj.AddComponent<Item>();
        // Configura via reflection ou SerializedField
        SetItemType(item, carPartType);
    }

    private void CreateConsumables()
    {
        // Pílulas anti-radiação
        if (Random.value > 0.3f) // 70% de chance
        {
            CreateConsumable(ItemType.AntiRadPills, consumableSpawnPosition1, "AntiRadPills", 30f);
        }

        // Vodka
        if (Random.value > 0.5f) // 50% de chance
        {
            CreateConsumable(ItemType.Vodka, consumableSpawnPosition2, "Vodka", 20f);
        }
    }

    private void CreateConsumable(ItemType type, Vector2 position, string name, float radiationReduction)
    {
        GameObject obj = new GameObject(name);
        obj.transform.position = position;
        obj.layer = 3;

        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = PlaceholderSpriteGenerator.CreateItemSprite(type, 32);
        sr.sortingOrder = 2;

        CircleCollider2D col = obj.AddComponent<CircleCollider2D>();
        col.radius = 0.3f;

        ConsumableItem consumable = obj.AddComponent<ConsumableItem>();
        SetItemType(consumable, type);
    }

    private void CreateExitZone()
    {
        GameObject zoneObj = new GameObject("ExitZone");
        zoneObj.transform.position = exitZonePosition;

        BoxCollider2D col = zoneObj.AddComponent<BoxCollider2D>();
        col.size = new Vector2(3, 1);
        col.isTrigger = true;

        InteractionZone zone = zoneObj.AddComponent<InteractionZone>();
        // Configuração via SerializedField necessita ser feita no editor
        // ou via reflection
    }

    private string GetPartName(ItemType type)
    {
        return type switch
        {
            ItemType.Key => "Chave",
            ItemType.GasCan => "Galao",
            ItemType.Tire => "Pneu",
            ItemType.Battery => "Bateria",
            _ => "Item"
        };
    }

    private void SetItemType(Item item, ItemType type)
    {
        // Usa reflection para setar o campo privado
        var field = typeof(Item).GetField("itemType",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(item, type);

        var nameField = typeof(Item).GetField("itemName",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        nameField?.SetValue(item, GetPartName(type));
    }
}
