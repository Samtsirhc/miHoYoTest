using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CDTip : MonoBehaviour
{
    public Text text;
    public PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isSkillCd)
        {
            text.text = "技能已就绪";
        }
        else
        {
            text.text = "技能冷却中 " + (int)(playerController.skillCdTimer);
        }
    }
}
