using UnityEngine;
using Valve.VR;

public class ControllerButtonIconBehaviour : MonoBehaviour
{
    public enum ControllerSide
    {
        Right,
        Left
    }
    public ControllerSide controller;
    public new SpriteRenderer renderer;
    public GameController gameController;
    public bool showHint;

    [Header("Animation Sprites")] 
    public Sprite idle;
    public Sprite pressThis;
    public Sprite pressed;

    private float m_Timer;
    private const float ANIMATION_TIME = 0.25f;

    private void Update()
    {
        if (!showHint)
            return;

        if (SteamVR_Actions._default.Squeeze.GetAxis(controller == ControllerSide.Left
                ? SteamVR_Input_Sources.LeftHand
                : SteamVR_Input_Sources.RightHand) >= 0.8f)
        {
            // Green image
            renderer.sprite = pressed;
        }
        else
        {
            // Press this button animation
            m_Timer += Time.deltaTime;
            if (m_Timer >= ANIMATION_TIME)
            {
                m_Timer = 0;
                renderer.sprite = renderer.sprite == idle ? pressThis : idle;
            }
        }

    }

    public void SetController(bool state)
    {
        showHint = state;
        renderer.enabled = state;
    }
}
