using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SparkEffectController : MonoBehaviour
{
    private List<ParticleSystem> m_Systems;

    private void Awake()
    {
        m_Systems = transform.GetComponentsInChildren<ParticleSystem>().ToList();
    }

    public void PlayEffects()
    {
        foreach (var s in m_Systems)
        {
            s.Play();
        }
    }
}
