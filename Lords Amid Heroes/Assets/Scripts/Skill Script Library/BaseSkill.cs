﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSkill : MonoBehaviour
{
    [SerializeField]
    protected string skillName;
    [TextArea][SerializeField]
    protected string briefSkillDescription;
    [TextArea][SerializeField]
    protected string fullSkillDescription;
    [SerializeField]
    protected float energyCost;
    [SerializeField]
    protected float sacrificeCost;
    [SerializeField]
    protected int adrenalineCost;
    [SerializeField]
    protected float castTime;
    [SerializeField]
    protected float cooldown;
    [SerializeField]
    protected float range;

    public abstract void activate(ObjectActor self, ObjectCombatable target);
    //public abstract bool validateTargetting(ObjectCombatable target);
    public virtual float getCooldown() { return cooldown; }
    public virtual float getCastTime() { return castTime; }
    public virtual float getEnergyCost() { return energyCost; }
    public virtual float getHealthCost() { return sacrificeCost; }
    public virtual float getAdrenalCost() { return adrenalineCost; }
    public virtual float getRange() { return range; }
    public virtual string getName() { return skillName; }
    public virtual string getShortDescription() { return briefSkillDescription; }
    public virtual string getLongDescription() { return fullSkillDescription; }
}