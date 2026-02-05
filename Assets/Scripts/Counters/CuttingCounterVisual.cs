using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    [SerializeField] CuttingCounter cuttingCounter;

    private Animator anim;
    private const string CUT = "Cut";

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        anim.SetTrigger(CUT);
    }
}
