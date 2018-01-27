using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDisplay : MonoBehaviour
{
    public GameObject[] skillIcons;

    private static SkillDisplay instance;

    void Awake()
    {
        Debug.Assert(instance == false);
        instance = this;
    }

    void Update()
    {
        for (int i = 0; i != skillIcons.Length; ++i)
            skillIcons[i].transform.localPosition = Vector2.Lerp(skillIcons[i].transform.localPosition, Vector3.zero, 0.1f);
    }

    public static void OnSkillOwnerChanged(int skillId, Player player)
    {
        player.GiveSkill(skillId);
        instance.skillIcons[skillId].transform.parent = player.transform;
    }
}
