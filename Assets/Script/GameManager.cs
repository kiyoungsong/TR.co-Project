using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //싱글턴을 할당할 전역변수
    public bool IsInhale = false; //들숨인지를 확인
    public int Executecount = 0; //측정 횟수 카운트
    public float MaxScore;  //측정 최대 값
    public float score; //실시간 측정 값
    public float[] mathScore = Enumerable.Repeat<float>(0, 1024).ToArray<float>(); //최대값 연산을 위한 배열
    public GameObject finishGameText; //측정 종료 시 활성화할 UI 게임 오브젝트
    public SceneManager sceneManager;
    

    // Start is called before the first frame update
    void Start()
    {
 
    }
    void Awake()
    {
        //싱글턴 변수 instance가 비어있는가?
        if(instance == null)
        {
            //비어있으면 그곳에 자기자신을 할당
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //무시
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다!.");
               
        }
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "ExHale") //날숨
        {
            IsInhale = false;
            Max(mathScore);
        }
         
        if (SceneManager.GetActiveScene().name == "InHale") //들숨
        {
            IsInhale = true;
            Max(mathScore);
        }
    }

    //최대값 계산 
    void Max(float[] score)
    {
        float max = float.MinValue;
        for (int i = 0; i < mathScore.Length; i++)
        {
            if (max < mathScore[i])
            {
                max = mathScore[i];
            }
        }
        if(!IsInhale) //들숨
        {
            MaxScore = (float)Math.Truncate(max*100.0f)/100.0f; //최대값
        }
        else
        {
            //이부분은 확인해봐야됨.
            MaxScore = ((float)Math.Truncate(max * 100.0f)/100.0f)*-1; //음수 최대값
        }
        //Debug.Log(MaxScore); //최대값 확인하는 로그
    }

    public void GameStart()
    {
        Debug.Log("버튼작동확인");
        IsInhale = true;
    }
    

}
