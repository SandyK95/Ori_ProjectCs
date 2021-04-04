using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprits : MonoBehaviour
{
    public Sprite[] sprites;
    public string spritesName;
    public int numSprite;

    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>(spritesName);
        if (numSprite == -1) numSprite = Random.Range(0, sprites.Length);
        GetComponent<SpriteRenderer>().sprite = sprites[numSprite];
    }
}
