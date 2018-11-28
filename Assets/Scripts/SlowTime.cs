using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTime : MonoBehaviour {

    private static float slowTotalTime = 1f;
    private static float currentSlowTime;
    
    void Start() {
        currentSlowTime = slowTotalTime;
    }

    void Update () {
        if (currentSlowTime < slowTotalTime) {
            
            currentSlowTime += Time.fixedUnscaledDeltaTime / slowTotalTime;
            Time.timeScale = currentSlowTime / slowTotalTime; // Make sure it's not 0
        }
        else {
            Time.timeScale = 1f;
        }
    }

    public static void SetSlow(float? newSlowTime = null, float timeScaleStartValue = .45f) {
        if (newSlowTime.HasValue) {
            slowTotalTime = newSlowTime.Value;
        }
        
        currentSlowTime = timeScaleStartValue * slowTotalTime;
    }

}
