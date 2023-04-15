using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseVisual : MonoBehaviour
{
    private bool bShow = false;

    private SpriteRenderer _spriteRenderer;
    private TrailRenderer _trailRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _spriteRenderer.enabled = false;
        _trailRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        if (Touchscreen.current.press.isPressed)
        {
            Vector3 ScreenPosition = new Vector3(Touchscreen.current.position.ReadValue().x, Touchscreen.current.position.ReadValue().y, 0f);
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(ScreenPosition);
            newPosition.z = 0f;
            transform.position = newPosition;

            _spriteRenderer.enabled = true;
            _trailRenderer.enabled = true;
        }
        else if (_spriteRenderer.enabled || _trailRenderer.enabled)
        {
            _spriteRenderer.enabled = false;
            _trailRenderer.enabled = false;
        }
        
    }
}
