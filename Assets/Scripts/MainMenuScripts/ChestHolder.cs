using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHolder : MonoBehaviour
{
    public List<GameObject> chest;
    public float move = 450f;


    public List<GameObject> screenChest;
    public float startPos;

    public int currentChest = 1;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.localPosition.x;
    }

    public void Left()
    {
        rectTransform.position = new Vector2(rectTransform.localPosition.x - move, rectTransform.position.y);
    }

    public void Right()
    {
        rectTransform.position = new Vector2(rectTransform.localPosition.x + move, rectTransform.position.y);
    }


}
