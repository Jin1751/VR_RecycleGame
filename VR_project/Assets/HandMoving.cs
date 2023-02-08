/*
 * 스크립트 이름: HandMoving.cs
 * 연결된 신: 게임 진행 신
 * 연결된 오브젝트: 메인 카메라
 * 기능1: 목표물을 주시 후 클릭하면 팔 포인트(ArmPoint) 사이즈를 늘려 팔(Arm)의 길이를 증가시킨다.
 * 기능2: 팔이 움직이는 동안 새로운 곳으로 팔이 이동되지 않게 한다.
 * 기능3: 손(hand)에 물체가 있다면 앞쪽 방향으로 던진다.
 * 기능4: 획득한 점수를 출력한다.
 * 기능5: 점수에 따라 피버타임을 실행시킨다.
 * 기능6: 목표점수에 도달하면 게임 정지 후 스테이지 이동버튼을 활성화 한다.
 * 기능6: 체력바를 관리한다.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class HandMoving : MonoBehaviour
{
    RaycastHit hit;//레이캐스트에 부딪힌 오브젝트
    public GameObject Armpoint;//팔 길이를 조절할 Empty 오브젝트
    public GameObject Arm;//ArmPoint의 자식으로 게임에서 늘어나는 팔
    public GameObject hand;//손모양 오브젝트, 물체를 붙이고 던진다
    public float speed = 2.0f;//팔 길이가 늘어나고 줄어드는 속도
    public bool objDetect = false;//레이캐스트에 물체가 감지됐는지 확인하는 변수
    public bool isHit = false;//손이 목표물에 닿았는지 판별하는 변수
    private bool isMoving = false;//팔이 움직이는 중인지 판별하는 변수
    public bool isHolding = false;//손에 물체가 들려있는지 확인하는 변수
    private Vector3 defaultSize = new Vector3(1.0f, 1.0f, 1.0f);//원래 팔의 길이

    public int score = 0;//점수를 기록할 변수
    public Text scoreTxt;//점수를 표시할 텍스트
    public Image feverGauge;//피버 타임을 표시할 이미지
    private float feverScore = 1000.0f;//피버타임에 필요한 물건 개수 * 점수
    private int endFever = 0;//이전 피버타임에서의 점수
    public Text feverTxt;//피버타임을 표시할 텍스트
    private float feverTime = 10.0f;//피버타임 시간
    public bool isFever = false;//피버타임인지 체크할 변수
    public int combo = 0;//콤보 횟수 변수
    public bool combo_ing = false;//콤보 추가점수를 판별할 변수
    public Text comboTxt;//콤보를 표시할 텍스트
    public GameObject nextStage;//스테이지 이동 버튼
    public Text stageTxt;//스테이지 이동 버튼 텍스트

    public Image healthBar;//체력바 이미지
    private int health;//체력 변수
    private int beforeScore;//이전 프레임 점수 기록으로 점수에 변화가 일어났는지 체크할 변수
    private float timeLimit;//스테이지 시간 변수
    public Image timeClock;//스테이지 남은 시간을 표시할 이미지
    public GameObject spawn;//스폰 포인트 오브젝트

    public AudioSource audioSource;
    public AudioClip grab_sound;

    
    // Start is called before the first frame update
    void Start()
    {
        feverGauge.enabled = true;//피버게이지 이미지 활성화
        beforeScore = 0;//이전 프레임 점수 0점으로 초기화
        health = 100;
        feverTxt.enabled = false;
        timeLimit = 100.0f;//스테이지를 30초동안 플레이
    }

    // Update is called once per frame
    void Update()
    {
        
        if (score > 0)//점수가 0점 이상일 시
        {
            scoreTxt.text = "Score: " + score + " / 5000";//현재 점수 출력
        }
        else//점수가 0점 미만일 시
        {
            scoreTxt.text = "Score: " + 0 + " / 5000";//0점으로 출력
        }
        ThrowAndDetect();//손에 있는 물체를 던지거나 전방에 물체가 있는지 확인하는 함수
        ArmStrach();//감지된 전방 목표물까지 팔을 늘리고 원래 위치로 줄이는 함수
        FeverTime();//피버타임 적용 함수
        
        if (timeLimit <= 0)//시간이 오버되면 스테이지 끝
        {
            EndStage();//스테이지 마무리 함수
        }
        else
        {
            
            if (score >= 5000 || health <= 0)//정해진 스테이지 점수 도달 또는 체력 0일때 스테이지 끝
            {
                EndStage();//스테이지 마무리 함수
            }
            else
            {
                timeLimit = timeLimit - Time.deltaTime;//시간 감소
                timeClock.fillAmount = timeLimit / 100.0f;//100초에 비례해 시간 표시 이미지 삭제
                nextStage.SetActive(false);//스테이지 이동 버튼 비활성화
            }
        }
        HealthAndScore();//체력바 관리 함수
    }

    void ThrowAndDetect()//손에 있는 물체를 던지거나 전방에 물체가 있는지 확인하는 함수
    {
        Debug.DrawRay(transform.position, transform.forward * 30.0f, Color.red);//전방의 레이캐스트 위치 확인
        if (Input.GetMouseButtonDown(0) && !isMoving)//팔이 움직이지 않은 상태에서 마우스 버튼이 눌리면 실행
        {

            if (isHolding)//손에 물체가 있다면 던진다
            {

                hand.GetComponent<HandPosition>().throwT = true;
                isHolding = false;
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit))//손에 물체가 없으면 레이캐스트 발사
            {
                audioSource.PlayOneShot(grab_sound);
                objDetect = true;
                isMoving = true;
            }
        }
    }

    void ArmStrach()//팔을 늘리고 줄이는 함수
    {
        if (objDetect && !isHit)//레이캐스트에 물체가 감지됐지만 손에 물체가 닿지않았으면 팔을 물체까지 늘림
        {
            
            if (!isHolding)
            {
                
                Armpoint.transform.LookAt(hit.point);//팔이 목표물을 향하게 함
                Armpoint.transform.localScale = new Vector3(Armpoint.transform.localScale.x, Armpoint.transform.localScale.y, Armpoint.transform.localScale.z + 3.0f * Time.deltaTime * speed);//목표물까지 팔 포인트를 정해진 속도로 증가시킨다
            }
        }
        else if (objDetect && isHit)//레이캐스트에 감지된 물체에 손이 닿았으면 손을 원래 위치로 돌아오게 함
        {
            if (Armpoint.transform.localScale.z <= defaultSize.z)//원래 사이즈보다 팔이 짧거나 같다면 기본 사이즈로 재조정, 충돌감지, 움직임감지, 레이캐스트 감지 변수 false로 초기화
            {
                Armpoint.transform.localScale = defaultSize;//짧아진 팔을 기본 사이즈로 조정
                Armpoint.transform.rotation = this.GetComponentInParent<Transform>().rotation;//기본 각도로 조정
                objDetect = false;//레이캐스트 감지 false
                isHit = false;//손 충돌 감지 false
                isMoving = false;//움직임 감지 false
            }
            else//팔 길이가 기본 길이보다 길다면 정해진 속도만큼 줄임
            {
                Armpoint.transform.localScale = new Vector3(Armpoint.transform.localScale.x, Armpoint.transform.localScale.y, Armpoint.transform.localScale.z - 10.0f * Time.deltaTime * speed);
            }
        }
    }

    void FeverTime()//피버타임 함수
    {
        if (!isFever)//피버타임이 아니면 실행
        {

            if ((score - endFever) >= feverScore)//현재 점수에서 이전 피버타임이 끝날 때까지의 점수(피버타임 이후 점수)가 피버타임 점수에 도달하면 실행
            {
                isFever = true;//피버타임 시작
            }
            else
            {
                feverGauge.fillAmount = (score - endFever) / feverScore;//피버타임 게이지를 채움
            }
        }
        else
        {
            feverTxt.enabled = true;//피버타임 텍스트 활성화
            feverTxt.text = "Fever Time!  SEC: " + feverTime.ToString("F2");//피버타임 시간을 출력
            feverTime -= Time.deltaTime;//피버타임(10초) 서서히 감소
            feverGauge.fillAmount = feverTime / 10.0f;//감소된 피버타임 시간에 맞게 게이지 감소
            if (feverTime <= 0)//피버타임 시간 오버시 실행
            {
                feverTxt.enabled = false;//피버타임 텍스트 비활성화
                endFever = score;//피버타임이 끝날때 점수
                feverTime = 10.0f;//피버타임 시간 초기화
                isFever = false;//피버타임 종료 
            }
        }
    }

    void EndStage()//현재 스테이지 마무리 함수
    {
        RaycastHit hit;//전방 버튼 감지할 레이캐스트
        Vector3 forward = this.transform.TransformDirection(Vector3.forward * 1000);//레이캐스트가 나갈 방향
        string nextScene;
        string btnText;
        if(SceneManager.GetActiveScene().name == "New Scene")
        {
            spawn.GetComponent<stg2_objspawn>().gameEnd = true;//스폰 오브젝트에게 게임이 끝나 스폰을 하지 않아도 됨을 알림
            nextScene = "EndScene";
            btnText = "End Game";
        }
        else
        {
            spawn.GetComponent<ObjSpawn>().gameEnd = true;//스폰 오브젝트에게 게임이 끝나 스폰을 하지 않아도 됨을 알림
            nextScene = "New Scene";
            btnText = "Next Stage";
        }
        if(health <= 0 || timeLimit <= 0)//체력이 0보다 작거나 시간이 오버되면 실패버튼으로 변경
        {
            stageTxt.text = "RETRY";//버튼의 텍스트를 Failed로 변경
            nextStage.transform.gameObject.tag = "Fail";//버튼의 태그를 Fail로 변경
        }
        else
        {
            stageTxt.text = btnText;
        }
        nextStage.SetActive(true);//다음 스테이지 이동 버튼 활성화
        isMoving = true;//팔이 움직이지 못하도록 변수 값 변경
        feverGauge.enabled = false;//피버게이지 비활성화
        feverTxt.enabled = false;//피버 텍스트 비활성화
        isFever = false;//피버 타임 비활성화

        if (Physics.Raycast(this.transform.position, forward, out hit))//레이캐스트 발사 후 걸리는 물체가 있으면 실행
        {
            int totalScore = 0;
            if (hit.collider.CompareTag("Success") && Input.GetMouseButton(0))//레이캐스트에 걸린 물체의 태그가 Success이고 터치가 있었으면 실행
            {
                totalScore = PlayerPrefs.GetInt("score") + score;
                PlayerPrefs.SetInt("score", totalScore);
                PlayerPrefs.Save();
                SceneManager.LoadScene(nextScene);//정해진 게임 신 로드
            }
            else if(hit.collider.CompareTag("Fail") && Input.GetMouseButton(0))//레이캐스트에 걸린 물체의 태그가 Fail이고 터치가 있었으면 실행
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);//스테이지 다시 시작
            }
        }
    }

    void HealthAndScore()//체력바, 점수 관리 함수
    {
        comboTxt.text = "X " + combo;
        if (beforeScore < 0)//이전 점수가 음수였는지 확인
        {
            if (score > beforeScore)//점수가 음수였다가 +1이 됐다면 실행
            {
                score = 1;//점수를 1로 변경 (ex. "-2 => -1"이 아닌  "-2 => 1"로 변경)
            }
            else if (score != beforeScore)//현재 점수가 이전 점수보다 같지않으면 실행
            {
                health -= 20;
                healthBar.fillAmount -= 20.0f / 100.0f;//체력바 감소
            }
        }
        else//이전 점수가 양수였는지 확인
        {
            if (score < beforeScore)//점수가 감소했으면 실행
            {
                health -= 20;
                healthBar.fillAmount -= 20.0f / 100.0f;//체력바 감소
                Debug.Log("FeverMINUS");
                endFever = score;
                feverGauge.fillAmount = 0;
            }  
        }
        beforeScore = score;//다음 프레임 점수와 비교를 위해 이전 프레임 점수 변수 값을 현재 점수로 변경
    }
}
