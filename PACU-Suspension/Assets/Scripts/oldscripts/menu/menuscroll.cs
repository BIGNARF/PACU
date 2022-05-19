using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class menuscroll : MonoBehaviour
{
    public Scrollbar bar;
    public void Movedown()
    {        
        float movement = gameObject.transform.position.y - 20 * bar.value;
        gameObject.transform.Translate(0, movement, 0);
    }
}
