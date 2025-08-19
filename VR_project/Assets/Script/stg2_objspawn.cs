using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stg2_objspawn : MonoBehaviour
{
    public GameObject[] prefabs;
    public float spawnTime;
    private bool isSpawn;
    public bool gameEnd;

    // Start is called before the first frame update
    void Start()
    {
        
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
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)], T_position, Quaternion.Euler(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f))); //그 위치에 스폰
        obj.GetComponent<Rigidbody>().AddForce(new Vector3(0,5,-5) * 70.0f);
        isSpawn = false; //쿨타임
    }
}
