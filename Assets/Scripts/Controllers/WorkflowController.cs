using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: Implement State Machine
public class WorkflowController : MonoBehaviour
{
    [SerializeField]
    private SelectProductView _selectProductView;

    [SerializeField]
    private ProductsCollection _productsCollection;

    [SerializeField]
    private RulersController _rulersController;

    [SerializeField]
    private CameraController _cameraControl;

    [SerializeField]
    private RoomController _room;

    [SerializeField]
    private PlacementController _placementController;

    [SerializeField]
    private SelectWallController _selectWallController;

    [SerializeField]
    private RoomEditorController _roomEditor;

    [SerializeField]
    private Button _editRoomButton;

    private bool _isSelectingWall;

    private void OnEnable()
    {
        _selectProductView.OnProductSelected += OnProductSelectedHandler;
        _placementController.OnProductPlacementDone += OnProductPlacementDoneHandler;
        _selectWallController.OnSelectWall += OnSelectWallHandler;
        _cameraControl.OnStartRotation += OnStartCameraRotationHandler;
        _cameraControl.OnEndRotation += OnEndCameraRotationHandler;
        _roomEditor.OnDone += OnRoomEditingDoneHandler;
        _roomEditor.OnCancel += OnRoomEditingCancelHandler;
        _editRoomButton.onClick.AddListener(OnRoomEditorButtonPressedHandler);
    }

    private void OnDisable()
    {
        _selectProductView.OnProductSelected -= OnProductSelectedHandler;
        _placementController.OnProductPlacementDone -= OnProductPlacementDoneHandler;
        _selectWallController.OnSelectWall -= OnSelectWallHandler;
        _cameraControl.OnStartRotation -= OnStartCameraRotationHandler;
        _cameraControl.OnEndRotation -= OnEndCameraRotationHandler;
        _roomEditor.OnDone -= OnRoomEditingDoneHandler;
        _roomEditor.OnCancel -= OnRoomEditingCancelHandler;
        _editRoomButton.onClick.RemoveAllListeners();
    }

    private void Start()
    {
        _selectProductView.Initialize(_productsCollection);
        ShowProductSelectionWindow();
    }

    private void ShowProductSelectionWindow()
    {
        _selectProductView.Show();
        _cameraControl.gameObject.SetActive(true);
    }

    private void OnEndCameraRotationHandler()
    {
        _selectWallController.enabled = _isSelectingWall;
    }

    private void OnStartCameraRotationHandler()
    {
        _selectWallController.enabled = false;
    }

    private void OnProductSelectedHandler(Product selectedProduct)
    {
        _placementController.SetSelectedProduct(selectedProduct);

        _selectProductView.Hide();
        _isSelectingWall = true;
        _selectWallController.enabled = true;
    }

    private void OnSelectWallHandler(Wall wall)
    {
        if (_cameraControl.IsDrag == false)
        {
            _cameraControl.gameObject.SetActive(false);
            var productInstance = _placementController.PlaceProduct(wall.CornerBegin, wall.CornerEnd);
            _rulersController.CreateRuler(productInstance, wall.CornerBegin);
            _rulersController.CreateRuler(productInstance, wall.CornerEnd);
            _isSelectingWall = false;
            _selectWallController.enabled = false;
        }
    }

    private void OnProductPlacementDoneHandler(Product product)
    {
        _rulersController.RemoveRulers(product);
        _cameraControl.gameObject.SetActive(true);
        ShowProductSelectionWindow();
    }

    private void OnRoomEditingDoneHandler(List<Vector3> planPoints)
    {
        _roomEditor.Clear();
        _roomEditor.gameObject.SetActive(false);
        _room.Build(planPoints);
        ShowProductSelectionWindow();
    }

    private void OnRoomEditingCancelHandler()
    {
        _roomEditor.Clear();
        _roomEditor.gameObject.SetActive(false);
        ShowProductSelectionWindow();
    }

    private void OnRoomEditorButtonPressedHandler()
    {
        _rulersController.RemoveAllRulers();
        _placementController.RemoveAllProducts();
        _placementController.DiscardCurrentPlacement();
        _cameraControl.gameObject.SetActive(false);
        _selectProductView.Hide();
        _roomEditor.Show();
    }
}
