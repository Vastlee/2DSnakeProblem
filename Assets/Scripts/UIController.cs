using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoSingleton<UIController> {
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
    #endregion

    #region Methods
    public void BroadcastClickGenerate() {
        OnClickGenerate?.Invoke();
    }

    public void BroadcastClickSnake() {
        OnClickSnake?.Invoke();
    }

    public void BroadcastWidthChange(string newWidth) {
        int width;
        if(System.Int32.TryParse(newWidth, out width)) {
            OnGridWidthChange?.Invoke(width);
        }
    }

    public void BroadcastHeightChange(string newHeight) {
        int height;
        if(System.Int32.TryParse(newHeight, out height)) {
            OnGridHeightChange?.Invoke(height);
        }
    }
    #endregion
}
