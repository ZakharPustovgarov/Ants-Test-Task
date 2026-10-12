using SimpleEventBus;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LoaderSubmind : Submind
{
    [SerializeField] private AnthillStorage anthillStorage;
    [SerializeField] private float stopDistanceForVeins = 0.2f;
    [SerializeField] private float rightVeinSearchWeight = 0.7f;

    private List<LoaderAnt> unauthorizedLoaders = new List<LoaderAnt>();
    private Dictionary<LoaderAnt, LoaderInfo> loadersState = new Dictionary<LoaderAnt, LoaderInfo>();

    [SerializeField] private List<ResourceVein> hiddenVeins = new List<ResourceVein>();
    private List<ResourceVein> foundVeins = new List<ResourceVein>();
    private List<ResourceVein> depletedVeins = new List<ResourceVein>();

    private bool isVeinsDepleted = false;
    private bool isAllIdle = false;
    private int idleCount = 0;

    public override void Initialize()
    {
        unauthorizedLoaders = FindObjectsByType<LoaderAnt>(FindObjectsSortMode.None).ToList();
        EventBus.Instance.Subscribe<ResourceVeinSpawned>(OnHiddenVeinSpawned);
        PutUnauthorizedToIdle();
    }

    public override void Execute()
    {
        if (isVeinsDepleted && isAllIdle) return;

        foreach (var loader in loadersState.Keys)
        {
            LoaderInfo info;
            loadersState.TryGetValue(loader, out info);
            
            if(info == null) loadersState.Add(loader, new LoaderInfo(LoaderState.IDLE));

            if(info.state == LoaderState.IDLE) PutIdleToWork(loader, info);        
        }
    }

    public override void Destroy()
    {
        EventBus.Instance.Unsubscribe<ResourceVeinSpawned>(OnHiddenVeinSpawned);
    }

    private void PutUnauthorizedToIdle()
    {
        foreach (var loader in unauthorizedLoaders)
        {
            loadersState.Add(loader, new LoaderInfo(LoaderState.IDLE));
        }

        unauthorizedLoaders.Clear();
    }

    private void PutIdleToWork(LoaderAnt loader, LoaderInfo info)
    {
        ResourceVein desirableVein = TryGetHiddenVein();

        if (desirableVein != null)
        {
            AssignSearch(loader, info, desirableVein);
            return;
        }

        if (foundVeins.Count == 0)
        {
            isVeinsDepleted = true;
            if (++idleCount == loadersState.Count) isAllIdle = true;
            return;
        }

        AssignMoveToGather(loader, info);
    }

    private ResourceVein TryGetHiddenVein()
    {
        if (hiddenVeins.Count == 0) return null;

        int allVeins = hiddenVeins.Count + foundVeins.Count + depletedVeins.Count;
        int notHiddenVeins = allVeins - hiddenVeins.Count;
        int chooseWeight = notHiddenVeins == 1 ? 0 : notHiddenVeins / allVeins;

        if(Random.value < chooseWeight)
        {
            return null;
        }

        return MathUtility.GetRandomElement(hiddenVeins);
    }

    private void AssignSearch(LoaderAnt loader, LoaderInfo info, ResourceVein vein)
    {
        SearchResourceState searchState = new SearchResourceState(vein, rightVeinSearchWeight);
        loader.ChangeState(searchState);
        info.unitState = searchState;
        info.state = LoaderState.SEARCH;

        //Debug.Log($"Assigned search to {loader.gameObject.name}");

        searchState.OnResourceFoundEvent += OnResourceFound;
    }

    private void OnResourceFound(Creature unit, ResourceVein vein)
    {
        LoaderInfo info;
        LoaderAnt loader = unit as LoaderAnt;
        loadersState.TryGetValue(loader, out info);
        //Debug.Log($"{loader.gameObject.name} found {vein.gameObject.name}");
        OnVeinFound(vein);

        (info.unitState as SearchResourceState).OnResourceFoundEvent -= OnResourceFound;

        AssignMoveToGather(loader, info, vein);
    }

    private void AssignMoveToGather(LoaderAnt loader, LoaderInfo info, ResourceVein vein = null)
    {
        ResourceVein targetVein = vein == null ? MathUtility.GetRandomElement(foundVeins) : vein;

        MoveToPositionState moveState = new MoveToPositionState(targetVein.gameObject.transform.position, stopDistanceForVeins);
        loader.ChangeState(moveState);
        info.unitState = moveState;
        info.assignedVein = targetVein;
        info.state = LoaderState.MOVE_TO_RESOURCE;
        loader.StartMovementAnimation();
        //Debug.Log($"{loader.gameObject.name} moving to {targetVein.gameObject.name} to position {targetVein.transform.position}");

        moveState.OnTargetReachedEvent += OnResourceReached;
    }

    private void OnResourceReached(IMovable unit)
    {
        LoaderInfo info;
        LoaderAnt loader = unit as LoaderAnt;
        loadersState.TryGetValue(loader, out info);
        //Debug.Log($"{loader.gameObject.name} reached {info.assignedVein.gameObject.name}");
        (info.unitState as MoveToPositionState).OnTargetReachedEvent -= OnResourceReached;
        loader.StopMovementAnimation();
        AssignGather(loader, info, info.assignedVein);
    }

    private void AssignGather(LoaderAnt loader, LoaderInfo info, ResourceVein vein)
    {
        GatherResourceState gatherState = new GatherResourceState(vein);
        loader.ChangeState(gatherState);
        info.unitState = gatherState;
        info.state = LoaderState.GATHER;
        //Debug.Log($"{loader.gameObject.name} began to gather from {vein.gameObject.name}");
        gatherState.OnResourceGatheredEvent += OnResourceGathered;
    }

    private void OnResourceGathered(Creature unit)
    {
        LoaderInfo info;
        LoaderAnt loader = unit as LoaderAnt;
        loadersState.TryGetValue(loader, out info);
        //Debug.Log($"{loader.gameObject.name} ended gathering from {info.assignedVein.gameObject.name}");
        (info.unitState as GatherResourceState).OnResourceGatheredEvent -= OnResourceGathered;

        if (info.assignedVein.CurrentCapacity == 0) OnVeinDepleted(info.assignedVein);

        AssignMoveToAnthill(loader, info);
    }

    private void AssignMoveToAnthill(LoaderAnt loader, LoaderInfo info)
    {
        MoveToPositionState moveState = new MoveToPositionState(anthillStorage.transform.position);
        loader.ChangeState(moveState);
        info.unitState = moveState;
        info.state = LoaderState.MOVE_TO_ANTHILL;
        loader.StartMovementAnimation();
        //Debug.Log($"{loader.gameObject.name} moving to anthill");
        moveState.OnTargetReachedEvent += OnAnthillReached;
    }

    private void OnAnthillReached(IMovable unit)
    {
        LoaderInfo info;
        LoaderAnt loader = unit as LoaderAnt;
        loadersState.TryGetValue(loader, out info);
        //Debug.Log($"{loader.gameObject.name} reached to anthill");
        loader.StopMovementAnimation();
        (info.unitState as MoveToPositionState).OnTargetReachedEvent -= OnAnthillReached;

        AssignUnload(loader, info);
    }

    private void AssignUnload(LoaderAnt loader, LoaderInfo info)
    {
        UnloadResourceState unloadState = new UnloadResourceState(anthillStorage);
        loader.ChangeState(unloadState);
        info.unitState = unloadState;
        info.state = LoaderState.UNLOAD;
        //Debug.Log($"{loader.gameObject.name} began to unload to anthill");
        unloadState.OnResourceUnloadedEvent += OnUnloaded;
    }

    private void OnUnloaded(Creature unit)
    {
        LoaderInfo info;
        LoaderAnt loader = unit as LoaderAnt;
        loadersState.TryGetValue(loader, out info);
        //Debug.Log($"{loader.gameObject.name} ended unloading to anthill");
        (info.unitState as UnloadResourceState).OnResourceUnloadedEvent -= OnUnloaded;

        info.state = LoaderState.IDLE;
        info.unitState = new IdleState();
        info.assignedVein = null;
    }

    private void OnHiddenVeinSpawned(ResourceVeinSpawned veinSpawned)
    {
        hiddenVeins.Add(veinSpawned.Vein);
        isVeinsDepleted = false;
        isAllIdle = false;
    }

    private void OnVeinDepleted(ResourceVein vein)
    {
        foundVeins.Remove(vein);
        depletedVeins.Add(vein);
    }

    private void OnVeinFound(ResourceVein vein)
    {
        hiddenVeins.Remove(vein);
        foundVeins.Add(vein);
    }


    private enum LoaderState
    {
        IDLE,
        SEARCH,
        MOVE_TO_RESOURCE,
        GATHER,
        MOVE_TO_ANTHILL,
        UNLOAD
    }

    private class LoaderInfo
    {
        public LoaderInfo(LoaderState state)
        {
            this.state = state;
        }

        public LoaderState state;
        public UnitState unitState;
        public ResourceVein assignedVein;
    }
}
