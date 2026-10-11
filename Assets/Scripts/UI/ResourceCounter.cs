using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private AnthillStorage anthillStorage;

    private void Start()
    {
        anthillStorage.OnAnthillStorageCurrentCapacityChangedEvent += OnAnthillStorageCurrentCapacityChanged;
        OnAnthillStorageCurrentCapacityChanged(anthillStorage.CurrentCapacity, anthillStorage.MaxCapacity);
    }

    private void OnAnthillStorageCurrentCapacityChanged(int current, int max)
    {
        text.text = $"{current}/{max}";
    }

    private void OnDestroy()
    {
        anthillStorage.OnAnthillStorageCurrentCapacityChangedEvent -= OnAnthillStorageCurrentCapacityChanged;
    }
}
