using UnityEngine;

public class ResourceVein : ResourceStorage, IExtractable
{
    private void Start()
    {
        currentCapacity = maxCapacity;
    }

    public int Extract(int count = 1)
    {
        return base.TakeResource(count);
    }

    protected override void OnDepletion()
    {
        base.OnDepletion();
        this.gameObject.SetActive(false);
    }
}
