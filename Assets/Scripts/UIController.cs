using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController> {
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;

    #region Properties
    #endregion

    #region Delegates & Events
    public delegate void UIClickHandler();
    public event UIClickHandler OnClickGenerate;
    public event UIClickHandler OnClickSnake;

    public delegate void UIGridSizeChangeHandler(int newSize);
    public event UIGridSizeChangeHandler OnGridWidthChange;
    public event UIGridSizeChangeHandler OnGridHeightChange;
    #endregion

    #region Unity
    private void Start() {
        CheckForInput();
    }
    #endregion

    #region Methods
    private void CheckForInput() {
        if(!string.IsNullOrEmpty(widthInput.text)) {
            BroadcastWidthChange();
        }
        if(!string.IsNullOrEmpty(heightInput.text)) {
            BroadcastHeightChange();
        }
    }

    public void BroadcastClickGenerate() {
        this.OnClickGenerate?.Invoke();
    }

    public void BroadcastClickSnake() {
        this.OnClickSnake?.Invoke();
    }

    public void BroadcastWidthChange() {
        int newWidth;
        if(int.TryParse(widthInput.text, out newWidth)) {
            OnGridWidthChange?.Invoke(newWidth);
        }
    }

    public void BroadcastHeightChange() {
        int newHeight;
        if(int.TryParse(heightInput.text, out newHeight)) {
            OnGridHeightChange?.Invoke(newHeight);
        }
    }
    #endregion
}
