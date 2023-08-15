using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageScroll : MonoBehaviour
{

  [SerializeField] private RawImage scrollingImage;
  [SerializeField] private float _x, _y;
    // Update is called once per frame
    void Update()
    {
        scrollingImage.uvRect = new Rect (scrollingImage.uvRect.position + new Vector2(_x,_y)*Time.deltaTime, scrollingImage.uvRect.size);
    }
}
