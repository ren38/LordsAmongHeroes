using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLibrary : Singleton<SkillLibrary>
{
    [SerializeField]
    private List<GameObject> library;

    public GameObject getByID(int id)
    {
        return library[id];
    }

    public List<GameObject> getByListID(List<int> list)
    {
        List<GameObject> returnList = new List<GameObject>();
        foreach (int id in list)
        {
            returnList.Add(getByID(id));
        }
        return returnList;
    }
}
