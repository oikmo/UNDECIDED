using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISway : MonoBehaviour
{
    private RectTransform rtransform;
    public float sens = -75f;
    public float idk = .1f;

    void Start() 
    {
        rtransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if(GameHandler.Instance != null)
        {
            if (!GameHandler.Instance.paused)
            {
                if (!GameHandler.Instance.mouse)
                {
                    float x_axis = GameHandler.Instance.playerInput.Player.Look.ReadValue<Vector2>().x * sens;
                    float y_axis = GameHandler.Instance.playerInput.Player.Look.ReadValue<Vector2>().y * sens;

                    float x_lerp = Mathf.Lerp(rtransform.anchoredPosition.x, x_axis, idk);
                    float y_lerp = Mathf.Lerp(rtransform.anchoredPosition.y, y_axis, idk);

                    rtransform.anchoredPosition = new Vector3(x_lerp, y_lerp);
                }

            }
        }
        
    }
}