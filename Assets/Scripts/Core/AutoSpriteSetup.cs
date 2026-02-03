using UnityEngine;

/// <summary>
/// Configura sprites automaticamente para todos os objetos sem sprite.
/// Adicione este script a um GameObject vazio na cena.
/// </summary>
public class AutoSpriteSetup : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private int spriteSize = 32;
    [SerializeField] private Color playerBodyColor = new Color(0.3f, 0.5f, 0.3f);
    [SerializeField] private Color playerHeadColor = new Color(0.9f, 0.75f, 0.6f);
    [SerializeField] private Color carBodyColor = new Color(0.6f, 0.2f, 0.2f);
    [SerializeField] private Color groundColor = new Color(0.3f, 0.3f, 0.25f);

    private void Awake()
    {
        SetupAllSprites();
    }

    private void SetupAllSprites()
    {
        SetupPlayer();
        SetupItems();
        SetupCar();
        SetupMap();
    }

    private void SetupPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        if (sr != null && sr.sprite == null)
        {
            sr.sprite = CreateCharacterSprite(spriteSize, spriteSize * 2, playerBodyColor, playerHeadColor);
            sr.sortingOrder = 5;
        }
    }

    private void SetupItems()
    {
        Item[] items = FindObjectsByType<Item>(FindObjectsSortMode.None);
        foreach (var item in items)
        {
            SpriteRenderer sr = item.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = CreateItemSprite(item.Type);
                sr.sortingOrder = 2;
            }
        }
    }

    private void SetupCar()
    {
        Car[] cars = FindObjectsByType<Car>(FindObjectsSortMode.None);
        foreach (var car in cars)
        {
            SpriteRenderer sr = car.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = CreateCarSprite();
                sr.sortingOrder = 1;
            }
        }
    }

    private void SetupMap()
    {
        // Procura por objeto chamado "Map" e configura um fundo
        GameObject map = GameObject.Find("Map");
        if (map != null)
        {
            SpriteRenderer sr = map.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite == null)
            {
                sr.sprite = CreateGroundSprite(256);
                sr.sortingOrder = -10;
            }
        }
    }

    private Sprite CreateCharacterSprite(int width, int height, Color bodyColor, Color headColor)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        // Limpa com transparente
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        int headHeight = height / 3;
        int bodyWidth = width * 2 / 3;
        int bodyStartX = (width - bodyWidth) / 2;

        // Corpo (retângulo)
        for (int y = 0; y < height - headHeight; y++)
        {
            for (int x = bodyStartX; x < bodyStartX + bodyWidth; x++)
            {
                pixels[y * width + x] = bodyColor;
            }
        }

        // Cabeça (círculo)
        int headRadius = headHeight / 2;
        int headCenterX = width / 2;
        int headCenterY = height - headRadius - 2;

        for (int y = height - headHeight; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float dx = x - headCenterX;
                float dy = y - headCenterY;
                if (dx * dx + dy * dy <= headRadius * headRadius)
                {
                    pixels[y * width + x] = headColor;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.25f), spriteSize);
    }

    private Sprite CreateItemSprite(ItemType type)
    {
        int size = spriteSize;
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];

        // Limpa
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        Color itemColor = type switch
        {
            ItemType.Key => new Color(0.9f, 0.8f, 0.2f), // Dourado
            ItemType.GasCan => new Color(0.8f, 0.2f, 0.2f), // Vermelho
            ItemType.Tire => new Color(0.2f, 0.2f, 0.2f), // Preto
            ItemType.Battery => new Color(0.1f, 0.4f, 0.1f), // Verde escuro
            ItemType.AntiRadPills => new Color(1f, 1f, 1f), // Branco
            ItemType.Vodka => new Color(0.7f, 0.85f, 0.7f), // Verde claro
            _ => Color.magenta
        };

        // Desenha um círculo/quadrado simples
        int margin = size / 6;
        for (int y = margin; y < size - margin; y++)
        {
            for (int x = margin; x < size - margin; x++)
            {
                pixels[y * size + x] = itemColor;
            }
        }

        // Adiciona borda mais escura
        Color borderColor = itemColor * 0.6f;
        borderColor.a = 1f;
        for (int y = margin; y < size - margin; y++)
        {
            pixels[y * size + margin] = borderColor;
            pixels[y * size + size - margin - 1] = borderColor;
        }
        for (int x = margin; x < size - margin; x++)
        {
            pixels[margin * size + x] = borderColor;
            pixels[(size - margin - 1) * size + x] = borderColor;
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private Sprite CreateCarSprite()
    {
        int width = spriteSize * 2;
        int height = spriteSize;
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        // Limpa
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        // Corpo do carro
        int margin = 4;
        int wheelHeight = height / 4;

        for (int y = wheelHeight; y < height - margin; y++)
        {
            for (int x = margin; x < width - margin; x++)
            {
                pixels[y * width + x] = carBodyColor;
            }
        }

        // Rodas
        Color wheelColor = new Color(0.15f, 0.15f, 0.15f);
        int wheelRadius = wheelHeight / 2;

        // Roda esquerda
        int wheel1X = width / 4;
        int wheelY = wheelRadius + 2;
        // Roda direita
        int wheel2X = width * 3 / 4;

        for (int dy = -wheelRadius; dy <= wheelRadius; dy++)
        {
            for (int dx = -wheelRadius; dx <= wheelRadius; dx++)
            {
                if (dx * dx + dy * dy <= wheelRadius * wheelRadius)
                {
                    int px1 = wheel1X + dx;
                    int py1 = wheelY + dy;
                    int px2 = wheel2X + dx;

                    if (px1 >= 0 && px1 < width && py1 >= 0 && py1 < height)
                        pixels[py1 * width + px1] = wheelColor;
                    if (px2 >= 0 && px2 < width && py1 >= 0 && py1 < height)
                        pixels[py1 * width + px2] = wheelColor;
                }
            }
        }

        // Janelas
        Color windowColor = new Color(0.3f, 0.5f, 0.7f);
        int windowY = height * 2 / 3;
        int windowHeight = height / 5;
        int windowMargin = width / 5;

        for (int y = windowY; y < windowY + windowHeight && y < height - margin; y++)
        {
            for (int x = windowMargin; x < width - windowMargin; x++)
            {
                pixels[y * width + x] = windowColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), spriteSize);
    }

    private Sprite CreateGroundSprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];

        // Cor base do chão
        for (int i = 0; i < pixels.Length; i++)
        {
            // Adiciona variação para textura
            float noise = Random.Range(0.9f, 1.1f);
            pixels[i] = groundColor * noise;
            pixels[i].a = 1f;
        }

        // Adiciona linhas de estrada (cruz)
        Color roadColor = new Color(0.2f, 0.2f, 0.2f);
        int roadWidth = size / 4;
        int center = size / 2;

        // Estrada horizontal
        for (int y = center - roadWidth / 2; y < center + roadWidth / 2; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (y >= 0 && y < size)
                    pixels[y * size + x] = roadColor;
            }
        }

        // Estrada vertical
        for (int y = 0; y < size; y++)
        {
            for (int x = center - roadWidth / 2; x < center + roadWidth / 2; x++)
            {
                if (x >= 0 && x < size)
                    pixels[y * size + x] = roadColor;
            }
        }

        // Linhas amarelas no centro das estradas
        Color lineColor = new Color(0.9f, 0.8f, 0.2f);
        int lineWidth = 2;

        for (int y = 0; y < size; y++)
        {
            for (int x = center - lineWidth / 2; x < center + lineWidth / 2; x++)
            {
                if (y % 20 < 15) // Linha tracejada
                    pixels[y * size + x] = lineColor;
            }
        }

        for (int y = center - lineWidth / 2; y < center + lineWidth / 2; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x % 20 < 15)
                    pixels[y * size + x] = lineColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 32);
    }
}
