using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Clock clock;
    public Button startButton;
    public Button restartButton;
    public Button closeButton;
    public GameObject mask;
    public ResultForm resultForm;
    public KeyRecorder keyRecorder;
    public NumberManager numberManager;

    private readonly ClickRecorder mClickRecorder = new();

    private void Awake()
    {
        instance = this;

        closeButton.onClick.AddListener(() => Application.Quit());

        startButton.onClick.AddListener(() =>
        {
            Restart();

            startButton.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(true);
            mask.SetActive(false);
        });

        restartButton.onClick.AddListener(Restart);

        mask.SetActive(true);
        startButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(false);
    }

    private void Restart()
    {
        keyRecorder.ResetKey();
        mClickRecorder.Reset();
        clock.ResetTime();

        numberManager.Layout();
    }

    private void Start()
    {
        NumberManager.instance.OnNext += id =>
        {
            mClickRecorder.Add(id);
        };

        NumberManager.instance.OnWin += () =>
        {
            clock.Stop();
            keyRecorder.Stop();

            resultForm.Open(clock.timeText.text, mClickRecorder, keyRecorder);
        };
    }
}