using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    public void HoverEffect() => LeanTween.scale(gameObject, new Vector3(1.1f, 1.1f, 1.1f), 0.2f).setEaseLinear();
    public void UnhoverEffect() => LeanTween.scale(gameObject, new Vector3(1.0f, 1.0f, 1.0f), 0.5f).setEaseOutElastic();
}
