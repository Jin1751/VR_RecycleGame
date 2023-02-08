/*
 * 스크립트 이름: TrashCanG.cs
 * 연결된 신: 게임 진행 신
 * 연결된 오브젝트: DetectDestroyG (Quad)
 * 기능1: 캔 쓰레기통에 들어온 물체를 감지한다.
 * 기능2: 들어온 물체가 유리면 1점 추가(피버타임이면 2점 추가) 후 삭제
 * 기능3: 들어온 물체가 유리가 아니면 1점 감소 후 삭제
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashCanG : MonoBehaviour
{
    public GameObject hand;
    public GameObject score_UI;

    private GameObject canvas;
    private int score = 100;
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
        if (other.gameObject.GetComponent<Trash>().trashType == 1)//닿은 물체가 플라스틱이면 점수 증가 후 삭제
        {
            Destroy(other.gameObject);

            if (hand.GetComponent<HandMoving>().isFever)//피버타임이면 추가 점수 200점
            {
                hand.GetComponent<HandMoving>().combo++;
                score = 200;
                comboscore();

            }
            else//피버타임이 아니면 추가 점수 100점
            {
                hand.GetComponent<HandMoving>().combo++;
                score = 100;
                comboscore();

            }
            hand.GetComponent<HandMoving>().score += score;
            canvas = Instantiate(score_UI, this.transform.position, Quaternion.Euler(0, 0, 0));
            canvas.transform.GetChild(0).gameObject.GetComponent<Text>().text = score.ToString();
            canvas.GetComponent<Rigidbody>().AddForce(0, 300, 0);
            Destroy(canvas, 0.5f);

        }
        else if (other.gameObject.CompareTag("Hand") || other.gameObject.CompareTag("Glass"))//닿은 물체가 손이거나 유리 쓰레기 통이면 아무것도 안함
        {

        }
        else//닿은 물체가 손과 유리 쓰레기통이 아니면 100점 감소 후 삭제
        {
            Destroy(other.gameObject);
            hand.GetComponent<HandMoving>().score -= 100;
            hand.GetComponent<HandMoving>().combo = 0;

        }
    }
    void comboscore()//콤보 점수 추가 함수
    {
        int c = hand.GetComponent<HandMoving>().combo;

        if (c >= 5)
        {
            score += 10 * c;
        }
    }
}
