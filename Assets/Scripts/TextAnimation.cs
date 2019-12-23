using TMPro;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public TextMeshProUGUI phrase;
    public SparkEffectController effectController;
    public GameController gameController;

    [HideInInspector] public Animator animator;

    private static readonly int Activate = Animator.StringToHash("Activate");

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetGetReadyText()
    {
        phrase.text = "Prepara-te!";
        PlayAnimation();
    }
    public void SetRoundOutcomeText(bool won)
    {
        phrase.text = won ? "Boa Defesa!" : "Obrigado por participar \n na experiência SABSEG \n\n Você defendeu '" + gameController.currentStage + "' penaltis";
        if (won)
        {
            effectController.PlayEffects();
        }

        PlayAnimation();
    }

    private void PlayAnimation()
    {
        animator.SetTrigger(Activate);
    }
}
