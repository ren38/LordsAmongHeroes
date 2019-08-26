using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillbookUI : Singleton<SkillbookUI>
{
    [SerializeField]
    private GameObject Panel;
    [SerializeField]
    private GameObject Content;
    [SerializeField]
    private bool active = false;
    private List<GameObject> skillList;
    private List<GameObject> skillInstances;

    public void toggleSkillBook()
    {
        //if not active, make it active
        if(!active)
        {
            createList();
            active = true;
            Panel.SetActive(active);
        }
        //if active, deactivate it
        else
        {
            destroyList();
            active = false;
            Panel.SetActive(active);
        }
    }

    private void createList()
    {
        //generate objects to be layed out in the panel
        skillList = SkillLibrary.Instance.getByListID(PlayerMotivator.Instance.getSkillBook());
        skillInstances = new List<GameObject>();
        OrderEnum pri = PlayerMotivator.Instance.GetPrimaryOrderEnum();
        OrderEnum sec = PlayerMotivator.Instance.GetSecondaryOrderEnum();
        foreach (GameObject skill in skillList)
        {
            BaseSkill component = skill.GetComponent<BaseSkill>();
            // make sure that it has a baseSkill component, just in case
            if (component != null)
            {
                OrderEnum skillOrder = component.getOrder();
                // and check if the object is of the right order, then add it to the list
                if (skillOrder == pri || skillOrder == sec || skillOrder == OrderEnum.none)
                {
                    GameObject instance = Instantiate(skill, Content.transform);
                    instance.AddComponent<skillDragAndDrop>();
                    skillInstances.Add(instance);
                }
            }
        }
    }

    private void destroyList()
    {
        skillList.Clear();
        foreach (GameObject instance in skillInstances)
        {
            Destroy(instance);
        }
    }

    public void updateList()
    {
        if(active)
        {
            destroyList();
            createList();
        }
    }
}
