using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingAvatar : MonoBehaviour
{
    public GameObject ManHair;
    public GameObject WomanHair;
    
    SkinnedMeshRenderer skin;

    private float GetProportion(double _min, double _max, double _value)
    {
        float len = (float)(_max - _min);
        float proportion;
        proportion = (float)(_value - _min) / len * 100;

        return proportion;
    }

    void settingGender(int gender)
    {
        if (gender == 1)
        {
            ManHair.gameObject.SetActive(true);
            WomanHair.gameObject.SetActive(false);
        } else
        {
            ManHair.gameObject.SetActive(false);
            WomanHair.gameObject.SetActive(true);
        }

    }
    void settingNose(double nose_w, double nose_h)
    {
        settingNoseWidth(nose_w);
        settingNoseHeight(nose_h);
    }
    void settingNoseWidth(double nose_w)
    {
        const double nose_w_ratio = 17.33;
        const double nose_w_ratio_max = 21.93;
        double my_w_ratio = nose_w / GameManager.Instance.userData.FaceWidth * 100;
        Debug.Log(my_w_ratio);
        if (my_w_ratio < nose_w_ratio)
        {
            skin.SetBlendShapeWeight(15, 0);
        }
        else if (my_w_ratio > nose_w_ratio_max)
        {
            skin.SetBlendShapeWeight(15, 100);
        }
        else
        {
            float proportion = GetProportion(nose_w_ratio, nose_w_ratio_max, my_w_ratio);
            skin.SetBlendShapeWeight(15, proportion);
        }
    }
    void settingNoseHeight(double nose_h)
    {
        const double max_ratio = 31.74;
        const double default_ratio = 26.91;
        const double min_ratio = 22.38;
        const int nose_low_idx = 16;
        const int nose_big_idx = 17;
        double my_ratio = nose_h / GameManager.Instance.userData.FaceWidth * 100;
        Debug.Log("# nose height");
        Debug.Log(my_ratio);

        if (my_ratio < min_ratio)
        {   
            skin.SetBlendShapeWeight(nose_low_idx, 100);    
            skin.SetBlendShapeWeight(nose_big_idx, 0);
        }
        else if (my_ratio <= default_ratio)
        {
            float low_proportion = 100 - GetProportion(min_ratio, default_ratio, my_ratio);
            skin.SetBlendShapeWeight(nose_low_idx, low_proportion);
        }
        else if (my_ratio < max_ratio)
        {
            float big_proportion = GetProportion(default_ratio, max_ratio, my_ratio);
            skin.SetBlendShapeWeight(nose_big_idx, big_proportion);
        }
        else
        {
            skin.SetBlendShapeWeight(nose_low_idx, 0);
            skin.SetBlendShapeWeight(nose_big_idx, 100);
        }
    }

    void settingMouth(double mouth_w)
    {
        const double max_ratio = 26.43;
        const double min_ratio = 17.33;
        double my_w_ratio = mouth_w / GameManager.Instance.userData.FaceWidth * 100;
        if (my_w_ratio < min_ratio)
        {
            skin.SetBlendShapeWeight(18, 0);
        }
        else if (my_w_ratio > max_ratio)
        {
            skin.SetBlendShapeWeight(18, 100);
        }
        else
        {
            float proportion = 100 - GetProportion(min_ratio, max_ratio, my_w_ratio);
            skin.SetBlendShapeWeight(18, proportion);
        }
    }

    void settingFace(double face_h, double chin_w)
    {
        settingFaceHeight(face_h);
        settingChin(chin_w);
    }
    void settingFaceHeight(double face_h)
    {
        const double max_ratio = 104.45;
        const double default_ratio = 99.04;
        const double min_ratio = 94.64;
        const int face_low_idx = 7;
        const int face_big_idx = 8;
        double my_h_ratio = face_h / GameManager.Instance.userData.FaceWidth * 100;
        Debug.Log("# face height");
        Debug.Log(my_h_ratio);

        if (my_h_ratio < min_ratio)
        {
            skin.SetBlendShapeWeight(face_low_idx, 100);
            skin.SetBlendShapeWeight(face_big_idx, 0);
        }
        else if (my_h_ratio <= default_ratio)
        {
            float low_proportion = 100 - GetProportion(min_ratio, default_ratio, my_h_ratio);
            skin.SetBlendShapeWeight(face_low_idx, low_proportion);
        }
        else if (my_h_ratio < max_ratio)
        {
            float big_proportion = GetProportion(default_ratio, max_ratio, my_h_ratio);
            skin.SetBlendShapeWeight(face_big_idx, big_proportion);
        }
        else
        {
            skin.SetBlendShapeWeight(face_low_idx, 0);
            skin.SetBlendShapeWeight(face_big_idx, 100);
        }
    }
    void settingChin(double chin_w)
    {
        const double max_ratio = 26.43;
        const double min_ratio = 17.33;
        const int chin_idx = 7;
        double my_w_ratio = chin_w / GameManager.Instance.userData.ChinWidth * 100;

        if (my_w_ratio < min_ratio)
        {
            skin.SetBlendShapeWeight(chin_idx, 0);
        }
        else if (my_w_ratio > max_ratio)
        {
            skin.SetBlendShapeWeight(chin_idx, 100);
        }
        else
        {
            float proportion = GetProportion(min_ratio, max_ratio, my_w_ratio);
            skin.SetBlendShapeWeight(chin_idx, proportion);
        }
    }

    void settingEyeDistance(double eye_d)
    {
        const double max_ratio = 28.00;
        const double default_ratio = 25.07;
        const double min_ratio = 22.55;
        const int eye_low_idx = 13;
        const int eye_big_idx = 14;
        double my_d_ratio = eye_d / GameManager.Instance.userData.FaceWidth * 100;
        Debug.Log("# eye distance");
        Debug.Log(my_d_ratio);

        if (my_d_ratio < min_ratio)
        {
            skin.SetBlendShapeWeight(eye_low_idx, 100);
            skin.SetBlendShapeWeight(eye_big_idx, 0);
        }
        else if (my_d_ratio <= default_ratio)
        {
            float low_proportion = 100 - GetProportion(min_ratio, default_ratio, my_d_ratio);
            skin.SetBlendShapeWeight(eye_low_idx, low_proportion);
        }
        else if (my_d_ratio < max_ratio)
        {
            float big_proportion = GetProportion(default_ratio, max_ratio, my_d_ratio);
            skin.SetBlendShapeWeight(eye_big_idx, big_proportion);
        }
        else
        {
            skin.SetBlendShapeWeight(eye_low_idx, 0);
            skin.SetBlendShapeWeight(eye_big_idx, 100);
        }
    }

    private void Start()
    {
        skin = transform.GetComponent<SkinnedMeshRenderer>();
        settingGender(GameManager.Instance.userData.Gender);
        settingFace(GameManager.Instance.userData.FaceHeight, GameManager.Instance.userData.ChinWidth);
        settingNose(GameManager.Instance.userData.NoseWidth, GameManager.Instance.userData.NoseHeight);
        settingMouth(GameManager.Instance.userData.MouthWidth);
    }
}
