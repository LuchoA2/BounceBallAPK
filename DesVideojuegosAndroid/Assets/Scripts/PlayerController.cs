using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour


{
    private Rigidbody rb;
    AudioSource audioPlayer;
    public AudioClip soundPoints, jumpSound, DeadSound, winSound;
    float movHorizontal,movVertical;
    public float velocidad = 1.0f;
    public float altitud = 100.0f;
    public bool isJump = false;
    int stars = 0;
    int lifes = 3;
    public Text lifesText, starsText, timeText, finalLifesText, finalStarsText;
    float totalTime = 120f;
    public GameObject startPoint, panelGameOver, panelCongratulations, uiMobile;
    public MenuManager menuManager;
    public Joystick joystick;
    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {
       #if UNITY_ANDROID
       uiMobile.SetActive(true);
       #endif
    rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pause){
        CountDown();
        }
        #if UNITY_ANDROID
        movHorizontal = joystick.Horizontal * 0.52f;
        movVertical = joystick.Vertical * 0.52f;
        #endif
        //movVertical = Input.GetAxis("Vertical");
       // movHorizontal = Input.GetAxis("Horizontal");
        
    Vector3 movimiento = new Vector3(movHorizontal, 0.0f, movVertical);
    rb.AddForce(movimiento * velocidad);
    if(Input.GetKey(KeyCode.Space) && (!isJump)){
        Jump();
    }
        }
    

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.name == "Floor" || collision.gameObject.name == "Wood"){
            isJump = false;
        }
        if(collision.gameObject.name == "GreatAxe"){
            transform.position = startPoint.transform.position;
            lifes -= 1;
            lifesText.text = "0" + lifes.ToString();
            if(lifes == 0){
                GetComponent<AudioSource>().clip = DeadSound;
                GetComponent<AudioSource>().Play();
                GameOverGame();
            }
        }
    }
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.name == "Star"){
            Destroy(collider.gameObject);
            stars += 1;
            starsText.text = "0" + stars.ToString();
            GetComponent<AudioSource>().clip = soundPoints;
            GetComponent<AudioSource>().Play();
            
        }
        if(collider.gameObject.name == "DeadZone"){
            transform.position = startPoint.transform.position;
            lifes -= 1;
            lifesText.text = "0" + lifes.ToString();
            GetComponent<AudioSource>().clip = DeadSound;
            GetComponent<AudioSource>().Play();
            if(lifes == 0){
                GetComponent<AudioSource>().clip = DeadSound;
                GetComponent<AudioSource>().Play();
                GameOverGame();
            }
        }
        if(collider.gameObject.name == "Final"){
            GetComponent<AudioSource>().clip = winSound;
                GetComponent<AudioSource>().Play();
            FinishedGame();
        }
    }
    void CountDown(){
        totalTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime - (minutes * 60));
        timeText.text = string.Format("{0:0}:{01:00}",minutes, seconds);
        if((minutes == 0) && (seconds == 0)){
            GetComponent<AudioSource>().clip = DeadSound;
            GetComponent<AudioSource>().Play();
            GameOverGame();
        }
    }

    public void PauseGame(){
        pause = !pause;
        rb.isKinematic = pause;
    }
    void GameOverGame(){
       
        menuManager.GoToMenu(panelGameOver);
          PauseGame();
       
    }
    public void RestartGame(){
    
    transform.position = startPoint.transform.position;
    totalTime = 120f;
    lifes = 3;
    stars = 0;
    lifesText.text = "03";
    starsText.text = "00";
    rb.isKinematic = false;
     pause = false;
    }
    void FinishedGame(){
        PauseGame();
        menuManager.GoToMenu(panelCongratulations);
        finalLifesText.text = "0" + lifes.ToString();
        finalStarsText.text = "0" + stars.ToString();
    }
    public void Jump(){
        if(!isJump){
         Vector3 salto = new Vector3(0,altitud,0);
        rb.AddForce(salto * velocidad);
        isJump = true;
        GetComponent<AudioSource>().clip = jumpSound;
        GetComponent<AudioSource>().Play();
        }
    }
}
