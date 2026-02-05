using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField] ContainerCounter containerCounter;

    private Animator anim;
    private const string OPEN_CLOSE = "OpenClose";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
    }

    private void ContainerCounter_OnPlayerGrabbedObject(object sender, System.EventArgs e)
    {
        anim.SetTrigger(OPEN_CLOSE);
    }
}
