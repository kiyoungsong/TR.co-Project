using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    //메인 화면에서 씬 전환하는 스크립트
   public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }


}
