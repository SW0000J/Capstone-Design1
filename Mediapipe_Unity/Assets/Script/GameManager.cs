using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Vector3[] eye_p = new Vector3[6];
    public Vector3[] mouth_p = new Vector3[6];

    public double left_eye_blink = 0;
    public double right_eye_blink = 0;
    public double mouth_open = 0;




    private static GameManager instance;
    private UserData mUserData;

    public UserData userData
    {
        get
        {
            return mUserData;
        }
    }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Init()
    {
        if (instance == null)
        {
            instance = this;
        }

        mUserData = new UserData();

        DontDestroyOnLoad(this.gameObject);
    }

    private void Awake()
    {
        Init();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
