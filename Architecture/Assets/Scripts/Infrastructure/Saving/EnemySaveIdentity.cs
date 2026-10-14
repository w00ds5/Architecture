using UnityEngine;

[RequireComponent(
    typeof(UnityCharacterSaveState),
    typeof(EnemyTargetSensor))]
public sealed class EnemySaveIdentity : MonoBehaviour
{
    [SerializeField] private EnemyKind kind;

    private UnityCharacterSaveState _state;

    public EnemyKind Kind => kind;
    public ICharacterSaveState State => _state;

    private void Awake()
    {
        _state = GetComponent<UnityCharacterSaveState>();
    }
}