using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] StoveCounter stoveCounter;
    [SerializeField] GameObject stoveOnGameObject;
    [SerializeField] GameObject particlesGameObject;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    void Show()
    {
        stoveOnGameObject.SetActive(true);
        particlesGameObject.SetActive(true);
    }

    void Hide()
    {
        stoveOnGameObject.SetActive(false);
        particlesGameObject.SetActive(false);
    }
}
