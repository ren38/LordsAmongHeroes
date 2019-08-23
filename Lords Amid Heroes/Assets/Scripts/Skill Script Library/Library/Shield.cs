using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SkillWithEffect
{
    [SerializeField]
    private GameObject effectIcon;

    public override void activate(ObjectActor self, ObjectCombatable target)
    {
        //Debug.Log(energyCost);
        ObjectActor targetActor = target as ObjectActor;
        ObjectInteractable source = self as ObjectInteractable;
        if (targetActor != null)
        {
            Shield preexisting = target.gameObject.GetComponent<Shield>();
            if (preexisting == null)
            {
                Shield effect = target.gameObject.AddComponent<Shield>();
                effect.setup(5.0f, targetActor, self, effectIcon);
                targetActor.applyNewEffect(effect);
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
        obs = target.gameObject.AddComponent<FloatAdjuster>();
        obs.setupObserver(subtractDamage);
        target.piercingDamageSubscribe(obs);
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
        endTime = duration + Time.time;
        base.iconUpdate(skillName, briefSkillDescription, true, endTime);
    }

    private float subtractDamage(float baseDmg)
    {
        if (baseDmg < 15.0f)
        {
            return 0;
        }
        return baseDmg - 15.0f;
    }

    public void OnCompleted()
    {
        obs.complete();
    }
}
