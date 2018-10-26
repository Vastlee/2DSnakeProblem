using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController> {
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;

    #region Properties
    #endregion

    #region Delegates & Events
    public delegate void UIClickHandler();
    public UIClickHandler OnClickGenerate;

    public delegate void UIGridSizeChangeHandler(int newSize);
    public UIGridSizeChangeHandler OnGridWidthChange;
    public UIGridSizeChangeHandler OnGridHeightChange;
    #endregion

    #region Unity
    #endregion

    #region Methods
    public void BroadcastClick_Generate() {
        this.OnClickGenerate?.Invoke();
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
