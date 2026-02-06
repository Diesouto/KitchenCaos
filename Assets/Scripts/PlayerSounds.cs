using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    Player player;
    float footstepTimer;
    float footstepTimerMax = .1f;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;

        if(footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepsSound(player.transform.position);
            }
        }
    }
}
