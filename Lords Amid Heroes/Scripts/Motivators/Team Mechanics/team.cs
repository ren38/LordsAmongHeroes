using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class team : MonoBehaviour
{
    [SerializeField]
    protected List<ObjectActor> actorObjects;
    //private league myLeague;
    public virtual List<ObjectActor> getActorObjects() { return actorObjects; }
    protected List<team> enemyTeams;
    protected List<ObjectActor> enemyIndividuals;


    protected void setup()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private bool combat = false;
}