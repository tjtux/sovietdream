using UnityEngine;

/// <summary>
/// Faz a câmera seguir o jogador suavemente.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Limites (Opcional)")]
    [SerializeField] private bool useBounds;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minY = -10f;
    [SerializeField] private float maxY = 10f;

    private void Start()
    {
        // Tenta encontrar o player se não estiver atribuído
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        // Aplica limites se habilitado
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Suaviza o movimento
        Vector3 smoothedPosition = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.position = smoothedPosition;
    }

    /// <summary>
    /// Define um novo alvo para a câmera.
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    /// <summary>
    /// Move a câmera instantaneamente para o alvo (útil após transição de fase).
    /// </summary>
    public void SnapToTarget()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (useBounds)
        {
            Gizmos.color = Color.yellow;
            Vector3 center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
            Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);
            Gizmos.DrawWireCube(center, size);
        }
    }
}
