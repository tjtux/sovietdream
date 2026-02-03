using UnityEngine;

/// <summary>
/// Dados de configuração de uma fase. Útil para configurar transições no Inspector.
/// </summary>
[CreateAssetMenu(fileName = "LevelData", menuName = "Soviet Dream/Level Data")]
public class LevelData : ScriptableObject
{
    [Header("Identificação")]
    public string levelName;
    public string sceneName;

    [Header("Conexões")]
    public LevelConnection northConnection;
    public LevelConnection southConnection;
    public LevelConnection eastConnection;
    public LevelConnection westConnection;

    /// <summary>
    /// Retorna a conexão para uma direção específica.
    /// </summary>
    public LevelConnection GetConnection(Direction direction)
    {
        return direction switch
        {
            Direction.North => northConnection,
            Direction.South => southConnection,
            Direction.East => eastConnection,
            Direction.West => westConnection,
            _ => null
        };
    }
}

[System.Serializable]
public class LevelConnection
{
    public string targetSceneName;
    public string spawnPointId;
    public bool isLocked;
    public string requiredItemName;
}

public enum Direction
{
    North,
    South,
    East,
    West
}
