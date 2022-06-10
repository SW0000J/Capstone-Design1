using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Test : MonoBehaviour
{
    Image image; 
    
    double LeftEyeBlink(Vector3[] p)
    {

        float A = (p[1].x - p[5].x) * (p[1].x - p[5].x) + (p[1].y - p[5].y) * (p[1].y - p[5].y);// + (p[1].z - p[5].z) * (p[1].z - p[5].z);
        float B = (p[2].x - p[4].x) * (p[2].x - p[4].x) + (p[2].y - p[4].y) * (p[2].y - p[4].y);// + (p[2].z - p[4].z) * (p[2].z - p[4].z);
        float C = (p[0].x - p[3].x) * (p[0].x - p[3].x) + (p[0].y - p[3].y) * (p[0].y - p[3].y);// + (p[0].z - p[3].z) * (p[0].z - p[3].z);

        double A_ = Math.Sqrt((double)A);
        double B_ = Math.Sqrt((double)B);
        double C_ = Math.Sqrt((double)C);

        double ear_aspect_ratio = (A_ + B_) / (2.0 * C_);

        return ear_aspect_ratio;
    }

    int InputBlinkLeft(double ear_aspect_ratio)
    {
        int blend_blink_left = 0;
        double changenum = (3.0 / ear_aspect_ratio);

        blend_blink_left = (int)changenum;


        return blend_blink_left;
    }

    int InputMouthOpen(double ear_aspect_ratio)
    {
        int blend_blink_left = 0;
        double changenum = (ear_aspect_ratio *330);

        blend_blink_left = (int)changenum;

        if(blend_blink_left > 100)
        {
            blend_blink_left = 100;
        }


        return blend_blink_left;
    }

    SkinnedMeshRenderer tt;
    // Start is called before the first frame update

    
    void Start()
    {
        tt = transform.GetComponent<SkinnedMeshRenderer>();
        


    }

    // Update is called once per frame
    void Update()
    {
        //tt.SetBlendShapeWeight(1, InputBlinkLeft(GameManager.Instance.eye_pos));// InputMouthOpen(LeftEyeBlink(GameManager.Instance.mouth_p));
        //Debug.Log(InputBlinkLeft(GameManager.Instance.eye_pos));
        //   Debug.Log(GameManager.Instance.eye_p[0]);
        //Debug.Log(LeftEyeBlink(GameManager.Instance.eye_p));
        //   Debug.Log(tt.GetBlendShapeWeight(0));

    }
}
