using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public new MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTextureOffset -= Vector2.right * Time.deltaTime * 0.2f;
    }

    public void Reset()
    {
        renderer.material.mainTextureOffset = Vector2.zero;
    }
}
