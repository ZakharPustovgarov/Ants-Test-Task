using System;
using System.Collections;
using UnityEngine;

public class LoaderAnt : Ant
{
    [SerializeField] private GameObject gatheredResource;
    [SerializeField] private CircleCollider2D searchCollider;
    [SerializeField] private float gatherDistance = 0.5f;
    [SerializeField] private float gatherTime = 4;
    [SerializeField] private int gatheredResourceLimit = 1;

    private LoaderAnimationController animController => _animController as LoaderAnimationController;
    private int loadedResourceCount = 0;
    private Coroutine gatherCoroutine;

    public Action<GameObject> OnObjectDetectedEvent;
    public Action OnResourceGatheredEvent;

    protected override void Start()
    {
        base.Start();
    }

    public void Gather(IExtractable extractable)
    {
        gatherCoroutine = StartCoroutine(GatherDelay(extractable));
        animController.StartActing();
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
        OnResourceGatheredEvent?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        OnObjectDetectedEvent?.Invoke(col.gameObject);
    }

    protected override void OnDeath()
    {
        StopCoroutine(gatherCoroutine);
        base.OnDeath();
    }
}
