using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDestroy : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        DestroyObject(other.gameObject);
    }
}
