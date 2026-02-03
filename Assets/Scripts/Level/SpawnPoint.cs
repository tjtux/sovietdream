using UnityEngine;

/// <summary>
/// Marca um ponto de spawn para o jogador ao entrar em uma fase.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string spawnId;

    public string SpawnId => spawnId;

    private void OnDrawGizmos()
    {
        // Visualiza o spawn point no editor
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 0.5f);

        // Desenha uma seta indicando a direção
        Vector3 arrowEnd = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(-0.2f, -0.2f, 0));
        Gizmos.DrawLine(arrowEnd, arrowEnd + new Vector3(0.2f, -0.2f, 0));
    }
}
