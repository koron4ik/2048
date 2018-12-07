using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour 
{
    public int number;
    public int row;
    public int column;

    public bool isEmpty;
    public bool isMerged;

	public void Transform()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        transform.localPosition = Vector3.zero;
    }
}