using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnPlaceholder : MonoBehaviour
{
    public ParticleSystem circle;
    public ParticleSystem pulse;
    public ParticleSystem particles;
    // Start is called before the first frame update

    private void Start()
    {
        circle.Play();
        circle.gameObject.SetActive(true);

        pulse.Stop();
        particles.Stop();
        pulse.gameObject.SetActive(false);
        particles.gameObject.SetActive(false);



    }

    public void OnHolderPlaced()
    {
        circle.Play();
        pulse.Play();
        particles.Play();

        circle.gameObject.SetActive(true);
        pulse.gameObject.SetActive(true);
        particles.gameObject.SetActive(true);

    }
}
