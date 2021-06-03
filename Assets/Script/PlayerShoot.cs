using UnityEngine;
using Mirror;

public class PlayerShoot : NetworkBehaviour
{
    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;

    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("Pas de camera renseigner sur le systeme de tir.");
            this.enabled = false;   //Desactive le script PlayerShoot sur le joueur.
        }
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))    //detection du clique gauche.
        {
            Shoot();
        }
    }

    [Client]    //uniquement appeler par le client.
    private void Shoot()
    {
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            if (hit.collider.tag == "Player")
            {
                CmdPlayerShot(hit.collider.name, weapon.damage);
            }
        }
    }

    [Command]   //sur le client vers le serveur.
    private void CmdPlayerShot(string playerId, float damage)
    {
        Debug.Log(playerId + " a été touché.");

        Player player = GameManager.GetPlayer(playerId);    //on recup le scrit Player du joeuur toucher;
        player.RpcTakeDamage(damage);

    }

}
