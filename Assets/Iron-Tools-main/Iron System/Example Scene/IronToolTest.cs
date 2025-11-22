using UnityEngine;
using UnityEngine.UI;
using IronTools.Attributes;

public class IronToolTest : MonoBehaviour
{
    [ShowTitle("Player Speed", EditorColor.Orange)]
    [SerializeField] private float playerSpeed;

    [Space(20)]
    [Section(EditorColor.Blue)]
    public bool CanAttack;

    [ShowIf("CanAttack")]
    [ShowTitle("Player Power")]
    public int AttackPower;
    [ShowIf("CanAttack")]
    public float AttackRange;
    [ShowIf("CanAttack")]
    public string AttackAnimName;

    [ShowDivider(EditorColor.Yellow)]
    [SerializeField] private int playerJump;
    [SerializeField] private bool isGround;

    [ShowButton(label:"",iconPath:"MyIcon",onlyIcon:true)]
    public void SpawnPlayer(GameObject player,Vector3 playerSpawnPos)
    {
        Instantiate(player, playerSpawnPos, Quaternion.identity);
    }
}
