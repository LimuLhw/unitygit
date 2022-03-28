using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    private ParticleSystem ps;

    void Start()
    {
        if (GameObject.Find("CircleParticle(Clone)"))
            ps = GameObject.Find("CircleParticle(Clone)").GetComponent<ParticleSystem>();
        else if (GameObject.Find("StarParticle(Clone)"))
            ps = GameObject.Find("StarParticle(Clone)").GetComponent<ParticleSystem>();
        else if (GameObject.Find("DiaParticle(Clone)"))
            ps = GameObject.Find("DiaParticle(Clone)").GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (ps)
        {
            if (!ps.IsAlive()) // 파티클 이펙트 종료
                StartCoroutine(StopParticle());
        }
    }

    IEnumerator StopParticle()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}
