// GENERATED AUTOMATICALLY FROM 'Assets/Polyhedrons/Runtime/Input/MouseInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MouseInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MouseInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MouseInput"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""cd73c20d-af1b-4a70-9f76-4d462e16c018"",
            ""actions"": [
                {
                    ""name"": ""PointerSelect"",
                    ""type"": ""Button"",
                    ""id"": ""079d8a8c-9a6b-4352-b1a9-114ed1036b13"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PointerPosition"",
                    ""type"": ""Value"",
                    ""id"": ""2f0824e4-aacd-411d-958a-2e9e8be9474d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e9c2c756-d716-4ffe-8065-09d18daeb75e"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerSelect"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f9a32e0-d935-4e9c-9e2f-e250e95ec1e1"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PointerPosition"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_PointerSelect = m_Player.FindAction("PointerSelect", throwIfNotFound: true);
        m_Player_PointerPosition = m_Player.FindAction("PointerPosition", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_PointerSelect;
    private readonly InputAction m_Player_PointerPosition;
    public struct PlayerActions
    {
        private @MouseInput m_Wrapper;
        public PlayerActions(@MouseInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PointerSelect => m_Wrapper.m_Player_PointerSelect;
        public InputAction @PointerPosition => m_Wrapper.m_Player_PointerPosition;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @PointerSelect.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerSelect;
                @PointerSelect.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerSelect;
                @PointerSelect.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerSelect;
                @PointerPosition.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPosition;
                @PointerPosition.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnPointerPosition;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PointerSelect.started += instance.OnPointerSelect;
                @PointerSelect.performed += instance.OnPointerSelect;
                @PointerSelect.canceled += instance.OnPointerSelect;
                @PointerPosition.started += instance.OnPointerPosition;
                @PointerPosition.performed += instance.OnPointerPosition;
                @PointerPosition.canceled += instance.OnPointerPosition;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnPointerSelect(InputAction.CallbackContext context);
        void OnPointerPosition(InputAction.CallbackContext context);
    }
}
