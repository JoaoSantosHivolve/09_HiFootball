using UnityEngine;

public class StadiumTribuneAnimation : MonoBehaviour
{
    [Header("Textures")]
    public Texture tribune1Frame1;
    public Texture tribune1Frame2;
    public Texture tribune2Frame1;
    public Texture tribune2Frame2; 
    public Texture tribune3Frame1;
    public Texture tribune3Frame2;

    [Header("Materials")]
    public Material tribune1;
    public Material tribune2;
    public Material tribune3;

    [Header("Animation Timers")]
    public float tribune1Threshold = 1.00f;
    public float tribune2Threshold = 1.25f;
    public float tribune3Threshold = 1.50f;

    private float m_Tribune1Timer;
    private float m_Tribune2Timer;
    private float m_Tribune3Timer;

    private void Update()
    {
        m_Tribune1Timer += Time.deltaTime;
        m_Tribune2Timer += Time.deltaTime;
        m_Tribune3Timer += Time.deltaTime;

        if (m_Tribune1Timer >= tribune1Threshold)
        {
            m_Tribune1Timer = 0;
            tribune1.mainTexture = tribune1.mainTexture == tribune1Frame1 ? tribune1Frame2 : tribune1Frame1;
        }
        if (m_Tribune2Timer >= tribune2Threshold)
        {
            m_Tribune2Timer = 0;
            tribune2.mainTexture = tribune2.mainTexture == tribune2Frame1 ? tribune2Frame2 : tribune2Frame1;
        }
        if (m_Tribune3Timer >= tribune3Threshold)
        {
            m_Tribune3Timer = 0;
            tribune3.mainTexture = tribune3.mainTexture == tribune3Frame1 ? tribune3Frame2 : tribune3Frame1;
        }
    }
}
