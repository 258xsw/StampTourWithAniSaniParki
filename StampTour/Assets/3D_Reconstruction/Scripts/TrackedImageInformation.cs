namespace RapidFramework
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.XR.ARFoundation;
    using UnityEngine.XR.ARSubsystems;
    public partial class TrackedImageInformation : MonoBehaviour //Inner
    {
        public class ARTrackedImageTimerData
        {
            public ARTrackedImage ARTrackedImage;
            public float LimitTimer = 0;
        }
    }
    public partial class TrackedImageInformation : MonoBehaviour //Data
    {
        
        //Key�� ARTrackedImage üŷ��, Value�� Ÿ�̸� ��
        private Dictionary<ARTrackedImage, ARTrackedImageTimerData> arTrackedImageTimerDataDictionary = default; //arTrackedImage Dictionary
        private List<ARTrackedImage> removeARTrackedImageTimerList = default;
        //Main
        private Dictionary<string, GameObject> ARF_GestureDictionary = default; // ���� ��ųʸ�
        [SerializeField] private Camera ARF_Camera; //AR Fondation Camera
        [SerializeField] ARTrackedImageManager m_TrackedImageManager; //Ʈ��ŷ �Ŵ���
        [SerializeField] private List<GameObject> ARF_GestureObjectList; // ������ ������Ʈ ����Ʈ
        [Header("Property")]
        [SerializeField] private float limitTimer = 1f; //���ѽð�
        [SerializeField] private float limitDistance = 100; //ī�޶� �Ÿ� ����
    }
    public partial class TrackedImageInformation : MonoBehaviour //�ʱ� ����
    {
        private void Allocate()
        {
            //�Ҵ�
            arTrackedImageTimerDataDictionary = new Dictionary<ARTrackedImage, ARTrackedImageTimerData>();
            removeARTrackedImageTimerList = new List<ARTrackedImage>();
            ARF_GestureDictionary = new Dictionary<string, GameObject>();
        }
        public void Initialize()
        {
            Allocate();
            Setup();
        }
        private void Setup()
        {
            for(int index = 0; index < ARF_GestureObjectList.Count; index++)
            {
                ARF_GestureDictionary.Add(ARF_GestureObjectList[index].name, ARF_GestureObjectList[index]);
            }
            
        }
    }

    public partial class TrackedImageInformation : MonoBehaviour // ����
    {
        private void Start()
        {
            Initialize();
        }
        private void Update()
        {
            Progress();
        }
        private void Progress() //Ʈ��ŷ �̹��� Ÿ�̸� ���
        {
            if (arTrackedImageTimerDataDictionary.Count != 0)
            {
                //����Ʈ ��ȯ
                List<ARTrackedImageTimerData> arTrackedImageTimerDataList = arTrackedImageTimerDataDictionary.Values.ToList();

                for (int index = 0; index < arTrackedImageTimerDataList.Count; index++)
                {
                    //�ð����� ����
                    Progress_LimitTimer(arTrackedImageTimerDataList[index]);

                    //�Ÿ��� ���� ����
                    Progress_LimitDistance(arTrackedImageTimerDataList[index]);
                }

                if (removeARTrackedImageTimerList.Count != 0) //���ŵ� ��ü�� �ִ���
                {
                    for (int index = 0; index < removeARTrackedImageTimerList.Count; index++)
                    {
                        //���� �ִ��� üũ
                        if (arTrackedImageTimerDataDictionary.ContainsKey(removeARTrackedImageTimerList[index]) == true)
                        {
                            //������ ����
                            arTrackedImageTimerDataDictionary.Remove(removeARTrackedImageTimerList[index]);
                        }
                    }
                    //���� ����Ʈ ����
                    removeARTrackedImageTimerList.Clear();
                }
            }
        }
        void OnCompleteButtonClick()
        {
            if (GameManager.Instance.GetIsSceneFinished("3D_Reconstruction"))
            {
                GameManager.RollbackMainScene();
            }
        }
        private void Progress_LimitTimer(ARTrackedImageTimerData arTrackedImageTimerData)
        {

            if (arTrackedImageTimerData.ARTrackedImage.trackingState == TrackingState.Limited)
            {
                //�ð��� ���� �κ� 
                if (arTrackedImageTimerData.LimitTimer >= limitTimer) //�ð��� ���Խð��� �ʰ��ߴ���
                {
                    string trackedImageName = arTrackedImageTimerData.ARTrackedImage.referenceImage.name;

                    if (trackedImageName != null && ARF_GestureDictionary.TryGetValue(trackedImageName, out GameObject ARF_GestureObject) == true)
                    {
                        ARF_GestureObject.SetActive(false);
                    }

                    removeARTrackedImageTimerList.Add(arTrackedImageTimerData.ARTrackedImage);
                    GameManager.Instance.SetIsSceneFinished("3D_Reconstruction", true);
                    //OnCompleteButtonClick();


                }
                else //���� �ð��� �ȵǾ����� �ð� ����
                {
                    arTrackedImageTimerData.LimitTimer += Time.deltaTime;
                }
            }
        }
        private void Progress_LimitDistance(ARTrackedImageTimerData arTrackedImageTimerData)
        {

            if (arTrackedImageTimerData.ARTrackedImage.trackingState == TrackingState.Tracking)
            {
                string trackedImageName = arTrackedImageTimerData.ARTrackedImage.referenceImage.name;

                if (trackedImageName != null && ARF_GestureDictionary.TryGetValue(trackedImageName, out GameObject ARF_GestureObject) == true)
                {
                    //ī�޶�� �Ÿ��� ����.
                    float distance = Vector3.Distance(ARF_Camera.gameObject.transform.position, ARF_GestureObject.transform.position);

                    if (distance > limitDistance) //�Ÿ��� ����ٸ�
                    {
                        ARF_GestureObject.SetActive(false);

                        //���� ��� �߰�
                        removeARTrackedImageTimerList.Add(arTrackedImageTimerData.ARTrackedImage);
                        GameManager.Instance.SetIsSceneFinished("3D_Reconstruction",true);
                        //OnCompleteButtonClick();
                    }
                }
            }
        }
    }
    public partial class TrackedImageInformation : MonoBehaviour // OnEnable || Disable 
    {
        private void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

        private void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    }
    public partial class TrackedImageInformation : MonoBehaviour //�Ӽ�
    {
        private void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
        {
            foreach (var newImage in eventArgs.added)
            {
                NewImage(newImage);
            }

            foreach (var updatedImage in eventArgs.updated)
            {
                UpdateImage(updatedImage);
            }

            foreach (var removedImage in eventArgs.removed)
            {
            }
        }
        private void NewImage(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState != TrackingState.None)
            {
                // �̹��� ũ�⿡ �°� ������ ������
                trackedImage.transform.localScale = new Vector3(trackedImage.size.x, trackedImage.size.x, trackedImage.size.y);
            }

            //��ųʸ� ������ ���� ����
            if (arTrackedImageTimerDataDictionary.ContainsKey(trackedImage) == false) //���ԵǾ� ���� �ʴٸ�
            {
                ARTrackedImageTimerData arTrackedImageTimerData = new ARTrackedImageTimerData();
                arTrackedImageTimerData.ARTrackedImage = trackedImage;
                arTrackedImageTimerData.LimitTimer = 0;

                arTrackedImageTimerDataDictionary.Add(arTrackedImageTimerData.ARTrackedImage, arTrackedImageTimerData);      
            }
        }
        private void UpdateImage(ARTrackedImage trackedImage)
        {
            if (trackedImage.trackingState != TrackingState.None)
            {
                // �̹��� ũ�⿡ �°� ������ ������
                trackedImage.transform.localScale = new Vector3(trackedImage.size.x, trackedImage.size.x, trackedImage.size.y);
            }

            //��ųʸ� ������ ���� ����
            if (arTrackedImageTimerDataDictionary.ContainsKey(trackedImage) == false) //���ԵǾ� ���� �ʴٸ�
            {
                ARTrackedImageTimerData arTrackedImageTimerData = new ARTrackedImageTimerData();
                arTrackedImageTimerData.ARTrackedImage = trackedImage;
                arTrackedImageTimerData.LimitTimer = 0;

                arTrackedImageTimerDataDictionary.Add(arTrackedImageTimerData.ARTrackedImage, arTrackedImageTimerData);
            }
            else
            {
                if (arTrackedImageTimerDataDictionary[trackedImage].ARTrackedImage.trackingState != TrackingState.Limited) //Ʈ��ŷ�� ���
                {
                    arTrackedImageTimerDataDictionary[trackedImage].LimitTimer = 0;

                    //Ʈ��ŷ�� ��츸 ����ȭ
                    string trackedImageName = trackedImage.referenceImage.name;

                    if (trackedImageName != null && ARF_GestureDictionary.TryGetValue(trackedImageName, out GameObject ARF_GestureObject) == true)
                    {
                        ARF_GestureObject.transform.position = trackedImage.transform.position;
                        ARF_GestureObject.transform.rotation = trackedImage.transform.rotation;
                        ARF_GestureObject.transform.localScale = trackedImage.transform.localScale;
                        ARF_GestureObject.SetActive(true);
                        GameManager.Instance.SetIsSceneFinished("3D_Reconstruction", true);
                    }

                }
            }
        }
    }
}
