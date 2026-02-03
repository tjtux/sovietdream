using UnityEngine;

/// <summary>
/// Gera sprites placeholder em runtime para teste.
/// Pode ser removido quando os assets finais forem adicionados.
/// </summary>
public static class PlaceholderSpriteGenerator
{
    /// <summary>
    /// Cria um sprite quadrado de cor sólida.
    /// </summary>
    public static Sprite CreateColoredSquare(int size, Color color)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    /// <summary>
    /// Cria um sprite de círculo.
    /// </summary>
    public static Sprite CreateCircle(int size, Color color)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        float radius = size / 2f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float distance = Vector2.Distance(new Vector2(x, y), center);
                if (distance < radius)
                {
                    pixels[y * size + x] = color;
                }
                else
                {
                    pixels[y * size + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    /// <summary>
    /// Cria um sprite de personagem (retângulo com "cabeça").
    /// </summary>
    public static Sprite CreateCharacter(int width, int height, Color bodyColor, Color headColor)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        int headHeight = height / 4;
        int bodyStart = headHeight;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (y >= height - headHeight)
                {
                    // Cabeça (círculo aproximado)
                    int headCenterX = width / 2;
                    int headCenterY = height - headHeight / 2;
                    float dist = Vector2.Distance(new Vector2(x, y), new Vector2(headCenterX, headCenterY));
                    if (dist < headHeight / 2f)
                    {
                        pixels[y * width + x] = headColor;
                    }
                    else
                    {
                        pixels[y * width + x] = Color.clear;
                    }
                }
                else
                {
                    // Corpo
                    int margin = width / 4;
                    if (x >= margin && x < width - margin)
                    {
                        pixels[y * width + x] = bodyColor;
                    }
                    else
                    {
                        pixels[y * width + x] = Color.clear;
                    }
                }
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), Mathf.Max(width, height));
    }

    /// <summary>
    /// Cria sprite de carro (retângulo com rodas).
    /// </summary>
    public static Sprite CreateCar(int width, int height, Color bodyColor, Color wheelColor)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        // Limpa tudo
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }

        int wheelSize = height / 4;
        int bodyHeight = height - wheelSize;

        // Corpo do carro
        for (int y = wheelSize; y < height; y++)
        {
            for (int x = 2; x < width - 2; x++)
            {
                pixels[y * width + x] = bodyColor;
            }
        }

        // Rodas
        int wheel1X = width / 4;
        int wheel2X = width - width / 4;
        int wheelY = wheelSize / 2;

        for (int dy = -wheelSize / 2; dy <= wheelSize / 2; dy++)
        {
            for (int dx = -wheelSize / 2; dx <= wheelSize / 2; dx++)
            {
                if (dx * dx + dy * dy <= (wheelSize / 2) * (wheelSize / 2))
                {
                    int px1 = wheel1X + dx;
                    int py1 = wheelY + dy;
                    int px2 = wheel2X + dx;
                    int py2 = wheelY + dy;

                    if (px1 >= 0 && px1 < width && py1 >= 0 && py1 < height)
                        pixels[py1 * width + px1] = wheelColor;
                    if (px2 >= 0 && px2 < width && py2 >= 0 && py2 < height)
                        pixels[py2 * width + px2] = wheelColor;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), Mathf.Max(width, height));
    }

    /// <summary>
    /// Cria sprite de item (forma baseada no tipo).
    /// </summary>
    public static Sprite CreateItemSprite(ItemType type, int size = 32)
    {
        return type switch
        {
            ItemType.Key => CreateKeySprite(size),
            ItemType.GasCan => CreateGasCanSprite(size),
            ItemType.Tire => CreateTireSprite(size),
            ItemType.Battery => CreateBatterySprite(size),
            ItemType.AntiRadPills => CreatePillsSprite(size),
            ItemType.Vodka => CreateVodkaSprite(size),
            _ => CreateColoredSquare(size, Color.magenta)
        };
    }

    private static Sprite CreateKeySprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Color keyColor = new Color(0.8f, 0.7f, 0.2f); // Dourado

        // Limpa
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        // Cabeça da chave (círculo)
        int headRadius = size / 4;
        int headX = size / 4;
        int headY = size / 2;

        for (int dy = -headRadius; dy <= headRadius; dy++)
        {
            for (int dx = -headRadius; dx <= headRadius; dx++)
            {
                if (dx * dx + dy * dy <= headRadius * headRadius)
                {
                    int px = headX + dx;
                    int py = headY + dy;
                    if (px >= 0 && px < size && py >= 0 && py < size)
                    {
                        // Faz um buraco no meio
                        if (dx * dx + dy * dy > (headRadius / 2) * (headRadius / 2))
                            pixels[py * size + px] = keyColor;
                    }
                }
            }
        }

        // Haste da chave
        for (int x = headX + headRadius; x < size - 2; x++)
        {
            int y = size / 2;
            pixels[y * size + x] = keyColor;
            pixels[(y - 1) * size + x] = keyColor;
        }

        // Dentes
        for (int i = 0; i < 2; i++)
        {
            int toothX = size - 6 - i * 4;
            for (int dy = 0; dy < 4; dy++)
            {
                int py = size / 2 - 1 - dy;
                if (py >= 0)
                    pixels[py * size + toothX] = keyColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private static Sprite CreateGasCanSprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Color canColor = Color.red;
        Color capColor = Color.black;

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        // Corpo do galão
        int margin = size / 6;
        for (int y = margin; y < size - margin - 4; y++)
        {
            for (int x = margin; x < size - margin; x++)
            {
                pixels[y * size + x] = canColor;
            }
        }

        // Tampa
        int capWidth = size / 4;
        int capStart = size / 2 - capWidth / 2;
        for (int y = size - margin - 4; y < size - 2; y++)
        {
            for (int x = capStart; x < capStart + capWidth; x++)
            {
                pixels[y * size + x] = capColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private static Sprite CreateTireSprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Color tireColor = new Color(0.2f, 0.2f, 0.2f);
        Color rimColor = new Color(0.6f, 0.6f, 0.6f);

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        int center = size / 2;
        int outerRadius = size / 2 - 2;
        int innerRadius = size / 4;

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int dx = x - center;
                int dy = y - center;
                int distSq = dx * dx + dy * dy;

                if (distSq <= outerRadius * outerRadius && distSq >= innerRadius * innerRadius)
                {
                    pixels[y * size + x] = tireColor;
                }
                else if (distSq < innerRadius * innerRadius && distSq >= (innerRadius / 2) * (innerRadius / 2))
                {
                    pixels[y * size + x] = rimColor;
                }
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private static Sprite CreateBatterySprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Color batteryColor = new Color(0.1f, 0.1f, 0.1f);
        Color terminalColor = new Color(0.6f, 0.6f, 0.6f);

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        // Corpo da bateria
        int margin = size / 6;
        for (int y = margin; y < size - margin - 4; y++)
        {
            for (int x = margin; x < size - margin; x++)
            {
                pixels[y * size + x] = batteryColor;
            }
        }

        // Terminais
        int termWidth = size / 6;
        for (int y = size - margin - 4; y < size - 2; y++)
        {
            // Terminal esquerdo
            for (int x = margin + 2; x < margin + 2 + termWidth; x++)
            {
                pixels[y * size + x] = terminalColor;
            }
            // Terminal direito
            for (int x = size - margin - 2 - termWidth; x < size - margin - 2; x++)
            {
                pixels[y * size + x] = terminalColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private static Sprite CreatePillsSprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Color pillColor = Color.white;
        Color bottleColor = new Color(0.8f, 0.5f, 0.2f);

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        // Frasco
        int margin = size / 4;
        for (int y = 2; y < size - margin; y++)
        {
            for (int x = margin; x < size - margin; x++)
            {
                pixels[y * size + x] = bottleColor;
            }
        }

        // Tampa
        for (int y = size - margin; y < size - 2; y++)
        {
            for (int x = margin + 2; x < size - margin - 2; x++)
            {
                pixels[y * size + x] = pillColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }

    private static Sprite CreateVodkaSprite(int size)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] pixels = new Color[size * size];
        Color bottleColor = new Color(0.8f, 0.9f, 0.8f, 0.8f);
        Color capColor = Color.red;
        Color labelColor = Color.red;

        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        // Corpo da garrafa
        int marginX = size / 4;
        int marginBottom = 2;
        int neckStart = size - size / 3;

        for (int y = marginBottom; y < neckStart; y++)
        {
            for (int x = marginX; x < size - marginX; x++)
            {
                pixels[y * size + x] = bottleColor;
            }
        }

        // Gargalo
        int neckMargin = size / 3;
        for (int y = neckStart; y < size - 4; y++)
        {
            for (int x = neckMargin; x < size - neckMargin; x++)
            {
                pixels[y * size + x] = bottleColor;
            }
        }

        // Tampa
        for (int y = size - 4; y < size - 1; y++)
        {
            for (int x = neckMargin; x < size - neckMargin; x++)
            {
                pixels[y * size + x] = capColor;
            }
        }

        // Rótulo vermelho
        int labelY = size / 3;
        for (int y = labelY; y < labelY + 6; y++)
        {
            for (int x = marginX + 2; x < size - marginX - 2; x++)
            {
                if (y < size && x < size)
                    pixels[y * size + x] = labelColor;
            }
        }

        texture.SetPixels(pixels);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
    }
}
