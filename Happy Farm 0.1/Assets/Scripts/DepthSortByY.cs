using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class DepthSortByY : MonoBehaviour
{

    private const int IsometricRangePerYUnit = 100;

    void Update()
    {
        Renderer renderer = this.GetComponent<SpriteRenderer>();
        renderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
    }
}
