using UnityEngine.UI;

public class GridArrayData {
    private Text gridText;
    private Image gridImage;

    public Text GridText { get { return this.gridText; } }
    public Image GridImage { get { return this.gridImage; } }

    public GridArrayData(Text gridText, Image gridImage) {
        this.gridText = gridText;
        this.gridImage = gridImage;
    }
}
