using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.XR.ARSubsystems;

public class PhotoCapture : MonoBehaviour
{
    public RenderTexture characterTexture; 
    public GameObject copyCharacter;
    public GameObject captureImagePos;
    public string characterName;
    public Camera camera;
    public Rect captureArea;
    public GameObject photoFrame;
    public Image photo;
    public AudioClip captureAudio;
    public AudioClip showAudio;
    public GameObject CameraImage;
    public GameObject ResultUI;
    public RectTransform CamRect;

    AudioSource audioSource;
    Texture2D screenImage;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        captureArea.width = Screen.width;
        captureArea.height = Screen.height;
    }

    public void CharacterTakeScreenShot()
    {
        //captureCanvas�� �ڽ� ������Ʈ�� ����
        if (captureImagePos.transform.childCount > 0) 
            Destroy(captureImagePos.transform.GetChild(0).gameObject);

        //����� �̹��� ����
        Instantiate(copyCharacter, captureImagePos.transform);

        RenderTexture.active = characterTexture;
        if (characterTexture != null)
        {
            //�̹��� �����
            Texture2D Image = new Texture2D(characterTexture.width, characterTexture.height);
            Image.ReadPixels(new Rect(0, 0, characterTexture.width, characterTexture.height), 0, 0);
            Image.Apply();

            byte[] pngBytes = Image.EncodeToPNG();
            SavePNG(pngBytes);
            RenderTexture.active = null;
            characterTexture.Release();
        }
    }
    public void TakeScreenShot()
    {
       
        int width = (int)captureArea.width;
        int height = (int)captureArea.height;
        // int width2 = (int)CamRect.rect.height;
        // int height2 = (int)CamRect.rect.width;
        Debug.Log(width);

        // ������ ������ �´� RenderTexture ����
        RenderTexture renderTexture = new RenderTexture(width, height, 24);
        camera.targetTexture = renderTexture;
        camera.Render();

        // ���� RenderTexture�� Ȱ�� ���� ����
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Texture2D�� RenderTexture ������ ����
        screenImage = new Texture2D(width, height, TextureFormat.RGB24, false);
        // captureArea ���� ĸ��
        screenImage.ReadPixels(new Rect(0, 0, width, height), (int)captureArea.x, (int)captureArea.y);
        screenImage.Apply();
        
        
        // RenderTexture ��Ȱ��ȭ �� ī�޶� Ÿ�� �ʱ�ȭ
        camera.targetTexture = null;
        RenderTexture.active = previous;
        Destroy(renderTexture);

        // PNG�� ���ڵ� �� ����
        byte[] imageBytes = screenImage.EncodeToPNG();
        screenImage.LoadImage(imageBytes);
        // SavePNG(imageBytes);
        SaveToGallery();
    }
    void SavePNG(byte[] pngArray)
    {
        Debug.Log("Picture taken");
        string path = Path.Combine(Application.persistentDataPath, characterName + ".png");
        File.WriteAllBytes(path, pngArray);
        Debug.Log(path);
    }
    public void SaveToGallery()
    {
        // do something with texture
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (permission == NativeGallery.Permission.Denied)
        {
            if (NativeGallery.CanOpenSettings())
            {
                NativeGallery.OpenSettings();
            }
        }
        float x = (captureArea.width - CamRect.rect.height)/2;
        Debug.Log(x);
        Debug.Log(CamRect.rect.height);
        Sprite sprite = Sprite.Create(screenImage, new Rect((captureArea.width - CamRect.rect.height)/2, (captureArea.height - CamRect.rect.width)/2, CamRect.rect.height, CamRect.rect.width), Vector2.zero);
        photo.sprite = sprite;
        audioSource.clip = captureAudio;
        audioSource.Play();
        Invoke("TakePhoto", 1f);
    }
    void TakePhoto()
    {
        CameraImage.SetActive(false);
        photoFrame.SetActive(true);
    }
    public void RetakePhoto()
    {
        CameraImage.SetActive(true);
        photoFrame.SetActive(false);
        audioSource.clip = showAudio;
        audioSource.Play();
    }
    public void EndGame()
    {
        NativeGallery.SaveImageToGallery(screenImage, "AnsanIndustrialHistoryMuseum", "�ƴϻ�Ϲٱ�� �Բ� �������");
        photoFrame.SetActive(false);
        ResultUI.SetActive(true);
    }


}
