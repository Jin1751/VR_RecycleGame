/*
 * 스크립트 이름: Destoryer.cs
 * 연결된 신: 게임 진행 신
 * 연결된 오브젝트: Destoryer (Quad)
 * 기능1: 컨베이어 벨트 마지막에 위치해 잡지 못한 쓰레기를 삭제시켜 메모리를 관리한다.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destoryer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Trash"))//부딪힌 물체가 쓰레기면 삭제
        {
            Destroy(other.gameObject);
        }
    }
}
