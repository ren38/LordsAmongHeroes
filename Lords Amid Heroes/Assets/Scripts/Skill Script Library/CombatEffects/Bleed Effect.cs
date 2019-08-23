using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedEffect : BaseEffect
{
    [SerializeField]
    private const float DEGENSPEED = 2.0f;
    [SerializeField]
    private int multiple = 1;
    private const int MAXMULTIPLIER = 10;
    protected ObjectActor subject;
    protected ObjectInteractable source;

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
        name = "Bleed";
        description = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        this.source = source;
        this.subject = subject;
        setupIcon(icon);
    }

    public override void apply(float deltaTime)
    {
        subject.takeDamageNoObs(DEGENSPEED * deltaTime * multiple, source); 
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
        multiple = 1;
    }

    public override void stack()
    {
        if (multiple < MAXMULTIPLIER)
        {
            multiple++;
        }
        endTime += duration;
        if(endTime - Time.time >= 15.0f)
        {
            endTime = Time.time + 15.0f;
        }
        string newName = ("Bleed x" + multiple);
        string newDescription = string.Format("Lose {0} health per second.", DEGENSPEED * multiple);
        bool timed = true;
        base.stack(newName, newDescription, timed, endTime);
    }
}
