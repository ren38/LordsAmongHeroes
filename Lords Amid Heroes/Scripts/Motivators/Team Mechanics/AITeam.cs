using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITeam : team
{
    [SerializeField]
    private List<BasicMotivator> motivatorUnits;
    private bool teamDead = false;

    public List<BasicMotivator> getMotivatorUnits() { return motivatorUnits; }

    // Start is called before the first frame update
    void Start()
    {
        base.setup();
        teamSetup();
    }

    public void teamSetup()
    {
        foreach (BasicMotivator unit in motivatorUnits)
        {
            unit.joinTeam(this);
        }
        foreach (ObjectActor actor in actorObjects)
        {
            GameObjectObserver obs = gameObject.AddComponent<GameObjectObserver>();
            obs.setupObserver(notifyAll);
            actor.rawHitSubscribe(obs);
            GameObjectObserver deadMember = gameObject.AddComponent<GameObjectObserver>();
            deadMember.setupObserver(removeDead);
            actor.deathSubscribe(deadMember);
        }
    }

    private void notifyAll(GameObject newTarget)
    {
        foreach (BasicMotivator unit in motivatorUnits)
        {
            unit.newTargetIndividual(newTarget);
        }
    }

    private void removeDead(GameObject newDead)
    {
        foreach (ObjectActor actor in actorObjects)
        {
            if (actor.getDeathState())
            {
                BasicMotivator deadActor = actor.GetComponent<BasicMotivator>();
                if (deadActor != null)
                {
                    motivatorUnits.Remove(deadActor);
                }
            }
        }
        if(motivatorUnits.Count == 0)
        {
            teamDead = true;
        }
    }
}
