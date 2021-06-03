using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const string playerIdPrefix = "Player";

    private static Dictionary<string,Player> players = new Dictionary<string, Player>();

    public MatchSettings matchSettings;

    public static GameManager insatnce;

    private void Awake()
    {
        if (insatnce == null)
        {
            insatnce = this;
            return;
        }
        Debug.LogError("plus dune instance de gamemanager dans la scene");
    }


    public static void RegisterPlayer(string netID, Player player)  //static pour y avoir acces dans PlayerSetup.
    {
        string playerId = playerIdPrefix + netID;
        players.Add(playerId, player);
        player.transform.name = playerId;
    }

    public static void UnRegisterPlayer(string playerId)    //il cherhce la clef dans le dico et supprime l'elemment du dico.
    {
        players.Remove(playerId);
    }

    public static Player GetPlayer(string playerId)
    {
        return players[playerId];
    }
}
