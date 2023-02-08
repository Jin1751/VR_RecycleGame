/*
 * 스크립트 이름: StartGame.cs
 * 연결된 신: 게임 시작 신
 * 연결된 오브젝트: 메인 카메라
 * 기능1: 전방에 있는 게임 시작 버튼을 누르면 게임 진행 신으로 전환
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 forward = this.transform.TransformDirection(Vector3.forward * 1000);

        if(Physics.Raycast(this.transform.position, forward, out hit))//레이캐스트에 걸린 물체가 있다면 실행
        {
            if (hit.collider.CompareTag("StartBtn") && Input.GetMouseButton(0))//걸린 물체가 StartBtn이고 마우스가 눌렸다면 게임 진행신 로드
            {
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

}
