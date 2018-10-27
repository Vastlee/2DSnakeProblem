using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject tileRowPrefab;
    [SerializeField] private Transform rowsContainer;
    [SerializeField] private float creationSpeed = 0.0f;

    private GridArrayData[,] grid2DArray;
    private WaitForSeconds creationWait;
    private Transform currentRow;
    private int gridWidth = 0;
    private int gridHeight = 0;

    #region Unity Methods
    private void Awake() {
        this.creationWait = new WaitForSeconds(this.creationSpeed);
    }

    private void OnEnable() { SubscribeToUI(); }
    private void OnDisable() { UnsubscribeToUI(); }
    #endregion

    #region Methods
    private void StartSnakeGrid() {
        StartCoroutine(SnakeGrid());
    }

    private IEnumerator SnakeGrid() {
        int row, rowMin, rowMax, col, colMin, colMax;
        rowMin = colMin = 0;
        colMax = this.grid2DArray.GetLength(0);
        rowMax = this.grid2DArray.GetLength(1);

        int test = 0;
        while(test++ < 3/*rowMin <= rowMax && colMin <= colMax*/) {
            Debug.Log("Pass #" + test
                + " | cMin: " + colMin
                + " | cMax: " + colMax
                + " | rMin: " + rowMin
                + " | rMax: " + rowMax);

            for(int i = colMin; i < colMax; i++) {
                ChangeGridItem(i, rowMin);
                yield return this.creationWait;
            }
            rowMin++;

            for(int i = rowMin; i < rowMax; i++) {
                ChangeGridItem((colMax-1), i);
                yield return this.creationWait;
            }
            colMax--;

            for(int i = (colMax-1); i >= colMin; i--) {
                ChangeGridItem(i, (rowMax-1));
                yield return this.creationWait;
            }
            rowMax--;

            for(int i = (rowMax - 1); i >= rowMin; i--) {
                ChangeGridItem(colMin, i);
                yield return this.creationWait;
            }
            colMin++;
        }
    }

    private void ChangeGridItem(int col, int row) {
        GridArrayData data = this.grid2DArray[col, row];
        data.GridImage.color = (data.GridImage.color == Color.red) ? Color.white : Color.red;
        data.GridText.color = (data.GridText.color == Color.black) ? Color.white : Color.black;
    }

    private void StartCreateGrid() {
        StartCoroutine(CreateGrid(this.gridWidth, this.gridHeight));
    }

    private IEnumerator CreateGrid(int width, int height) {
        this.grid2DArray = new GridArrayData[width, height];

        ClearContainer();
        yield return new WaitUntil(() => { return (this.rowsContainer.childCount == 0); });

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.rowsContainer.gameObject.GetComponent<RectTransform>());
        for(int i = 0; i < height; i++) {
            CreateGridRow();
            for(int i2 = 0; i2 < width; i2++) {
                this.grid2DArray[i2, i] = CreateGridItem();
                this.grid2DArray[i2, i].GridText.text = i2.ToString() + "," + i.ToString();
                LayoutRebuilder.ForceRebuildLayoutImmediate(this.rowsContainer.gameObject.GetComponent<RectTransform>());
                yield return this.creationWait;
            }
        }
    }

    private void ClearContainer() {
        foreach(Transform row in this.rowsContainer) {
            Destroy(row.gameObject);
        }
    }

    private GridArrayData CreateGridItem() {
        Text txt;
        Image img;

        GameObject go = Instantiate(this.tilePrefab, this.currentRow.transform);
        go.SetActive(true);
        txt = go.GetComponentInChildren<Text>();
        img = go.GetComponent<Image>();

        return new GridArrayData(txt, img);
    }

    private void CreateGridRow() {
        GameObject go = Instantiate(this.tileRowPrefab, this.rowsContainer);
        go.SetActive(true);
        this.currentRow = go.transform;
    }

    private void UpdateWidth(int width) {
        this.gridWidth = width;
    }

    private void UpdateHeight(int height) {
        this.gridHeight = height;
    }

    private void SubscribeToUI() {
        UIController ui = FindObjectOfType<UIController>();
        UnsubscribeToUI();
        ui.OnClickGenerate += StartCreateGrid;
        ui.OnClickSnake += StartSnakeGrid;
        ui.OnGridWidthChange += UpdateWidth;
        ui.OnGridHeightChange += UpdateHeight;
    }

    private void UnsubscribeToUI() {
        UIController ui = FindObjectOfType<UIController>();
        ui.OnClickGenerate -= StartCreateGrid;
        ui.OnClickSnake -= StartSnakeGrid;
        ui.OnGridWidthChange -= UpdateWidth;
        ui.OnGridHeightChange -= UpdateHeight;
    }
    #endregion

}
