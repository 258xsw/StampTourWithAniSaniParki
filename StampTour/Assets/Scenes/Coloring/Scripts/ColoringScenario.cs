using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Unity.Collections.AllocatorManager;

public class ColoringScenario : MonoBehaviour
{
    public Image CharacterSelectUI;
    public GameObject coloring;
    public GameObject[] character = new GameObject[3];
    public TextMeshProUGUI talk_Bubble;

    public void OnPalette()
    {
        string click = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log(click);
        CharacterSelectUI.gameObject.SetActive(false);
        if (click == "Ani")
            character[0].SetActive(true);
        else if (click == "Sani")
            character[1].SetActive(true);
        else if (click == "Bani")
            character[2].SetActive(true);
        coloring.SetActive(true);
        talk_Bubble.text = "���ϴ� ��������\nĥ�غ���!";
    }
    public void ReturnButton()
    {
        coloring.SetActive(false);
        character[0].SetActive(false);
        character[1].SetActive(false);
        character[2].SetActive(false);
        talk_Bubble.text = "��ĥ���̸� �ϰ� ���� ĳ���͸� �����!";
        CharacterSelectUI.gameObject.SetActive(true);



    }
}
