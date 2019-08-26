using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamMemberMotivator : BasicMotivator
{
    private GameObject[] skillBar;
    private System.Random rand;
    private int skillCount;
    [SerializeField]
    private int planning = 1;
    private float timer;
    private ObjectActor targetActor;

    void Start()
    {
        skillBar = actorSelf.getSkillBar();
        rand = new System.Random();
        skillCount = skillBar.Length;
    }

    public override void newTargetIndividual(GameObject newTarget)
    {
        target = newTarget;
        targetActor = newTarget.gameObject.GetComponent<ObjectActor>();
        inCombat = true;
        timer = Time.time + 20.0f;
    }

    void Update()
    {
        // loses interest
        if (inCombat && (Time.time >= timer))
        {
            interestLost();
        }

        if (inCombat && actorSelf.queueCount() < planning)
        {
            actorSelf.skillEnqueue(rand.Next(skillCount - 1), targetActor);
        }
    }
}
