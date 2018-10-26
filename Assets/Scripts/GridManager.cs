using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour {
    [SerializeField]
    private GameObject tilePrefab;
    [SerializeField]
    private GameObject tileRowPrefab;
    [SerializeField]
    private Transform rowsContainer;
    [SerializeField]
    private float creationSpeed = 0.0f;

    private Image[,] grid2DArray;
    private WaitForSeconds creationWait;
    private Transform currentRow;
    private int gridWidth = 0;
    private int gridHeight = 0;

    #region Unity Methods
    private void Awake() {
        this.creationWait = new WaitForSeconds(this.creationSpeed);
    }
    private void Start() {
    }

    private void OnEnable() { SubscribeToUI(); }
    private void OnDisable() { UnsubscribeToUI(); }
    #endregion

    #region Methods
    private void CreateGrid() {
        StartCoroutine(CreateGrid(this.gridWidth, this.gridHeight));
    }

    private IEnumerator CreateGrid(int width, int height) {
        this.grid2DArray = new Image[width, height];

        GameObject objectCheck;
        while(this.rowsContainer.childCount > 0) {
            objectCheck = this.rowsContainer.GetChild(0).gameObject;
            Destroy(objectCheck);
            yield return new WaitUntil(() => objectCheck == null);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.rowsContainer.gameObject.GetComponent<RectTransform>());
        for(int i = 0; i < height; i++) {
            CreateGridRow();
            for(int i2 = 0; i2 < width; i2++) {
                this.grid2DArray[i2, i] = CreateGridItem().GetComponent<Image>();
                LayoutRebuilder.ForceRebuildLayoutImmediate(this.rowsContainer.gameObject.GetComponent<RectTransform>());
                yield return this.creationWait;
            }
        }
    }

    private GameObject CreateGridItem() {
        GameObject go = Instantiate(this.tilePrefab, this.currentRow.transform);
        go.SetActive(true);
        return go;
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
        ui.OnClickGenerate -= CreateGrid;
        ui.OnClickGenerate += CreateGrid;
        ui.OnGridWidthChange -= UpdateWidth;
        ui.OnGridWidthChange += UpdateWidth;
        ui.OnGridHeightChange -= UpdateHeight;
        ui.OnGridHeightChange += UpdateHeight;
    }

    private void UnsubscribeToUI() {
        UIController ui = FindObjectOfType<UIController>();
        ui.OnClickGenerate -= CreateGrid;
        ui.OnGridWidthChange -= UpdateWidth;
    }
    #endregion

}
