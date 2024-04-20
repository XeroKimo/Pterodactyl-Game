using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public new MeshRenderer renderer;
    private Vector2 offset;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTextureOffset -= Vector2.right * Time.deltaTime * 0.2f;
    }
}
