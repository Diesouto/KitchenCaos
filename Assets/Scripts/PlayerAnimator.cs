using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    const string ISWALKING = "IsWalking";
    Animator anim;
    Player player;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        anim.SetBool(ISWALKING, player.IsWalking());
    }
}
