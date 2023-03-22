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
            text.text = "�����Ѿ���";
        }
        else
        {
            text.text = "������ȴ�� " + (int)(playerController.skillCdTimer);
        }
    }
}
