using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloriousDawn : SkillWithEffect
{
    /*
        skillName = "Glorious Dawn";
        briefSkillDescription = "Target is healed for 10 over 5 seconds.";
        fullSkillDescription = "Target gains 2 health per second over 5 seconds.";
        energyCost = 10.0f;
        sacrificeCost = 0.0f;
        adrenalineCost = 0;
        castTime = 0.5f;
        cooldown = 2.5f;
        range = 6.5f;
    */
    [SerializeField]
    private GameObject effectIcon;


    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            GloriousDawn preexisting = target.gameObject.GetComponent<GloriousDawn>();
            if (preexisting == null)
            {
                GloriousDawn regeneration = target.gameObject.AddComponent<GloriousDawn>();
                regeneration.setup(targetActor, 5.0f, source, effectIcon);
                targetActor.applyNewEffect(regeneration);
            }
            else
            {
                preexisting.stack();
            }
        }

    }

    protected const float REGENSPEED = 20.0f;
    protected ObjectInteractable source;
    protected ObjectActor subject;

    public void setup(ObjectActor subject, float num, ObjectInteractable source, GameObject icon)
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
        this.source = source;
        this.subject = subject;
        setupIcon(icon);
    }

    public override void apply(float deltaTime)
    {
        subject.takeHealingNoObs(REGENSPEED * deltaTime, source);
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
    }

    public void stack()
    {
        subject.takeRawHealing(30.0f, source);
    }

}
