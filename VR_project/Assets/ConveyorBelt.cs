using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public float speed; //일반 속도
    public List<GameObject> onBelt; //벨트 위에 올라가는 물건 리스트
    public Vector3 direction; //뱡향벡터 얘도 속도 영향끼침
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < onBelt.Count; i++) //컨베이어 벨트위에 물건이 있는동안만 작동
        {
            onBelt[i].GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision) //벨트에 닿으면 리스트에 추가
    {
        if (this.transform.CompareTag("Test"))
        {
            Debug.Log("Enter");
        }
        onBelt.Add(collision.gameObject);
    }
    private void OnCollisionExit(Collision collision) //벨트에서 떨어지면 리스트에서 삭제
    {
        if (this.transform.CompareTag("Test"))
        {
            Debug.Log("Exit");
        }
        
        onBelt.Remove(collision.gameObject); 
    }
}
