using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class QuestFinish : MonoBehaviour
{
    
    [SerializeField] private GameObject dialogBox, unfinishedText, finishedText;
    [SerializeField] private int questGoal = 20;
    [SerializeField] private int levelToLoad;
    
    private Animator anim;
    private bool levelIsLoading = false;
    
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogBox.SetActive(true);
            if(other.GetComponent<PlayerMovement>().applesCollected >= questGoal)
            {
                anim.SetTrigger("Flag");
                finishedText.SetActive(true);
                Invoke("LoadNextLevel", 3.0f);
                levelIsLoading = true;
            }
            else
            {
                unfinishedText.SetActive(true);
            }
        }
    }
    
    private void LoadNextLevel()
    {
        SceneManager.LoadScene(levelToLoad);

    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !levelIsLoading)
        {
            (dialogBox).SetActive(false);
            (finishedText).SetActive(false);
            (unfinishedText).SetActive(false);
        }

    }
}
