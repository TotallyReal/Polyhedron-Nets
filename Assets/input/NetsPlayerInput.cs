// GENERATED AUTOMATICALLY FROM 'Assets/input/NetsPlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @NetsPlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @NetsPlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""NetsPlayerInput"",
    ""maps"": [
        {
            ""name"": ""Mouse Selection"",
            ""id"": ""d3741cf8-f7ef-4300-a2eb-dad3afa8459e"",
            ""actions"": [
                {
                    ""name"": ""Edge Select"",
                    ""type"": ""Button"",
                    ""id"": ""7f7e6240-4be6-4eea-9943-e6671b0b33e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3571928e-ca73-4f18-bd46-65baa1f66b8c"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Edge Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""9ac3a76b-0c32-4c16-83cd-ede2b19f9762"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""b4c52499-bbc2-4de8-98b3-6390fba9adae"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""1731cad4-fd2b-493c-aefb-f0be26bc5fb9"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Mouse Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""07419160-564f-4437-9bf3-9c5fdbd1ce20"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Auto Rotate"",
                    ""type"": ""Button"",
                    ""id"": ""542f797e-07a2-483b-b3d4-afee178492f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e3ff63c7-f28b-4df2-9af7-50fa33bb2589"",
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
                    ""id"": ""d0e635b1-6147-4b9e-9738-6a8b7acac227"",
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
                    ""id"": ""f84bc312-b8ef-48eb-984e-52742b176757"",
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
                    ""id"": ""de4a9a8d-b6cf-4b82-bca1-c98834f94b97"",
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
                    ""id"": ""888ef07a-16c7-4364-8299-824d1b96bd93"",
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
                    ""id"": ""1d001396-d2b8-451f-862a-c656a89e36d1"",
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
                    ""id"": ""1d6cd41f-74ee-410e-b576-70129d75f3cf"",
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
                    ""id"": ""51582a0e-31d8-402f-83d5-363fe0dbb508"",
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
                    ""id"": ""95ef3b4f-392e-4f7b-9f10-2b48c88cbe97"",
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
                    ""id"": ""be69b8db-0ff8-41ca-94f7-9ef481865f0a"",
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
                    ""id"": ""2354c35e-6aed-40de-9841-2ec02772a786"",
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
                    ""id"": ""03cfdf0d-fbb8-4f08-af9c-4e1e61fc07e5"",
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
                    ""id"": ""014384c5-dc64-4e39-8cd2-329afd565e77"",
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
                    ""id"": ""b815c2e2-d841-468d-96ee-46f8ee457ff3"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mouse Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bee209a-4ed7-4e2b-b20a-0cb03fa66cf3"",
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
            ""id"": ""cd73c20d-af1b-4a70-9f76-4d462e16c018"",
            ""actions"": [
                {
                    ""name"": ""Unfold"",
                    ""type"": ""Button"",
                    ""id"": ""6b905147-ef4f-4e43-b2d6-e8e4108d6b0a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Restart"",
                    ""type"": ""Button"",
                    ""id"": ""6291e907-4b60-4c03-a4e3-bde9c4989196"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Focus"",
                    ""type"": ""Button"",
                    ""id"": ""ec0c72a3-fe3f-48ca-899b-22e52682faa9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Action"",
                    ""type"": ""Button"",
                    ""id"": ""5bda3b14-0559-4c5e-8cd7-2d054714d9ac"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NextStep"",
                    ""type"": ""Button"",
                    ""id"": ""64a45b9f-84c3-47cc-94a9-16d905139a99"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cc44e3dc-3b94-4816-b28c-3223441e6e0e"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Unfold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fcf1a38c-fd0f-480a-83fb-c0cbb334b575"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Restart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7996954-efc5-4134-a719-77aa04e731af"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Focus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Button With One Modifier"",
                    ""id"": ""afbe3ef3-69bc-4080-90f9-674cfb61df6b"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""1eebb375-f4cf-4fd2-8646-5db0fe77a87f"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""0811bd27-59ff-4e9f-b09b-8f47123f5975"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3390e797-2e6a-403b-95f3-d942869dc9d2"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextStep"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Mouse Selection
        m_MouseSelection = asset.FindActionMap("Mouse Selection", throwIfNotFound: true);
        m_MouseSelection_EdgeSelect = m_MouseSelection.FindAction("Edge Select", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Movement = m_Camera.FindAction("Movement", throwIfNotFound: true);
        m_Camera_Zoom = m_Camera.FindAction("Zoom", throwIfNotFound: true);
        m_Camera_MouseZoom = m_Camera.FindAction("Mouse Zoom", throwIfNotFound: true);
        m_Camera_AutoRotate = m_Camera.FindAction("Auto Rotate", throwIfNotFound: true);
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Unfold = m_Player.FindAction("Unfold", throwIfNotFound: true);
        m_Player_Restart = m_Player.FindAction("Restart", throwIfNotFound: true);
        m_Player_Focus = m_Player.FindAction("Focus", throwIfNotFound: true);
        m_Player_Action = m_Player.FindAction("Action", throwIfNotFound: true);
        m_Player_NextStep = m_Player.FindAction("NextStep", throwIfNotFound: true);
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

    // Mouse Selection
    private readonly InputActionMap m_MouseSelection;
    private IMouseSelectionActions m_MouseSelectionActionsCallbackInterface;
    private readonly InputAction m_MouseSelection_EdgeSelect;
    public struct MouseSelectionActions
    {
        private @NetsPlayerInput m_Wrapper;
        public MouseSelectionActions(@NetsPlayerInput wrapper) { m_Wrapper = wrapper; }
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
    private readonly InputAction m_Camera_MouseZoom;
    private readonly InputAction m_Camera_AutoRotate;
    public struct CameraActions
    {
        private @NetsPlayerInput m_Wrapper;
        public CameraActions(@NetsPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Camera_Movement;
        public InputAction @Zoom => m_Wrapper.m_Camera_Zoom;
        public InputAction @MouseZoom => m_Wrapper.m_Camera_MouseZoom;
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
                @MouseZoom.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMouseZoom;
                @MouseZoom.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMouseZoom;
                @MouseZoom.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMouseZoom;
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
                @MouseZoom.started += instance.OnMouseZoom;
                @MouseZoom.performed += instance.OnMouseZoom;
                @MouseZoom.canceled += instance.OnMouseZoom;
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
    private readonly InputAction m_Player_Unfold;
    private readonly InputAction m_Player_Restart;
    private readonly InputAction m_Player_Focus;
    private readonly InputAction m_Player_Action;
    private readonly InputAction m_Player_NextStep;
    public struct PlayerActions
    {
        private @NetsPlayerInput m_Wrapper;
        public PlayerActions(@NetsPlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Unfold => m_Wrapper.m_Player_Unfold;
        public InputAction @Restart => m_Wrapper.m_Player_Restart;
        public InputAction @Focus => m_Wrapper.m_Player_Focus;
        public InputAction @Action => m_Wrapper.m_Player_Action;
        public InputAction @NextStep => m_Wrapper.m_Player_NextStep;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Unfold.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUnfold;
                @Unfold.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUnfold;
                @Unfold.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnUnfold;
                @Restart.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRestart;
                @Restart.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRestart;
                @Restart.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRestart;
                @Focus.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFocus;
                @Focus.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFocus;
                @Focus.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnFocus;
                @Action.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAction;
                @Action.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAction;
                @Action.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAction;
                @NextStep.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNextStep;
                @NextStep.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNextStep;
                @NextStep.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNextStep;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Unfold.started += instance.OnUnfold;
                @Unfold.performed += instance.OnUnfold;
                @Unfold.canceled += instance.OnUnfold;
                @Restart.started += instance.OnRestart;
                @Restart.performed += instance.OnRestart;
                @Restart.canceled += instance.OnRestart;
                @Focus.started += instance.OnFocus;
                @Focus.performed += instance.OnFocus;
                @Focus.canceled += instance.OnFocus;
                @Action.started += instance.OnAction;
                @Action.performed += instance.OnAction;
                @Action.canceled += instance.OnAction;
                @NextStep.started += instance.OnNextStep;
                @NextStep.performed += instance.OnNextStep;
                @NextStep.canceled += instance.OnNextStep;
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
        void OnMouseZoom(InputAction.CallbackContext context);
        void OnAutoRotate(InputAction.CallbackContext context);
    }
    public interface IPlayerActions
    {
        void OnUnfold(InputAction.CallbackContext context);
        void OnRestart(InputAction.CallbackContext context);
        void OnFocus(InputAction.CallbackContext context);
        void OnAction(InputAction.CallbackContext context);
        void OnNextStep(InputAction.CallbackContext context);
    }
}
