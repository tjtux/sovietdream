using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Gerencia a saúde e radiação do jogador.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    [Header("Saúde")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    [Header("Radiação")]
    [SerializeField] private float maxRadiation = 100f;
    [SerializeField] private float currentRadiation = 0f;
    [SerializeField] private float radiationGainPerSecond = 2f;
    [SerializeField] private float radiationDamageThreshold = 50f;
    [SerializeField] private float radiationDamagePerSecond = 5f;

    [Header("Eventos")]
    public UnityEvent<float, float> OnHealthChanged; // current, max
    public UnityEvent<float, float> OnRadiationChanged; // current, max
    public UnityEvent OnPlayerDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float CurrentRadiation => currentRadiation;
    public float MaxRadiation => maxRadiation;
    public float HealthPercent => currentHealth / maxHealth;
    public float RadiationPercent => currentRadiation / maxRadiation;
    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentRadiation = 0f;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnRadiationChanged?.Invoke(currentRadiation, maxRadiation);
    }

    private void Update()
    {
        if (IsDead) return;

        // Acumula radiação ao longo do tempo
        AddRadiation(radiationGainPerSecond * Time.deltaTime);

        // Dano de radiação se estiver acima do threshold
        if (currentRadiation >= radiationDamageThreshold)
        {
            float damageMultiplier = (currentRadiation - radiationDamageThreshold) / (maxRadiation - radiationDamageThreshold);
            TakeDamage(radiationDamagePerSecond * damageMultiplier * Time.deltaTime);
        }
    }

    /// <summary>
    /// Causa dano ao jogador.
    /// </summary>
    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Cura o jogador.
    /// </summary>
    public void Heal(float amount)
    {
        if (IsDead || amount <= 0) return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    /// <summary>
    /// Adiciona radiação ao jogador.
    /// </summary>
    public void AddRadiation(float amount)
    {
        if (IsDead || amount <= 0) return;

        float previousRadiation = currentRadiation;
        currentRadiation = Mathf.Min(maxRadiation, currentRadiation + amount);

        if (Mathf.Abs(currentRadiation - previousRadiation) > 0.01f)
        {
            OnRadiationChanged?.Invoke(currentRadiation, maxRadiation);
        }
    }

    /// <summary>
    /// Reduz a radiação do jogador.
    /// </summary>
    public void ReduceRadiation(float amount)
    {
        if (amount <= 0) return;

        currentRadiation = Mathf.Max(0, currentRadiation - amount);
        OnRadiationChanged?.Invoke(currentRadiation, maxRadiation);
    }

    /// <summary>
    /// Chamado quando o jogador morre.
    /// </summary>
    private void Die()
    {
        OnPlayerDeath?.Invoke();
        AudioManager.Instance?.PlayDeathSound();

        // Recarrega a cena após um delay
        Invoke(nameof(ReloadScene), 2f);
    }

    private void ReloadScene()
    {
        GameManager.Instance?.RestartGame();
    }

    /// <summary>
    /// Reseta a saúde e radiação (para reiniciar o jogo).
    /// </summary>
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        currentRadiation = 0f;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnRadiationChanged?.Invoke(currentRadiation, maxRadiation);
    }
}
