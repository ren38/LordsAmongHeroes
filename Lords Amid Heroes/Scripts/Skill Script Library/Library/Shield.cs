using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : BaseSkill, IEffect
{

    protected ObjectInteractable source;
    protected ObjectActor subject;
    protected FloatAdjuster obs;

    protected List<GameObject> instanceList;
    protected bool timed = true;
    [SerializeField]
    protected float duration;
    protected float endTime;
    [SerializeField]
    private float value = 15.0f;

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
                effect.setup(targetActor, source, skillName, briefSkillDescription, duration, value);
                targetActor.applyNewEffect(effect);
            }
            else
            {
                preexisting.stack();
            }
        }
    }


    public void setup(
        ObjectActor subject, ObjectInteractable source,
        string name, string desc, float duration, float value
        )
    {
        this.subject = subject; // order passed in.
        this.source = source;
        skillName = name;
        briefSkillDescription = desc;
        this.duration = duration;
        this.value = value;
        setEnd(duration);
        obs = subject.gameObject.AddComponent<FloatAdjuster>();
        obs.setupObserver(subtractDamage);
        subject.piercingDamageSubscribe(obs);
        instanceList = new List<GameObject>();
    }

    public void apply(float deltaTime) //, ObjectInteractable source
    {
        //this.source = source;
        //subject.takeRawDamage(15.0f, source);
    }

    public float getEnd()
    { return endTime; }

    public void setEnd(float num)
    {
        endTime = num + Time.time;
    }

    public void end(ObjectActor subject)
    {
        obs.complete();
        Destroy(this);
    }

    public GameObject getIcon()
    {
        GameObject newInstance = Instantiate(SkillLibrary.Instance.getByID(4));
        instanceList.Add(newInstance);
        effectFunctions.setupIcon(newInstance, skillName, briefSkillDescription, timed, endTime);
        return newInstance;
    }

    public void stack()
    {
        endTime = duration + Time.time;
        effectFunctions.iconUpdate(instanceList, skillName, briefSkillDescription, true, endTime);
    }

    public float getDuration()
    {
        return duration;
    }

    private float subtractDamage(float baseDmg)
    {
        if (baseDmg < value)
        {
            return 0;
        }
        return baseDmg - value;
    }
}
