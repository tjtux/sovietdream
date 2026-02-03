/// <summary>
/// Interface para objetos interagíveis.
/// </summary>
public interface IInteractable
{
    bool CanInteract();
    void Interact();
    void OnPlayerEnter();
    void OnPlayerExit();
}
