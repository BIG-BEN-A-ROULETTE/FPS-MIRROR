using UnityEngine;
using Mirror;
using System.Collections;

public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead  //on creer un accesseur.
    {
        get { return _isDead; }
        protected set { _isDead = value; }   //seul cette classe peut modifier la valeur de set
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]   //pour que tout les instances soit au courent de la vie des autres instances.
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabledOnStrat;

    public void Setup()
    {
        wasEnabledOnStrat = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStrat[i] = disableOnDeath[i].enabled;
        }

        SetDefaults();
    }

    public void SetDefaults()   //pour la reinitialisation du joueur.
    {
        isDead = false;
        currentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStrat[i];
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.insatnce.matchSettings.respawnTimer);
        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }

    private void Update()   //methode pour simplifier les testes (sinfliger des degats a soit meme).
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(999);
        }
    }

    [ClientRpc] //depuis le serveur vers le client.
    public void RpcTakeDamage(float amount)
    {

        if(isDead)
        {
            return;
        }

        currentHealth -= amount;

        Debug.Log(transform.name + "a mtn : " + currentHealth + "point de vie.");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log(transform.name + " a ete elimine.");

        StartCoroutine(Respawn());
    }

}
