using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    //전체적인 UI와 기능 부분임 
    public static float checktime; //게임 매니저에게 시간을 넘겨줌
    float score;    //실시간 측정 값
    public Text textScore; //실시간 측정 값을 UI로 표현하는 text
    
    public float time;  //경과시간
    public Text timeText; //시간 UI

    float countdown;    //카운트 다운 
    public Text countdownText; //카운트 다운 UI

    public Text resultText; //결과를 보여주는 UI

    public bool pause = false; //일시정지 확인

    //타스크립트 접근용
    FrowardMoving frowardMoving; 
    BackMoving backMoving;
    GameManager gameManager;
    SceneManager sceneManager;
    
    //씬이 시작되면 초기화함
    void Start()
    {
        //다른씬 컴포넌트 가져옴
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        frowardMoving = GameObject.Find("Cat").GetComponent<FrowardMoving>();
        backMoving = GameObject.Find("Cat").GetComponent<BackMoving>();

        if(pause == false)  //일시정지가 아니면 초기값 셋팅
        {
            time = 0.001f;
            countdown = 6.0f;
            score = 0;
            resultText.gameObject.SetActive(false);
        }
        if(pause == true)   //일시정지면 시간을 멈추고 결과값을 보여줌
        {
            Time.timeScale = 0.0f;
            resultText.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Executecount < 3 && pause == false) //GameManager의 실행횟수를 확인 && 일시정지가 아니면 실행
        {
            if (Time.timeScale == 0.0f) //시간이 멈췄을때
            {
                Time.timeScale = 1.0f; //시간이 다시 돌게하고
                countdown -= Time.deltaTime;    //카운트 다운하고
                CountDown(countdown);           //UI로 보여줌
                if (countdown == 0.0f || countdown < 0) //카운트다운이 끝나면 실행
                {
                    countdownText.gameObject.SetActive(false);  //카운트 다운 UI 없애고
                    time += Time.deltaTime;     //경과시간 증가
                    if (time <= 3.01f && Time.timeScale != 0.0f)    //0~3초가 되거나 시간이 멈추지 않았으면
                    {
                        timeText.text = "시간 :" + time.ToString();   //시간 UI
                        switch (gameManager.IsInhale)   //들숨인지 날숨인지 확인
                        {
                            case true:  //들숨이면
                                score = backMoving.count;   //backMoving에서 count값 가져옴
                                AddScore(score);
                                gameManager.mathScore[gameManager.Executecount] = score; //게임 매니저로 측정된 스코어값 보내줌
                                break;
                            case false: //날숨이면
                                score = frowardMoving.count;
                                AddScore(score);
                                gameManager.mathScore[gameManager.Executecount] = score;    //게임 매니저로 측정된 스코어값 보내줌
                                break;
                        }
                    }
                    else if (time > 3.01f) //신이 전환되면 실행 횟수하고 초기화함
                    {
                        gameManager.mathScore[gameManager.Executecount] = score;
                        Time.timeScale = 0.0f;  //시간 멈추고
                        gameManager.Executecount++; //실행횟수 1올려주고
                        switch(gameManager.IsInhale)//화면 전환
                        {
                            case true:  //들숨이면
                                SceneManager.LoadScene("InHale");
                                break;
                            case false: //날숨이면
                                SceneManager.LoadScene("ExHale");
                                break;
                        }
                    }else { }
                }else { }
            }
            else  //시간이 멈추지 않았으면 (일반적인 실행부분) 위와 같음
            {
                countdown -= Time.deltaTime;    //카운트 다운하고
                CountDown(countdown);
                if (countdown == 0.0f || countdown < 0)
                {
                    countdownText.gameObject.SetActive(false);
                    time += Time.deltaTime;
                    if (time <= 3.01f && Time.timeScale != 0.0f)
                    {
                        timeText.text = "시간 :" + time.ToString();
                        switch (gameManager.IsInhale)
                        {
                            case true:  //들숨이면
                                score = backMoving.count;
                                AddScore(score);
                                gameManager.mathScore[gameManager.Executecount] = score; //게임 매니저에게 측정된 값 보내줌
                                break;
                            case false: //날숨이면
                                score = frowardMoving.count;
                                AddScore(score);
                                gameManager.mathScore[gameManager.Executecount] = score; //게임 매니저에게 측정된 값 보내줌
                                break;
                        }
                    }
                    else if (time > 3.01f) //씬 실행
                    {
                        gameManager.mathScore[gameManager.Executecount] = score;
                        Time.timeScale = 0.0f;
                        gameManager.Executecount++;
                        switch (gameManager.IsInhale)
                        {
                            case true:  //들숨이면
                                SceneManager.LoadScene("InHale");
                                break;
                            case false: //날숨이면
                                SceneManager.LoadScene("ExHale");
                                break;
                        }
                    } else { }
                }else { }
            }
        }
        else if (gameManager.Executecount == 3 && pause == false && gameManager.IsInhale == false) //실행횟수가 3이고 일시정지가 아니면
        {
            Debug.LogWarning("날숨에서 들숨으로 넘어감");
            gameManager.Executecount = 0;
            gameManager.IsInhale = true; // 들숨으로 바꿔주고
            SceneManager.LoadScene("InHale"); //날숨->들숨
            if (Input.GetKeyDown(KeyCode.Escape))   //ESC누르면 측정 메뉴 화면으로 돌아가는데 이부분은 안드로이드 뒤로가기 버튼넣으면 될듯
            {
                pause = false;  //일시정지 풀어주고
                gameManager.Executecount = 0;   //실행횟수 0해주고
                SceneManager.LoadScene("StartMenu");
            }
        }
        else
        {
            Debug.LogWarning("모든 측정 종료시");

            pause = true;   //일시정지로 바꿔주고
            Time.timeScale = 0.0f;  //시간멈춤 (이래야 게임이 안돌아감)
            Result();   //결과값 보여주고
            if (Input.GetKeyDown(KeyCode.Escape))   //ESC누르면 측정 메뉴 화면으로 돌아가는데 이부분은 안드로이드 뒤로가기 버튼넣으면 될듯
            {
                pause = false;  //일시정지 풀어주고
                gameManager.Executecount = 0;   //실행횟수 0해주고
                SceneManager.LoadScene("StartMenu");
            }
        }

    }

    //측정 시작전 카운트 다운을 UI에
    public void CountDown(float countdown)
    {
        int count;
        count = (int)countdown;
        countdownText.text = count.ToString();
    }

    //압력 UI
    public void AddScore(float newScore)
    {
        textScore.text = "압력 : " + newScore;
    }

    //측정결과
    public void Result()
    {
        resultText.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(false);
        resultText.text = "측정 호기압은 : " + gameManager.InHaleScore.ToString() + " 입니다. \n" +
                          "측정 흡기압은 : " + (-1 * gameManager.ExHaleScore).ToString() + " 입니다."; ;

    }
}
