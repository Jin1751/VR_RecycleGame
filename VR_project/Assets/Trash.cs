/*
 * 스크립트 이름: Trash.cs
 * 연결된 신: 게임 진행 신
 * 연결된 오브젝트: 쓰레기 프리펩
 * 기능1: 쓰레기의 종류를 정한다.(0: 플라스틱, 1: 유리, 2: 캔)
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public int trashType;//쓰레기 종류를 Inspector에서 정한다.(0: 플라스틱, 1: 유리, 2: 캔)
    private float zeroSpeed = 0.1f;//쓰레기가 사라지는 최소 움직임
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
