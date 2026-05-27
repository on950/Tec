using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public class Dialogue : MonoBehaviour
{
    private bool isPlayerInRange;
    private bool didDialogueStart;
    private int lineIndex;
    private float typingSpeed = 0.05f;

    [SerializeField] private GameObject dialogueSpace, dialogueEnter;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(4,6)] private string[] dialogueLines;
    [SerializeField] private int sceneIndex;

    // Update is called once per frame
    void Update()
    {
        if(isPlayerInRange&&Input.GetButtonDown("Jump"))
        {
            if (!didDialogueStart)
            {
                StartDialogue();
            }
            else if(dialogueText.text == dialogueLines[lineIndex])
            {
                NextDialogueLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[lineIndex];
            }
        }
        if(isPlayerInRange&&Input.GetButtonDown("Fire1"))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    private void StartDialogue()
    {
        didDialogueStart = true;
        dialoguePanel.SetActive(true);
        dialogueEnter.SetActive(false);
        dialogueSpace.SetActive(false);
        lineIndex = 0;
        Time.timeScale = 0;
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if(lineIndex<dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            didDialogueStart = false;
            dialoguePanel.SetActive(false);
            dialogueEnter.SetActive(true);
            dialogueSpace.SetActive(true);
            Time.timeScale = 1;
        }
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;
        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            dialogueSpace.SetActive(true);
            dialogueEnter.SetActive(true);

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            dialogueSpace.SetActive(false);
            dialogueEnter.SetActive(false);

        }
    }
}
