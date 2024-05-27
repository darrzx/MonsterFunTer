using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaladinSpotlight : MonoBehaviour
{
    private PlayerObjectController instance = PlayerObjectController.getInstance();
    public Light PaladinSpotlightAtribut;
    private Animator animator;
    public AudioSource audioSource;
    public GameObject loadingScreen;
    public Slider slider;
    public GameObject animationHover;

    // Start is called before the first frame update
    void Start()
    {
        PaladinSpotlightAtribut.enabled = false;
        animator = GetComponent<Animator>();
        audioSource.Stop();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }

    void OnMouseDown()
    {
        instance.setIndex(1);
        LoadLevel(1);
    }

    void OnMouseEnter()
    {
        PaladinSpotlightAtribut.enabled = true;
        animator.SetTrigger("TrHover");
        audioSource.Play();
        animationHover.SetActive(true);
        audioSource.volume = 1f;
    }

    void OnMouseExit()
    {
        animationHover.SetActive(false);
        PaladinSpotlightAtribut.enabled = false;
        animator.SetTrigger("TrIdle");
        audioSource.Stop();
    }
}
