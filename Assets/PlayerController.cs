using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform rifleStart;
    [SerializeField] private Text HpText;

    [SerializeField] private GameObject GameOver;
    [SerializeField] private GameObject Victory;

    public float health = 0;

    void Start()
    {
        Destroy(this);
        ChangeHealth(0);
    }

    public void ChangeHealth(int hp)
    {
        health += hp;
        if (health > 100)
        {
            health = 100;
        }
        else if (health <= 0)
        {
            Lost();
        }
        HpText.text = health.ToString();
    }

    public void Win()
    {
        Victory.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }

    public void Lost()
    {
        GameOver.SetActive(true);
        Destroy(GetComponent<PlayerLook>());
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject buf = Instantiate(bullet);
            buf.transform.position = rifleStart.position;
            buf.GetComponent<Bullet>().setDirection(transform.forward);
            buf.transform.rotation = transform.rotation;
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Collider[] tar = Physics.OverlapSphere(transform.position, 2);
            foreach (var item in tar)
            {
                if (item.tag == "Enemy")
                {
                    Destroy(item.gameObject);
                }
            }
        }

        Collider[] targets = Physics.OverlapSphere(transform.position, 3);
        foreach (var item in targets)
        {
            if (item.tag == "Heal")
            {
                ChangeHealth(50);
                Destroy(item.gameObject);
            }
            if (item.tag == "Finish")
            {
                Win();
            }
            if (item.tag == "Enemy")
            {
                Lost();
            }
        }
    }
}
