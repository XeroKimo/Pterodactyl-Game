using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public new MeshRenderer renderer;
    public float baseSpeed = 0.15f;
    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        renderer.material.mainTextureOffset -= Vector2.right * Time.deltaTime * baseSpeed * GameManager.globalSpeedMultiplier;
    }

    public void Reset()
    {
        renderer.material.mainTextureOffset = Vector2.zero;
    }
}
