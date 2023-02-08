/*
 * 스크립트 이름: HandPosion.cs
 * 연결된 신: 게임 진행 신
 * 연결된 오브젝트: 손 모양 오브젝트(Hand)
 * 기능1: 손에 닿은 물체가 Trash이면 닿은 물체를 복사 후 따라오게 한다.
 * 기능2: 손이 움직이고 있는지 확인해준다. 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosition : MonoBehaviour
{
    public GameObject handPoint;//손 오브젝트의 위치
    public GameObject grab;//팔 오브젝트
    public GameObject stickPoint;//복사된 물체를 자식으로 가져올 Empty 오브젝트
    public GameObject trash;//복사된 물체
    public Vector3 throwDir;
    public bool isHold = false;// 1개만 복사하기 위한 변수
    public bool throwT = false;
    private int t = 0;
    // Start is called before the first frame update
    void Start()
    {
        throwDir = new Vector3(0, 1, 1);//던지는 방향
    }

    // Update is called once per frame
    void Update()
    {
        
        this.transform.position = handPoint.transform.position;//손 오브젝트가 팔에 연결된 손 위치 오브젝트의 위치를 따라감
        this.transform.rotation = handPoint.transform.rotation;//손 오브젝트가 팔에 연결된 손 위치 오브젝트의 각도를 따라감

        if (throwT == false && isHold == true)//물체를 던지지 않고 손에 들고 있으면 물체가 손을 쫓아옴
        {
            trash.transform.position = stickPoint.transform.position;
            trash.transform.rotation = stickPoint.transform.rotation;
            trash.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);//쓰레기의 velocity를 모두 0으로 설정
        }
        if (throwT == true && isHold == true)//손에 물체가 있고 던지기 변수가 참이면 던짐
        {
            isHold = false;//손에 물체가 없음
            throwT = false;//던지기 중복 실행 방지
            trash.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);//쓰레기의 velocity를 모두 0으로 설정 (이전 버그 발생 이유: velocity y가 계속 마이너스 되고 있었음 이유는 몰라)
            trash.GetComponent<Rigidbody>().useGravity = true;//손에 들고있는 물체에 중력적용
            trash.GetComponent<Rigidbody>().AddRelativeForce(throwDir * 6.3f, ForceMode.VelocityChange);//쓰레기 오브젝트 좌표계에서 벡터 (0,1,1)으로 발사
            trash = null;//쓰레기 오브젝트 변수를 비움

        }
    }

    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Trash") && isHold == false)//부딪힌 물체가 Trash면 해당 물체를 손을 따라오게 함
        {
            t++;
            Debug.Log("HIT : " + t);
            trash = other.gameObject;
            trash.transform.position = stickPoint.transform.position;
            trash.transform.rotation = stickPoint.transform.rotation;
            isHold = true;
            grab.GetComponent<HandMoving>().isHolding = true;
        }
        grab.GetComponent<HandMoving>().isHit = true;//팔에 있는 HandMoving 스크립트에 충돌감지 변수를 true로 전환
    }
}
