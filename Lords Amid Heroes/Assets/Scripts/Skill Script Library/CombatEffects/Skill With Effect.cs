using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillWithEffect : BaseSkill
{
    protected GameObject iconInstance;
    protected List<GameObject> listIconInstances;
    protected bool timed;
    protected float duration;
    protected float endTime;

    public virtual GameObject getIcon()
    {
        GameObject instance = Instantiate(iconInstance);
        UIEffect e = instance.AddComponent<UIEffect>();
        e.setup(skillName, briefSkillDescription, timed, endTime);
        EffectMouseOver m = instance.AddComponent<EffectMouseOver>();
        m.setup(e);
        listIconInstances.Add(instance);
        return instance;
    }

    protected virtual void setupIcon(GameObject icon)
    {
        iconInstance = Instantiate(icon);
        listIconInstances = new List<GameObject>();
    }

    public void iconUpdate(string newName, string newDescription, bool newTimed, float newEndTime)
    {
        foreach (var instance in listIconInstances)
        {
            if (instance != null)
            {
                UIEffect e = instance.GetComponent<UIEffect>();
                if (e != null)
                {
                    e.setup(newName, newDescription, newTimed, newEndTime);
                }
                EffectMouseOver m = instance.GetComponent<EffectMouseOver>();
                if (m != null)
                {
                    m.pull();
                }
            }
        }
    }

    protected virtual void clearModels()
    {
        Destroy(iconInstance);
    }

    public abstract void apply(float deltaTime);
    public abstract float getEnd();
    public abstract void setEnd(float num);
    public abstract void end(ObjectActor subject);
    public abstract float getDuration();
}
