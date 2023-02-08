/*
 * 스크립트 이름: ThrowEffect.cs
 * 연결된 신: 게임 시작 신
 * 연결된 오브젝트: 효과를 위한 오브젝트 생성 Empty 오브젝트
 * 기능1: 정해진 위치에서 설정된 오브젝트를 좌우 무작위 각도로 0.3초마다 랜덤하게 뿌린다.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowEffect : MonoBehaviour
{
    public int trashNum = 3;
    public GameObject[] trashes = new GameObject[3];
    private GameObject spawned;
    private Vector3 randRotate;
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(SpawnEffect());//코루틴 시작
    }

    // Update is called once per frame
    void Update()
    {


    }

    IEnumerator SpawnEffect()
    {
        switch (this.transform.position.z)//Empty 오브젝트 위치에 따라 무작위 좌우 각도 설정
        {
            case 0:
                randRotate = new Vector3(0, 0, Random.Range(-100, 100));
                break;
            default:
                randRotate = new Vector3(Random.Range(-100, 100), 0, 0);
                break;
        }
 
        spawned = Instantiate(trashes[Random.Range(0, trashNum)], this.transform.position, Quaternion.identity);//설정된 오브젝트들 중 하나를 생성
        spawned.gameObject.transform.localScale = new Vector3(3.0f,3.0f,3.0f);
        spawned.transform.GetComponent<Rigidbody>().AddForce(randRotate * 2.0f);//랜덤한 각도로 힘을 줘 좌우로 흩뿌림
        Destroy(spawned, 2.0f);//생성된 오브젝트 2초 뒤 삭제
        yield return new WaitForSeconds(0.3f);//0.3초 대기
        StartCoroutine(SpawnEffect());//코루틴 호출로 무한 반복
    }
}
