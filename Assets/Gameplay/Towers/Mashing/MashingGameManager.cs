using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MashingGameManager : MonoBehaviour
{
    public float score;
    [SerializeField] float maxScore;
    [SerializeField] float scoreIncreaseRate;
    [SerializeField] float scoreDecreaseRate;
    [SerializeField] Slider scoreProgress;
    [SerializeField] private Animator playerAnim;
    private bool isResetting = false;

    void Start()
    {
        scoreProgress.maxValue = maxScore;
    }

    void Update()
    {
        if (score >= maxScore) //if we have reached the max score
        {
            if (playerAnim) playerAnim.SetTrigger("Success");
            
            //play sound
            //particles
            isResetting = true;
        }

        //Limit score to minimum
        if (score < 0) score = 0;
        
        //Decrease Score
        score -= scoreDecreaseRate * Time.deltaTime;
        if (isResetting)
            score -= scoreDecreaseRate * 16 * Time.deltaTime ; // decrease score greatly if we are resetting
        if (score <= 0)
            isResetting = false;

        //Set slider UI
        scoreProgress.value = score;
    }

    public void Mashed()
    {
        if (!isResetting) //are we not resetting
        {
            //Increase score when mashing
            score += scoreIncreaseRate;

            //Limit score to maximum
            if (score > maxScore) score = maxScore;
        }
    }
}
