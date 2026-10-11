using System;
using System.Collections;
using UnityEngine;

public class LoaderAnt : Ant
{
    [SerializeField] private LoaderAnimationController animController;
    [SerializeField] private GameObject gatheredResource;
    [SerializeField] private CircleCollider2D searchCollider;
    [SerializeField] private float gatherDistance = 0.5f;
    [SerializeField] private float gatherTime = 4;
    [SerializeField] private int gatheredResourceLimit = 1;

    public LoaderAnimationController AnimController { get { return animController; } }
    private int loadedResourceCount = 0;
    private Coroutine gatherCoroutine;

    public Action<GameObject> OnObjectDetected;

    protected override void Start()
    {
        base.Start();
    }

    public void Gather(IExtractable extractable)
    {
        gatherCoroutine = StartCoroutine(GatherDelay(extractable));
        animController.BeginActing();
    }

    public void Unload(IInsertable insertable)
    {
        loadedResourceCount -= insertable.Insert(loadedResourceCount);

        if(loadedResourceCount == 0)
            gatheredResource.SetActive(false);
    }

    private IEnumerator GatherDelay(IExtractable extractable)
    {
        yield return new WaitForSeconds(gatherTime);

        loadedResourceCount = extractable.Extract(gatheredResourceLimit);
        gatheredResource.SetActive(true);
        animController.StopActing();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        OnObjectDetected?.Invoke(col.gameObject);
    }

    protected override void StartDeathAnimation()
    {
        animController.Die();
    }

    protected override void OnDeath()
    {
        StopCoroutine(gatherCoroutine);
        base.OnDeath();
    }
}
