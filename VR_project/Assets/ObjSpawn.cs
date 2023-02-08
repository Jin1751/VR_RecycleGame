using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpawn : MonoBehaviour
{
    public GameObject[] prefabs; //프리팹 배열
    public float spawnTime; //생성 주기
    private bool isSpawn = false; //Invoke용 인수
    public bool gameEnd;//게임이 끝났는지 확인할 변수
    // Start is called before the first frame update
    void Start()
    {
        gameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)//게임이 끝날때까지 스폰
        {
            if (!isSpawn) //스폰 쿨타임 돌때마다 스폰시키는 함수
            {
                isSpawn = true;
                Invoke("Spawn", spawnTime);
            }
        }

    }
    void Spawn()
    {
        Vector3 T_position = new Vector3(this.transform.position.x + Random.Range(-0.5f, 0.5f), this.transform.position.y, this.transform.position.z + Random.Range(-0.5f, 0.5f)); //적당한 위치 잡아주는 벡터 생성
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], T_position, this.transform.rotation); //그 위치에 스폰
        isSpawn = false; //쿨타임
    }
}
