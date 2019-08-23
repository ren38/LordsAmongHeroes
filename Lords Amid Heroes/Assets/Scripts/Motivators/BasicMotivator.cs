using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasicMotivator : MonoBehaviour
{
    [SerializeField]
    protected ObjectActor actorSelf;
    protected team myTeam;
    protected bool inCombat = false;
    protected GameObject target;

    public virtual void newTargetIndividual(GameObject newTarget)
    {
        target = newTarget;
        inCombat = true;
    }
    
    public void leaveCombat()
    {
        inCombat = false;
        target = null;
    }

    public void interestLost()
    {
        inCombat = false;
        actorSelf.abruptSkillCancel();
        actorSelf.clearQueue();
    }

    public virtual void dying() { interestLost(); }
    public void joinTeam(team i) { myTeam = i; }
}
