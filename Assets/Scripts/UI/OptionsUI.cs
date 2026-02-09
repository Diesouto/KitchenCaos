using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }

    [SerializeField] Button sfxButton;
    [SerializeField] Button musicButton;
    [SerializeField] Button closeButton;
    [SerializeField] TextMeshProUGUI sfxText;
    [SerializeField] TextMeshProUGUI musicText;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one OptionsUI! " + transform + " - " + Instance);
        }
        Instance = this;

        sfxButton.onClick.AddListener(() => {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        musicButton.onClick.AddListener(() => {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() => Hide());
    }

    void Start()
    {
        UpdateVisual();
        Hide();
    }

    void UpdateVisual()
    {
        sfxText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10f);
        musicText.text = "Sound Effects: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10f);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
