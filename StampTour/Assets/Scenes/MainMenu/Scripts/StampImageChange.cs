using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StampImageChange : MonoBehaviour
{
    // ������ ��� ����� �̹���
    [SerializeField]
    Sprite changedImage;

    // Ȯ�ο� ������ �÷��� �̸���
    [SerializeField]
    string[] flagNames;

    // ���� �̹���
    Sprite originalImage;

    // Image ������Ʈ
    Image stampImage;

    private void Awake()
    {
        // ���� �̹��� ����
        stampImage = GetComponent<Image>();
        originalImage = stampImage.sprite;  
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(string flagName in flagNames)
        {
            try
            {
                // �÷��װ� �����ϰ�, �������� �Ϸ� �Ǿ����� Ȯ��
                if (GameManager.Instance.IsContainKey(flagName) && GameManager.Instance.GetIsSceneFinished(flagName))
                {
                    // �̹��� ����
                    stampImage.sprite = changedImage;

                    //���� ������ �� �̻� �ݺ� �ʿ� ��� ��ȯ
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception occurred while checking flag {flagName}: {e.Message}");
            }
            
        }

        // ��� �̸��� Ȯ���ص� ������ ���� �̹����� ����
        stampImage.sprite = originalImage;
    }
}
