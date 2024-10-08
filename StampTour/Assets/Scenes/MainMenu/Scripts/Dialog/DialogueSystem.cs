using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogueSystem : MonoBehaviour
{
    [SerializeField]
    private Dialogue[] dialogues;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Image dialogueWindow;

    [SerializeField]
    private AudioClip clip;
    private int currentDialogueIndex;
    private bool isFirst;
    public float typingSpeed = 0.1f;
    private bool isTypingEffect = false;

    public void Setup()
    {
        dialogueText.gameObject.SetActive(true);
        if(dialogueWindow != null) dialogueWindow.gameObject.SetActive(true);
        currentDialogueIndex = -1;
        isFirst = true;
    }

    public bool UpdateDialogue()
    {
        if (isFirst)
        {
            SetNextDialogue();
            isFirst = false;
        }
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Began)
            {
                return HandelInput();
            }
        }
        else if(Input.GetMouseButtonDown(0))
        {
            return HandelInput();
        }
        return false;
    }

    private bool HandelInput()
    {
        if(isTypingEffect)
        {
            isTypingEffect = false;

            StopCoroutine(nameof(OnTypingText));
            GameManager.Instance.Source_SFX.Stop();
            dialogueText.text = dialogues[currentDialogueIndex].sentence;
        }
        else if(dialogues.Length > currentDialogueIndex + 1)
        {
            SetNextDialogue();
        }
        else
        {
            EndDialogue();
            return true;
        }
        return false;
    }

    private void SetNextDialogue()
    {
        currentDialogueIndex++;
        GameManager.Instance.PlaySFXOneShot(clip);
        StartCoroutine(nameof(OnTypingText));
    }

    private void EndDialogue()
    {
        dialogueText.gameObject.SetActive(false);
        if (dialogueWindow != null) dialogueWindow.gameObject.SetActive(false);
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        string sentence = dialogues[currentDialogueIndex].sentence;
        int sentenceLength = sentence.Length;

        isTypingEffect = true;

        while (index <= sentenceLength)
        {
            dialogueText.text = dialogues[currentDialogueIndex].sentence.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTypingEffect = false;
        GameManager.Instance.Source_SFX.Stop();
    }

    
}


[System.Serializable]
public struct Dialogue
{
    [TextArea(3, 10)]
    public string sentence;
}