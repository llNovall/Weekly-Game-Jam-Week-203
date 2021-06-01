using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _minY, _maxY;
    private void Update()
    {
        if (gameObject.transform.rotation.eulerAngles.y > _minY && gameObject.transform.rotation.eulerAngles.y < _maxY)
            _spriteRenderer.flipY = false;
        else
            _spriteRenderer.flipY = true;
    }
}
