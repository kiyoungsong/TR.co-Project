using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrowardMoving : MonoBehaviour
{
    public float speed = 100.0f;    //앞으로 이동하는 속도 조절 
    public int count = 0;           //얼마나 이동했는가를 카운트하는 부분 (블루투스 수치값 넣어주면 될듯)
    UIManager uIManager;
    public float checkTime = 0.0f;  //시간이 얼마나 지냤나를 체크하는 부분
    // Start is called before the first frame update
    void Start()
    {

        uIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

    }

    // Update is called once per frame
    void Update()
    {
        checkTime = uIManager.time;
        if (uIManager)
        {
            if (checkTime <=3.01f && checkTime >= 0.01f)
            {
                transform.Translate(Vector3.forward * speed * Time.deltaTime / 2, Space.World);
                count++;
                //키누르면 앞으로 이동 이부분의 값을 블루투스로 바꿔주면 될듯함
                if (Input.GetKeyDown(KeyCode.UpArrow) == true)  
                {
                    transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.World);
                    count++;
                }
            }
        }
        else
        {
            Debug.Log("안됨");
        }
        
        
    }
}
