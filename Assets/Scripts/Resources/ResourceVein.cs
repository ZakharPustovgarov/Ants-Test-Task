using UnityEngine;

public class ResourceVein : MonoBehaviour
{
    [SerializeField] private int maxCapacity;
    private int currentCapacity;

    private void Start()
    {
        currentCapacity = maxCapacity;
    }

    public int TakeResource(int count = 1)
    {
        if(currentCapacity - count > 0)
        {
            currentCapacity -= count;
            return count;
        }

        int buf = currentCapacity;
        OnDepletion();
        return buf;
    }

    private void OnDepletion()
    {
        currentCapacity = 0;
        enabled = false;
    }
}
