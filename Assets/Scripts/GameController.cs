using UnityEngine;
using Valve.VR;

public class GameController : MonoBehaviour
{
    public enum Stage
    {
        Idle,
        Defending
    }
    public Stage stage;

    public int currentStage;
    public StageController stageController;
    public ControllerButtonIconBehaviour controllerSprite1;
    public ControllerButtonIconBehaviour controllerSprite2;
    

    private float m_Timer;
    private const float WIN_WAIT_TIME = 4f;
    private const float LOGO_WAIT_TIME = 2f;

    private void Awake()
    {
        stage = Stage.Defending;
    }
    private void Update()
    {
        if (stage == Stage.Idle)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= (currentStage > 0 ? WIN_WAIT_TIME : LOGO_WAIT_TIME))
            {
                SetControllerHints(true);

                if (PlayerIsReady())
                {
                    m_Timer = 0;
                    stage = Stage.Defending;
                    stageController.InitStage(currentStage);

                    SetControllerHints(false);
                }
            }
        }
    }

    public bool PlayerIsReady()
    {
        return SteamVR_Actions._default.Squeeze.GetAxis(SteamVR_Input_Sources.LeftHand) >= 0.8f
               && SteamVR_Actions._default.Squeeze.GetAxis(SteamVR_Input_Sources.RightHand) >= 0.8f;
    }

    public void SetControllerHints(bool state)
    {
        controllerSprite1.SetController(state);
        controllerSprite2.SetController(state);
    }
}