using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hivemind : MonoBehaviour
{
    private List<Submind> subminds = new List<Submind>();

    void Start()
    {
        subminds = gameObject.GetComponents<Submind>().ToList();

        foreach (Submind smind in subminds)
        {
            smind.Initialize();
        }
    }

   
    void Update()
    {
        foreach (Submind smind in subminds)
        {
            smind.Execute();
        }
    }
}
