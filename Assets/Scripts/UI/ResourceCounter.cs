using SimpleEventBus;
using UnityEngine;
using UnityEngine.UI;

public class ResourceCounter : MonoBehaviour
{
    [SerializeField] private Text text;

    private bool isAwaken = false;

    private void Awake()
    {
        if (isAwaken) return;
        isAwaken = true;
        EventBus.Instance.Subscribe<AnthillStorageCapacityChanged>(OnAnthillStorageCurrentCapacityChanged);
    }

    private void OnAnthillStorageCurrentCapacityChanged(AnthillStorageCapacityChanged @event)
    {
        text.text = $"{@event.CurrentCapacity}/{@event.MaxCapacity}";
    }

    private void OnDestroy()
    {
        EventBus.Instance.Unsubscribe<AnthillStorageCapacityChanged>(OnAnthillStorageCurrentCapacityChanged);
    }
}

