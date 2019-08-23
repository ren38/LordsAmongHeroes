using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyphersPoisonSkill : SkillWithEffect
{
    /*
    skillName = "Cypher's Poison";
    briefSkillDescription = "Target takes 15 extra damage.";
    fullSkillDescription = "While this enchantment is active, the target " +
    "takes 15 extra damage each time damage is applied. (Does not count damage over time.)";
    energyCost = 15.0f;
    sacrificeCost = 0.0f;
    adrenalineCost = 0;
    castTime = 1.5f;
    cooldown = 5.5f;
    range = 15.0f;
    */
    
    [SerializeField]
    private GameObject effectIcon;
    

    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        //Debug.Log(energyCost);
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            CyphersPoisonSkill preexisting = target.gameObject.GetComponent<CyphersPoisonSkill>();
            if (preexisting == null)
            {
                CyphersPoisonSkill cyphers = target.gameObject.AddComponent<CyphersPoisonSkill>();
                cyphers.setup(5.0f, targetActor, self, effectIcon);
                targetActor.applyNewEffect(cyphers);
            }
            else
            {
                preexisting.stack();
            }
        }
    }
    
    protected ObjectInteractable source;
    protected ObjectActor subject;
    protected FloatAdjuster obs;

    public void setup(float num, ObjectActor target, ObjectInteractable source, GameObject icon)
    {
        if (num != 0)
        {
            timed = true;
            duration = num;
            endTime = duration + Time.time;
        }
        else
        {
            timed = false;
        }
        /*
        name = "Cypher's Poison";
        description = string.Format("Increases incoming damage by 15 points.");
        */
        obs = target.gameObject.AddComponent<FloatAdjuster>();
        obs.setupObserver(addDamage);
        target.rawDamageSubscribe(obs);
        this.source = source;
        this.subject = target;
        setupIcon(icon);
    }

    public override void apply(float deltaTime) //, ObjectInteractable source
    {
        //this.source = source;
        //subject.takeRawDamage(15.0f, source);
    }

    public override void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public override float getDuration()
    {
        return duration;
    }

    public override float getEnd()
    { return endTime; }

    public override void end(ObjectActor subject)
    {
        clearModels();
        obs.complete();
    }

    public void stack()
    {
        endTime = endTime + duration;
        base.iconUpdate(skillName, briefSkillDescription, true, endTime);
    }

    private float addDamage(float baseDmg)
    {
        return baseDmg + 15.0f;
    }

    /*
    public void OnNext(GameObject newSkill)
    {
        subject.takeDamageNoObs(15.0f, source);
    }
    */

    public void OnCompleted()
    {
        obs.complete();
    }
}
