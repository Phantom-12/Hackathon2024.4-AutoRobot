using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogWindowController : MonoBehaviour
{
    Sprite[] sprites;
    [SerializeField]
    Image image;

    int nowi;

    public void LoadDialog(Sprite[] sprites)
    {
        this.sprites = sprites;
        nowi = 0;
        image.sprite = sprites[nowi];
    }

    public void ShowDialog()
    {
        gameObject.SetActive(true);
    }

    public void NextPage()
    {
        nowi++;
        if (nowi == sprites.Length)
            gameObject.SetActive(false);
        else
            image.sprite = sprites[nowi];
    }
}
