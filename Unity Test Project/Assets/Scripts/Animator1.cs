using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator1 : MonoBehaviour
{
    Transform m_B1;
    Transform m_B2;
    float m_R = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_B1 = transform.GetChild(0);
        m_B2 = m_B1.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        m_R += Time.deltaTime;
        m_B1.rotation = Quaternion.Euler(0, 0, 90 + 10 * Mathf.Cos(m_R));
        m_B2.rotation = Quaternion.Euler(0, 0, 90 + 20 * Mathf.Cos(m_R));
    }
}
