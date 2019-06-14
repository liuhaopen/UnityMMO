using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SystemCollection
{
    List<ComponentSystemBase> systems = new List<ComponentSystemBase>();

    public void Add(ComponentSystemBase system)
    {
        systems.Add(system);
    }

    public void Update()
    {
        foreach (var system in systems)
            system.Update();
    }

    public void Shutdown(World world)
    {
        foreach (var system in systems)
            world.DestroySystem(system);
    }
}
