using UnityEngine;

public class Tile : MonoBehaviour 
{
    public Animation anim;

    public int number;
    public int row;
    public int column;

    public bool isEmpty;
    public bool isMerged;

    void Awake()
    {
        anim = GetComponent<Animation>();
    }

    public void Transform()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
        rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        transform.localPosition = Vector3.zero;
    }

    public void Setup(int number, int row, int column)
    {
        this.number = number;
        this.row = row;
        this.column = column;
    }

    public void PlayMergeAnimation()
    {
        anim.Play("Merge");
    }

    public void PlayAppearAnimation()
    {
        anim.Play("Appear");
    }
}