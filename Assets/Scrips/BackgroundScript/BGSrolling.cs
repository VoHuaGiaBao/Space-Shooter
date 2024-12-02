using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSrolling : MonoBehaviour
{
    public float srollSpeed;

    private Material mat;

    private Vector2 offset = Vector2.zero;

    void Awake()
    {
        mat = GetComponent<MeshRenderer > ().material;
    }
    // Start is called before the first frame update
    void Start()
    {
        offset = mat.GetTextureOffset("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        offset.y += srollSpeed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", offset);
    }
}
