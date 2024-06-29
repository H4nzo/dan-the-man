using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseButton;

    public GameObject pauseSettings;
    public Animator pauseContainer;

    public const string animatorState = "isOpen";
    public bool isPaused = false;

    public GameObject canvasHUD;
    public GameObject canvasControls;

    public void Pause()
    {
        isPaused = true;

        pauseButton.SetActive(false);
        canvasHUD.SetActive(false);
        canvasControls.SetActive(false);

        pauseSettings.SetActive(true);
        pauseContainer.SetBool(animatorState, true);
    }


    public void Resume()
    {
        isPaused = false;
        StartCoroutine(animState(1f, false));

    }

    IEnumerator animState(float t, bool state)
    {
        pauseContainer.SetBool(animatorState, state);

        yield return new WaitForSeconds(t);

        pauseButton.SetActive(!state);
        pauseSettings.SetActive(state);
        canvasHUD.SetActive(!state);
        canvasControls.SetActive(!state);


    }

    private void Update()
    {
        if(isPaused == true){
            Time.timeScale = 0f;
            
        }
        else
        {
             Time.timeScale = 1f;
        }
    }
}
