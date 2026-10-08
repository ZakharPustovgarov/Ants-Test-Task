using UnityEngine;

public class ResourceVein : ResourceStorage, IExtractable
{
    private void Start()
    {
        currentCapacity = maxCapacity;
    }

    public void Extract(int count = 1)
    {
        base.TakeResource(count);
    }

    protected override void OnDepletion()
    {
        base.OnDepletion();
        enabled = false;
    }
}
