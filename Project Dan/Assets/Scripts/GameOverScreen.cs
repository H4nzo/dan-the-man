using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Material mat;

    [SerializeField] float rad = 0;
    Vector3 pos = Vector3.zero;

    Transform player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(sequence());
    }

    void Update()
    {

    }

    public void EndSequence()
    {
        StartCoroutine(sequence_end());
    }
    IEnumerator sequence_end()
    {
        for (float i = 0; i <= 2; i += Time.deltaTime)
        {
            rad = Mathf.Lerp(3f, 0f, i / 2);
            yield return null;
        }
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator sequence()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            rad = Mathf.Lerp(0f, 3f, i);
            yield return null;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        pos = Camera.main.WorldToScreenPoint(transform.position);

        pos.x /= Screen.width;
        pos.x = (pos.x - 0.5f) * 2;
        pos.x *= (float)Screen.width / Screen.height;

        pos.y /= Screen.height;
        pos.y = (pos.y - 0.5f) * 2;

        mat.SetFloat("_Aspect", (float)Screen.width / Screen.height);
        mat.SetVector("_Pos", pos);
        mat.SetFloat("_Rad", rad);

        Graphics.Blit(source, destination, mat);
    }
}
