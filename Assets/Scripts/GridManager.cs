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
        StartCoroutine(SnakeGrid(this.grid2DArray));
    }

    private IEnumerator SnakeGrid(GridArrayData[,] data) {
        int row, rowMin, rowMax, col, colMin, colMax;
        rowMin = colMin = 0;
        colMax = data.GetLength(0);
        rowMax = data.GetLength(1);
        bool isComplete = false;
        WaitForSeconds pauseTime = new WaitForSeconds(0.0f);

        while(!isComplete) {
            isComplete = true;
            if(rowMax > rowMin) {
                for(int i = colMin; i < colMax; i++) {
                    ChangeGridItem(data[i, rowMin]);
                    yield return pauseTime;
                }
                rowMin++;
                isComplete = false;
            }

            if(colMax > colMin) {
                col = colMax - 1;
                for(int i = rowMin; i < rowMax; i++) {
                    ChangeGridItem(data[col, i]);
                    yield return pauseTime;
                }
                colMax--;
                isComplete = false;
            }

            if(rowMax > rowMin) {
                row = rowMax - 1;
                for(int i = (colMax - 1); i > (colMin - 1); i--) {
                    ChangeGridItem(data[i, row]);
                    yield return pauseTime;
                }
                rowMax--;
                isComplete = false;
            }

            if(colMax > colMin) {
                for(int i = (rowMax - 1); i > (rowMin - 1); i--) {
                    ChangeGridItem(data[colMin, i]);
                    yield return pauseTime;
                }
                colMin++;
                isComplete = false;
            }
        }
    }

    private void ChangeGridItem(GridArrayData data) {
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
        UnsubscribeToUI();
        UIController.Instance.OnClickGenerate += StartCreateGrid;
        UIController.Instance.OnClickSnake += StartSnakeGrid;
        UIController.Instance.OnGridWidthChange += UpdateWidth;
        UIController.Instance.OnGridHeightChange += UpdateHeight;
    }

    private void UnsubscribeToUI() {
        UIController.Instance.OnClickGenerate -= StartCreateGrid;
        UIController.Instance.OnClickSnake -= StartSnakeGrid;
        UIController.Instance.OnGridWidthChange -= UpdateWidth;
        UIController.Instance.OnGridHeightChange -= UpdateHeight;
    }
    #endregion

}
