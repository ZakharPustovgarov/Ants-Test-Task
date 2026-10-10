using UnityEngine;

public class LoaderAnt : Ant
{
    [SerializeField] private LoaderAnimationController animController;
    [SerializeField] private GameObject gatheredResource;
    [SerializeField] private float searchRadius = 3f;
    [SerializeField] private int gatheredResourceLimit = 1;

    private int loadedResourceCount = 0;

    public float SearchRadius { get { return searchRadius; } }

    public void Gather(IExtractable extractable)
    {
        loadedResourceCount = extractable.Extract(gatheredResourceLimit);
        gatheredResource.SetActive(true);
    }

    public void Unload(IInsertable insertable)
    {
        insertable.Insert(loadedResourceCount);
        gatheredResource.SetActive(false);
    }

    protected override void StartDeathAnimation()
    {
        animController.Die();
    }
}
