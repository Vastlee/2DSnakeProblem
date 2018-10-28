using UnityEngine;
using UnityEngine.UI;

public class InputInit : MonoBehaviour {
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;
    [SerializeField] private Button generateButton;
    [SerializeField] private Button snakeButton;

    private void Awake() {
        UIController ui = UIController.Instance;
        this.widthInput.onValueChanged.AddListener((s) => { ui.BroadcastWidthChange(this.widthInput.text); });
        this.heightInput.onValueChanged.AddListener((s) => { ui.BroadcastHeightChange(this.heightInput.text); });
        this.generateButton.onClick.AddListener(ui.BroadcastClickGenerate);
        this.snakeButton.onClick.AddListener(ui.BroadcastClickSnake);
    }

    private void Start() {
        CheckForInput();
    }

    private void CheckForInput() {
        UIController ui = UIController.Instance;
        if(!System.String.IsNullOrEmpty(this.widthInput.text)) {
            ui.BroadcastWidthChange(this.widthInput.text);
        }
        if(!System.String.IsNullOrEmpty(this.heightInput.text)) {
           ui.BroadcastHeightChange(this.heightInput.text);
        }
    }
};
