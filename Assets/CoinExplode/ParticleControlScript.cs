using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleControlScript : MonoBehaviour
{

    [Header(" Manager ")]
    public Animator levelCompleteAnimator;

    [Header(" Settings  ")]
    public float particleSpeed;
    public float speedIncrement;
    public int coinsCount;
    float speed;
    bool fountainSoundPlayed;
    float timer;
    public float pTime;
    float t = 0;
    bool isLvlComplete;
    bool moreCoins;
    bool sliderLerped;
    bool rewardVideoCoins;
    public RectTransform reachPos;

    private void OnEnable()
    {
        PlayControlledParticles(new Vector2(Screen.width / 2, Screen.height / 2), reachPos);
    }

    public void PlayControlledParticles(Vector3 pos, RectTransform targetUI)
    {
        sliderLerped = false;

        speed = particleSpeed * Screen.width / 1080f;
        ParticleSystem ps = GetComponent<ParticleSystem>();

        transform.position = pos;
        StartCoroutine(PlayCoinParticlesCoroutine(ps, targetUI));
    }

    IEnumerator PlayCoinParticlesCoroutine(ParticleSystem ps, RectTransform targetUIElement)
    {
        //speed = particleSpeed;
        fountainSoundPlayed = false;

        Vector3[] distances = new Vector3[coinsCount];

        bool[] reached = new bool[coinsCount];

        if (isLvlComplete || moreCoins)
        {
            reached = new bool[coinsCount];
            distances = new Vector3[coinsCount];
        }

        ParticleSystem.EmissionModule em = ps.emission;
        em.SetBurst(0, new ParticleSystem.Burst(0, coinsCount));

        ps.Play();


        yield return new WaitForSeconds(1f);

        // Store the particles positions
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[ps.particleCount];
        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = particles[i].position;
        }


        while (ps.isPlaying)
        {
            particles = new ParticleSystem.Particle[ps.particleCount];

            ps.GetParticles(particles);

            for (int i = 0; i < particles.Length; i++)
            {
                Vector3 targetPos = Vector3.zero;

                targetPos.x = targetUIElement.position.x;
                targetPos.y = targetUIElement.position.y;
                targetPos.z = 0;


                Vector2 dir = targetPos - particles[i].position;
                t += Time.deltaTime / 2f;

                float smooth = Vector2.Distance(targetPos, distances[i]) / pTime;
               
                particles[i].position = Vector2.MoveTowards(particles[i].position, targetPos, smooth * Time.deltaTime);


                speed += speedIncrement;

                if (dir.magnitude < 0.05f)
                {
                    particles[i].color = new Color32(0, 0, 0, 0);

                    if (!reached[i])
                    {
                        reached[i] = true;
                    }


                }
            }

            ps.SetParticles(particles, particles.Length);

            timer += Time.deltaTime / 2f;

            if (timer > 0.5f)
            {
                ps.Stop();



                if (isLvlComplete)
                {
                    // Hide the gift panel
                    //levelCompleteAnimator.Play("HideGift");
                }

                yield return null;
            }

            yield return new WaitForSeconds(Time.deltaTime / 2);
        }




        if (isLvlComplete)
        {
            // Hide the gift panel
            //levelCompleteAnimator.Play("HideGift");
        }

        timer = 0;

        Debug.Log("Finished");




        yield return null;
    }
}
