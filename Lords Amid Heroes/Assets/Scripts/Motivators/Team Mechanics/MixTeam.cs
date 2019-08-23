using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixTeam : team
{
    private PlayerMotivator player;
    private List<BasicMotivator> AIUnits;

    // Start is called before the first frame update
    void Start()
    {
        AIUnits = new List<BasicMotivator>();
        base.setup();
    }
}