using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleOnFinish : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        // Si el sistema de partículas ha terminado y no tiene más partículas, destruir el objeto
        if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
