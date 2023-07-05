using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using UnityEngine.Serialization;
using MoreMountains.CorgiEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float timeToCompleteLevel = 5f;
    [SerializeField] float timeBetweenDeaths = 3f;
    [SerializeField] Image TimerImage;
    public float fillFraction;
    Character player;
    Health playerHealth;

    public bool isAlive;

    
    float timerValue;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        playerHealth = player.gameObject.GetComponent<Health>();
        isAlive = true;
        timerValue = timeToCompleteLevel;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer(){
        timerValue = 0;
    }

    void UpdateTimer(){

        if(isAlive){
            if(timerValue > 0){
                timerValue -= Time.deltaTime;
                fillFraction = timerValue / timeToCompleteLevel;
                TimerImage.fillAmount = fillFraction;
            } else {
                LevelManager.Instance.KillPlayer(player);
                isAlive = false;
                timerValue = timeBetweenDeaths;
                TimerImage.fillAmount = 1;
            }
        } else {
            timerValue -= Time.deltaTime;
            if(timerValue <= 0){
                isAlive = true;
                timerValue = timeToCompleteLevel;
            }
        }
    }
}
