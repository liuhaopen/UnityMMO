using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class SystemCollection
{
    List<ScriptBehaviourManager> systems = new List<ScriptBehaviourManager>();

    public void Add(ScriptBehaviourManager system)
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
            world.DestroyManager(system);
    }
}
