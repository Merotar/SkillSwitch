using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDisplay : MonoBehaviour
{
    public GameObject[] skillIcons;

    private Vector3[] targetPositions = new Vector3[4];

    private static SkillDisplay instance;

    void Awake()
    {
        Debug.Assert(instance == false);
        instance = this;

        for (int i = 0; i != targetPositions.Length; ++i)
            targetPositions[i] = skillIcons[i].transform.localPosition;
    }

    void Update()
    {
        for (int i = 0; i != targetPositions.Length; ++i)
            skillIcons[i].transform.localPosition = targetPositions[i];
        //skillIcons[i].transform.localPosition = Vector3.Lerp(skillIcons[i].transform.localPosition, targetPositions[i], 0.5f);
    }

    public static void OnSkillOwnerChanged(int skillId, Player player)
    {
        Vector3 targetPos = instance.skillIcons[skillId].transform.localPosition;
        targetPos.y = instance.transform.transform.InverseTransformPoint(player.StartPosition()).y;
        instance.targetPositions[skillId] = targetPos;
    }
}
