using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToPlayScene : MonoBehaviour
{
    public void changeScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
