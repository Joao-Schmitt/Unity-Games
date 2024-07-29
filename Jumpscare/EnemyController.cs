using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    public AudioSource jumpScareSound;
    public GameObject jumpScareImage;
    public enum Events 
    {
        WalkInRoof,
        JumpScareImage,
        JumpScareAnim,
        LoadGameOver
    }

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PlayAnimWalkInRoof()
    {
        anim.SetTrigger("walk");
        StartCoroutine("DestroyInSeconds", 4);
    }

    public void PlayJumpScareImage()
    {
        jumpScareImage.SetActive(true);
        jumpScareSound.Play();
        StartCoroutine("LoadSceeneInSeconds", 5);
    }

    public void PlayJumpScareAnim()
    {
        jumpScareSound.Play();
        anim.SetTrigger("run");
    }

    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    private IEnumerator DestroyInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private IEnumerator LoadSceeneInSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("GameOver");
    }
}
