using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGender : MonoBehaviour
{
    public void GenderClick(bool isOn)
    {
        if (isOn)
        {
            GameManager.Instance.userData.Gender = 1;
            Debug.Log("남자");
        }
        else
        {
            GameManager.Instance.userData.Gender = 2;
            Debug.Log("여자");
        }
    }
}
