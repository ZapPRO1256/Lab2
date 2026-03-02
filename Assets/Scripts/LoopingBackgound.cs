using UnityEditor.Rendering;
using UnityEngine;

public class LoopingBackgound : MonoBehaviour
{
    public float backSpeed;
    public Renderer backrenderer;

    void Update()
    {
        backrenderer.material.mainTextureOffset += new Vector2(0f, backSpeed * Time.deltaTime);


    }
}
