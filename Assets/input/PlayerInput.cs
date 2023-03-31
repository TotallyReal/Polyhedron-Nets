// GENERATED AUTOMATICALLY FROM 'Assets/input/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""MouseSelection"",
            ""id"": ""c9e66897-7f26-467f-ae09-4b30d9336a55"",
            ""actions"": [
                {
                    ""name"": ""EdgeSelect"",
                    ""type"": ""Button"",
                    ""id"": ""db3153fa-ffa5-4cdc-b492-8c23f8928940"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""85fc4ac0-846a-46cd-b146-5ecbd87103b6"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EdgeSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""7b47fe08-a32e-4380-a990-29bb4859d33d"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""778ce677-dd5d-45b4-9465-4d2a0d67bd66"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""56dadcdf-f439-4f1f-b136-7b4f2ab68ace"",
                    ""expectedControlType"": ""Double"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""mouse zoom"",
                    ""type"": ""Button"",
                    ""id"": ""2335ba7e-0c84-4725-a106-633f43822689"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Auto Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""e8be567f-4f8f-4215-804d-d2b05e46bd76"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""2387f042-73ab-4a29-9f33-e1b8ac1fca34"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b9eed8bb-5d06-4d97-a135-946fd0aff379"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""09f011ca-b0af-411a-8d57-fa8e546ee813"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a7cf4c78-451d-4352-88b4-172d32b5c427"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dbe6af00-1e78-40db-892a-275b7f36b9b1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""27a8c46b-72b6-4deb-b682-28262a0f2e4b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9e320b65-e809-475b-86b8-8aa54d1c04f4"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5525e743-234a-487f-b103-73bdeb6090e8"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ded44f39-ffab-4f71-a570-7a6eb9f11e4b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cd32cb09-15c5-4ceb-8aa9-d4feb636b8b0"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Plus\\Minus"",
                    ""id"": ""b8df04cd-e514-4a16-9ee5-9882e82ea05e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""afeb1070-20d7-4094-b7f3-376053cec1c8"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""de32c598-0cbf-45ae-99f0-4ddc110d0a0d"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""b206e7d6-2887-4eec-af38-f4cc5e49c3d6"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""mouse zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f905a1b-1304-4ab9-b703-d0cedc57472b"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Auto Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player"",
            ""id"": ""8143e19c-2b3c-472f-a2d2-11bd02c4eb29"",
            ""actions"": [
                {
                    ""name"": ""Graph"",
                    ""type"": ""Button"",
                    ""id"": ""346facb1-4348-43a8-aeb6-1518851da72e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""51f53477-5246-4bba-9d19-aa0db49287b1"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Graph"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MouseSelection
        m_MouseSelection = asset.FindActionMap("MouseSelection", throwIfNotFound: true);
        m_MouseSelection_EdgeSelect = m_MouseSelection.FindAction("EdgeSelect", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Movement = m_Camera.FindAction("Movement", throwIfNotFound: true);
        m_Camera_Zoom = m_Camera.FindAction("Zoom", throwIfNotFound: true);
        m_Camera_mousezoom = m_Camera.FindAction("mouse zoom", throwIfNotFound: true);
        m_Camera_AutoRotate = m_Camera.FindAction("Auto Rotate", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Graph = m_Player.FindAction("Graph", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // MouseSelection
    private readonly InputActionMap m_MouseSelection;
    private IMouseSelectionActions m_MouseSelectionActionsCallbackInterface;
    private readonly InputAction m_MouseSelection_EdgeSelect;
    public struct MouseSelectionActions
    {
        private @PlayerInput m_Wrapper;
        public MouseSelectionActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @EdgeSelect => m_Wrapper.m_MouseSelection_EdgeSelect;
        public InputActionMap Get() { return m_Wrapper.m_MouseSelection; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseSelectionActions set) { return set.Get(); }
        public void SetCallbacks(IMouseSelectionActions instance)
        {
            if (m_Wrapper.m_MouseSelectionActionsCallbackInterface != null)
            {
                @EdgeSelect.started -= m_Wrapper.m_MouseSelectionActionsCallbackInterface.OnEdgeSelect;
                @EdgeSelect.performed -= m_Wrapper.m_MouseSelectionActionsCallbackInterface.OnEdgeSelect;
                @EdgeSelect.canceled -= m_Wrapper.m_MouseSelectionActionsCallbackInterface.OnEdgeSelect;
            }
            m_Wrapper.m_MouseSelectionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @EdgeSelect.started += instance.OnEdgeSelect;
                @EdgeSelect.performed += instance.OnEdgeSelect;
                @EdgeSelect.canceled += instance.OnEdgeSelect;
            }
        }
    }
    public MouseSelectionActions @MouseSelection => new MouseSelectionActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_Movement;
    private readonly InputAction m_Camera_Zoom;
    private readonly InputAction m_Camera_mousezoom;
    private readonly InputAction m_Camera_AutoRotate;
    public struct CameraActions
    {
        private @PlayerInput m_Wrapper;
        public CameraActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Camera_Movement;
        public InputAction @Zoom => m_Wrapper.m_Camera_Zoom;
        public InputAction @mousezoom => m_Wrapper.m_Camera_mousezoom;
        public InputAction @AutoRotate => m_Wrapper.m_Camera_AutoRotate;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMovement;
                @Zoom.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnZoom;
                @mousezoom.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMousezoom;
                @mousezoom.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMousezoom;
                @mousezoom.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMousezoom;
                @AutoRotate.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnAutoRotate;
                @AutoRotate.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnAutoRotate;
                @AutoRotate.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnAutoRotate;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
                @mousezoom.started += instance.OnMousezoom;
                @mousezoom.performed += instance.OnMousezoom;
                @mousezoom.canceled += instance.OnMousezoom;
                @AutoRotate.started += instance.OnAutoRotate;
                @AutoRotate.performed += instance.OnAutoRotate;
                @AutoRotate.canceled += instance.OnAutoRotate;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Graph;
    public struct PlayerActions
    {
        private @PlayerInput m_Wrapper;
        public PlayerActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Graph => m_Wrapper.m_Player_Graph;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Graph.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGraph;
                @Graph.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGraph;
                @Graph.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnGraph;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Graph.started += instance.OnGraph;
                @Graph.performed += instance.OnGraph;
                @Graph.canceled += instance.OnGraph;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IMouseSelectionActions
    {
        void OnEdgeSelect(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnMousezoom(InputAction.CallbackContext context);
        void OnAutoRotate(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnGraph(InputAction.CallbackContext context);
    }
}
