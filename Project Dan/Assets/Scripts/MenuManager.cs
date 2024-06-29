using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
  public GameObject settingsPanel;
  public GameObject settingsCloseButton;
  public GameObject controlsPanel;

  public Animator settingsAnimator;
  public Animator controlsAnimator;
  public const string animatorState = "isOpen";

  public void Play()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  }

  public void Settings()
  {
    //Show Settings UI
    settingsPanel.SetActive(true);
    settingsAnimator.SetBool(animatorState, true);
  }

  public void Controls()
  {
    controlsPanel.SetActive(true);
    settingsCloseButton.SetActive(false);
    controlsAnimator.SetBool(animatorState, true);
    
  }

  public void SettingsClose()
  {
    StartCoroutine(animState(1f, false));
  }

  public void ControlsClose()
  {
    StartCoroutine(animControlState(.3f, false));
  }

  public void Exit()
  {
    Application.Quit();
  }

  IEnumerator animState(float t, bool state)
  {
    settingsAnimator.SetBool(animatorState, state);
    yield return new WaitForSeconds(t);

    settingsPanel.SetActive(state);
  }

  IEnumerator animControlState(float t, bool state)
  {
    controlsAnimator.SetBool(animatorState, state);
    yield return new WaitForSeconds(t);

    settingsCloseButton.SetActive(!state);
  }

}
