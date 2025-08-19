using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Text score;
    public GameObject toHome;
    private int totalScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        totalScore = PlayerPrefs.GetInt("score");
        score.text = "Total Score: " + totalScore;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 forward = this.transform.TransformDirection(Vector3.forward * 1000);

        if (Physics.Raycast(this.transform.position, forward, out hit))//레이캐스트에 걸린 물체가 있다면 실행
        {
            if (hit.collider.CompareTag("Home") && Input.GetMouseButtonDown(0))//걸린 물체가 StartBtn이고 마우스가 눌렸다면 게임 시작신 로드
            {
                SceneManager.LoadScene("StartScene");
            }
        }
    }
}
