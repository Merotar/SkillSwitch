using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDisplay : MonoBehaviour
{
    private GameObject[] skillIcons;

    public Vector3 positionOffset;

    private static SkillDisplay instance;

    void Awake()
    {
        skillIcons = new GameObject[3];
        Debug.Assert(instance == false);
        instance = this;
    }

    void Update()
    {
        for (int i = 0; i != skillIcons.Length; ++i)
        {
            var skillIcon = skillIcons[i];
            if (skillIcon)
                skillIcon.transform.localPosition = Vector2.Lerp(skillIcon.transform.localPosition, positionOffset + i * Vector3.right, 0.1f);
        }
    }

    public static void OnSkillOwnerChanged(int skillId, Player player)
    {
        player.GiveSkill(skillId);
        instance.skillIcons[skillId].transform.parent = player.transform;
    }

    public static void OnSkillCollected(Skill skill, Player player)
    {
        instance.skillIcons[skill.skillId] = skill.gameObject;
        OnSkillOwnerChanged(skill.skillId, player);
    }
}
