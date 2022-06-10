using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum UserGender
{
    Male,
    Female
}


public class UserData : MonoBehaviour
{
    public int avatarNumber;

    public bool IsCreatePhotonCharacter;

    public GameObject UserObject;

    //public GameObject boardObject;

    //public GameObject photonVoiceObject;

    //public int currentUserNumber; // �ڽ��� �� ��°����

    public string UserName;
    public int UserMode;

    public string UserId;
    public string UserPass;

    public int currentTeacherMonitorNum;

    // user gender ����
    public int Gender;

    // user face ����
    public double FaceWidth;
    public double FaceHeight;
    public double ChinWidth;
    public double NoseWidth;
    public double NoseHeight;
    public double MouthWidth;
    public double UpperLipHeight;
    public double LowerLipHeight;
    public double EyeDistance;
}
