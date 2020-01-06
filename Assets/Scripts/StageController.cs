using System;
using UnityEngine;
using Valve.VR;

public class StageController : MonoBehaviour
{
    public int difficulty;

    public Football football;
    public FootballPlayer footballPlayer;
    public GateHitBox gateHitBox;
    public GameController gameController;
    public TextAnimation textAnimation;
    public WeatherController weatherController;

    [Header("Gloves Collider")] 
    public BoxCollider glove1;
    public BoxCollider glove2;

    [Header("Logo stage components")]
    public GameObject logoSphere;
    public SteamVR_Fade fade;

    [Header("Football Player Move Points")]
    public Transform movePointBall;
    public Transform movePointReadyPoint;

    private Action m_Action;

    private bool m_CanStart;
    private float m_Timer;
    private float m_SecondTimer;
    private float m_ThirdTimer;
    private const float WAIT_BEFORE_SHOOT_TIME = 2f;
    private const float FOOTBALL_PLAYER_MOVE_INCREMENT = 0.5f;
    private const float FOOTBALL_FORCE_INCREMENT = 3.50f;
    private const float DEFENDING_TIME_DURATION = 4f;
    private const float ANIMATION_SPEED_INCREMENT = 0.10f;
    private const float LOGO_WAIT_TIME = 2.25f;

    private void Start()
    {
        m_Action += StageLogo;

        // Prepare timer
        m_Timer = LOGO_WAIT_TIME;

        m_CanStart = true;
    }
    private void Update()
    {
        m_Action?.Invoke();
    }

    public void InitStage(int stage)
    {
        m_Timer = 0;
        m_Action += StagePreparation;

        // Set glove collider
        glove1.enabled = stage != 4;
        glove2.enabled = stage != 4;

        // Set get ready text
        textAnimation.SetGetReadyText();

        // Difficulty is based on what stage the player is playing on
        difficulty = stage;

        // Set agent properties
        footballPlayer.agent.speed = 2.5f;
        footballPlayer.agent.SetDestination(movePointReadyPoint.position);

        // Set animator properties
        footballPlayer.animator.speed = 1;
        footballPlayer.animator.SetBool("movingBack", true);
    }
    private void StagePreparation()
    {
        if (FootballPlayerCloseToPosition(movePointReadyPoint.position))
        {
            // Stop moving back animation
            footballPlayer.animator.SetBool("movingBack", false);

            // Wait before shooting the ball
            m_Timer += Time.deltaTime;
            if (m_Timer >= WAIT_BEFORE_SHOOT_TIME)
            {
                // Set nav mesh agent properties
                footballPlayer.agent.speed = 3 + difficulty * FOOTBALL_PLAYER_MOVE_INCREMENT;
                footballPlayer.agent.SetDestination(movePointBall.position);

                // Set animator properties
                footballPlayer.animator.speed = 1 + difficulty * ANIMATION_SPEED_INCREMENT;
                footballPlayer.animator.SetBool("shooting", true);

                // Ready Next Stage
                m_Timer = 0;
                m_Action += StageShooting;
                m_Action -= StagePreparation;
            }
        }
        else
        {
            footballPlayer.agent.Move(Vector3.zero);
        }
    }
    private void StageShooting()
    {
        if (FootballPlayerCloseToPosition(movePointBall.position))
        {
            footballPlayer.animator.SetBool("canShoot", true);
            footballPlayer.animator.SetBool("shooting", false);
            
            // Ball kick delay
            m_Timer += Time.deltaTime;
            if (m_Timer >= 0.35f)
            {
                // Play kick sounds
                AudioManager.Instance.PlaySound(Sounds.kick);

                // Calculate ball direction
                var direction = (gateHitBox.GetRandomPosition(difficulty) - football.transform.position).normalized;

                // Kick ball
                var force = 7.5f + difficulty * FOOTBALL_FORCE_INCREMENT;
                football.rigidbody.AddForce(direction * force, ForceMode.Impulse);
                football.SetGravity(true);

                // Enable trail renderer
                football.trailRenderer.enabled = true;

                // Ready next stage
                m_Timer = 0;
                m_Action += StageDefending;
                m_Action -= StageShooting;
            }
        }
        else
        {
            footballPlayer.agent.Move(Vector3.zero);
        }
    }
    private void StageDefending()
    {
        // LOST - ball inside gate
        if(football.isInsideGate)
            StageEnd(false);

        // WON - lasted x time without ball entering the gate
        m_Timer += Time.deltaTime;
        if (m_Timer >= DEFENDING_TIME_DURATION)
            StageEnd(true);
    }
    private void StageEnd(bool won)
    {
        // Play result sounds
        AudioManager.Instance.PlaySound(won ? Sounds.won : Sounds.lost);

        // Resetting canShoot parameter
        footballPlayer.animator.SetBool("canShoot", false);

        // Show win / lost text
        textAnimation.SetRoundOutcomeText(won);

        // Change current stage depending on round outcome
        gameController.currentStage = won ? gameController.currentStage + 1 : 0; 

        // Reset ball position
        football.ResetPosition();

        // Ready next stage
        m_Action -= StageDefending;

        if (won)
        {
            gameController.stage = GameController.Stage.Idle;
        }
        else
        {
            InitLogoStage();
        }
    }
    private void StageLogo()
    {
        if (gameController.PlayerIsReady())
        {
            m_CanStart = true;
            textAnimation.animator.speed = 1;
            gameController.SetControllerHints(false);
        }
        if (!m_CanStart)
        {
            if (textAnimation.animator.GetCurrentAnimatorStateInfo(0).IsName("PassingText")
                    && textAnimation.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
                textAnimation.animator.speed = 0;

            gameController.SetControllerHints(true);
            return;
        }

        m_Timer += Time.deltaTime;
        if (m_Timer >= LOGO_WAIT_TIME && m_SecondTimer == 0)
        {
            // Activate fade animation
            fade.OnStartFade(Color.white, 3, true);
        }

        if (m_Timer >= LOGO_WAIT_TIME)
            m_SecondTimer += Time.deltaTime;
        if (m_SecondTimer >= LOGO_WAIT_TIME && m_ThirdTimer == 0)
        {
            // Change field weather
            weatherController.SetRandomWeather();

            // Activate logo sphere
            logoSphere.gameObject.SetActive(true);

            // Fade out
            fade.OnStartFade(Color.clear, 1, true);
        }

        if (m_SecondTimer >= LOGO_WAIT_TIME)
            m_ThirdTimer += Time.deltaTime;
        if (m_ThirdTimer >= LOGO_WAIT_TIME)
        {
            m_Timer = 0;
            m_SecondTimer = 0;
            m_ThirdTimer = 0;

            m_Action -= StageLogo;

            // Start idle stage
            gameController.stage = GameController.Stage.Idle;

            // Activate fade animation
            fade.OnStartFade(Color.white, 0, true);
            fade.OnStartFade(Color.clear, 2, true);

            // Deactivate logo sphere
            logoSphere.gameObject.SetActive(false);
        }
    }

    private void InitLogoStage()
    {
        m_Action += StageLogo;

        // Prepare timer
        m_Timer = 0;

        m_CanStart = false;
    }
    private bool FootballPlayerCloseToPosition(Vector3 position)
    {
        return Vector3.Distance(footballPlayer.transform.position, position) < 1f;
    }
}