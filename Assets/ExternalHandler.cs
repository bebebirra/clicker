using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExternalHandler : MonoBehaviour, IExternalClickerHandler
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private int Width;
    [SerializeField] private int Height;
    [SerializeField] private Color textureColor = Color.grey;
    [SerializeField] private Color circleColor = Color.red;
    [SerializeField] private int radius = 15;
    [SerializeField] private Image agreeButton;
    [SerializeField] private Image cancelButton;
    [SerializeField] private GameObject agreeStamp;
    [SerializeField] private GameObject cancelStamp;
    [SerializeField] private Image selected;
    [SerializeField] private Image sendButton;
    [SerializeField] private RectTransform targetRect;

    public void Start()
    {
        setSelected(agreeButton);
        initRawImage();
    }

    public void switchSelected()
    {
        setSelected();
    }

    private void setSelected(Image nextSelected = null)
    {
        if (selected is null)
        {
            if (nextSelected != null)
            {
                selected = nextSelected == agreeButton ?
                    cancelButton :
                    agreeButton;
            }
            else
            {
                selected = agreeButton;
            }
        }

        if (nextSelected is null)
        {
            nextSelected = selected == agreeButton ?
                cancelButton :
                agreeButton;
        }



        selected.GetComponent<Button>().interactable = false;

        selected = nextSelected;

        selected.GetComponent<Button>().interactable = true;
    }

    private void initRawImage()
    {
        RectTransform rectTransform = rawImage.GetComponent<RectTransform>();
        Width = (int)rectTransform.sizeDelta.x;
        Height = (int)rectTransform.sizeDelta.y;

        Color[] fillPixels = new Color[Width * Height];
        for (int i = 0; i < Width * Height; i++) fillPixels[i] = textureColor;

        Texture2D texture = new Texture2D(Width, Height);
        texture.SetPixels(fillPixels);

        texture.Apply();

        rawImage.texture = texture;
        rawImage.color = Color.white;
    }

    public void HandleOnPointerClick(Transform sender, PointerEventData pointerEventData, Vector2 correctedPoint)
    {
        RawImage image = sender.gameObject.GetComponent<RawImage>();
        if (image != null)
        {
            //(image.texture as Texture2D).DrawCircle(circleColor, (int)correctedPoint.x, (int)correctedPoint.y, radius);

            GameObject template = selected == agreeButton ?
                agreeStamp :
                cancelStamp;

            addChildOnPosition(sender as RectTransform, template, correctedPoint);
        }
    }

    private void addChildOnPosition(RectTransform parent, GameObject template, Vector2 position)
    {
        Vector3 _position = new Vector3(position.x, position.y, 10);

        GameObject stamp = Instantiate(template,
            Vector2.zero,
            new Quaternion(0f, 0f, 0f, 0f),
            parent);

        RectTransform nRect = stamp.transform as RectTransform;

        int halfWidth = (int)nRect.sizeDelta.x / 2;
        int halfHeight = (int)nRect.sizeDelta.y / 2;
        int frameWidth = (int)parent.sizeDelta.x;
        int frameHeight = (int)parent.sizeDelta.y * -1;

        if (_position.x - halfWidth < 0)
        {
            _position.x = halfWidth;
        }
        else if (_position.x + halfWidth > frameWidth)
        {
            _position.x = frameWidth - halfWidth;
        }

        if (_position.y + halfHeight > 0)
        {
            _position.y = -halfHeight;
        }
        else if (_position.y - halfHeight < frameHeight)
        {
            _position.y = frameHeight + halfHeight;
        }

        (stamp.transform as RectTransform).anchoredPosition = new Vector2(_position.x, _position.y);

        stamp.SetActive(true);
    }

    public void TakeScreenshot()
    {
        string savePath = $"{Application.persistentDataPath}{Path.DirectorySeparatorChar}{DateTime.Now:yyyy_MM_dd_HH.mm.ss.fff}.png";

        StartCoroutine(TakeRectSnapshot(
            targetRect,
            savePath));
    }

    //https://forum.unity.com/threads/taking-screenshot-of-partial-area.54189/
    public IEnumerator TakeRectSnapshot(RectTransform target, string savePath)
    {
        yield return new WaitForEndOfFrame();
        Vector3[] corners = new Vector3[4];
        target.GetWorldCorners(corners);

        int width = ((int)corners[3].x - (int)corners[0].x);
        int height = (int)corners[1].y - (int)corners[0].y;
        var startX = corners[0].x;
        var startY = corners[0].y;

        Texture2D ss = new Texture2D(width, height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        ss.Apply();

        byte[] byteArray = ss.EncodeToPNG();
        
        File.WriteAllBytes(savePath, byteArray);

        Destroy(ss);
    }

}