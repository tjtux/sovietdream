using UnityEngine;

/// <summary>
/// Item consumível que pode ser usado para restaurar saúde ou reduzir radiação.
/// </summary>
public class ConsumableItem : Item
{
    [Header("Configurações do Consumível")]
    [SerializeField] private float radiationReduction = 30f;
    [SerializeField] private float healthRestore = 0f;

    public float RadiationReduction => radiationReduction;
    public float HealthRestore => healthRestore;

    /// <summary>
    /// Usa o consumível no jogador.
    /// </summary>
    public void Use(PlayerHealth playerHealth)
    {
        if (playerHealth == null) return;

        if (radiationReduction > 0)
        {
            playerHealth.ReduceRadiation(radiationReduction);
        }

        if (healthRestore > 0)
        {
            playerHealth.Heal(healthRestore);
        }
    }
}
