using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Player player;

    private float chaseRange = 7f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int hp;
    [SerializeField] private int maxhp = 10;
    

    [SerializeField] public ParticleSystem deathParticles;
    [SerializeField] public GameObject collectible;

    private void Start()
    {
        hp = maxhp;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = FindObjectOfType<Player>();
        //deathParticles = (ParticleSystem)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Particles/Death Particles.prefab", typeof(ParticleSystem));
        //collectible = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Collectible.prefab", typeof(GameObject));
    }

    private void FixedUpdate()
    {
        anim.SetFloat("HORI_MOVE", rb.velocity.x);


        if (Vector3.Distance(transform.position, player.transform.position) <= chaseRange)
        {
            Chase();
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

    }

    private void Chase()
    {
        rb.velocity = (player.transform.position - transform.position).normalized * moveSpeed;
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        anim.SetTrigger("DMG");
        if(hp <= 0)
        {
            Instantiate(collectible, this.gameObject.transform.position,this.gameObject.transform.rotation, null);
            Destroy(gameObject);
            player.ScoreUp(1);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            TakeDamage(collision.gameObject.GetComponent<Dado>().valor);
        }
    }

}
