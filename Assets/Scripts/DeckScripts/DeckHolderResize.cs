using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckHolderResize : MonoBehaviour
{
    // Start is called before the first frame update
    int baseWidth = 450;
    void Start()
    {
        StartCoroutine(SetWidth(.8f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SetWidth(float time)
    {
        yield return new WaitForSeconds(time);
        RectTransform rectTransform = GetComponent<RectTransform>();
        var child = GetComponentsInChildren<SingleDeckHelper>();
        int i = child.Length;

        rectTransform.sizeDelta = new Vector2(i * baseWidth, baseWidth);
    }

    public void DeactivateAllDecks()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        var child = GetComponentsInChildren<SingleDeckHelper>();
        foreach(var c in child)
        {
            c.GetComponent<SingleDeckHelper>().ShadowSetInActive();
        }
    }

}
