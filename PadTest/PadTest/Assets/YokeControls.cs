//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/YokeControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @YokeControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @YokeControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""YokeControls"",
    ""maps"": [
        {
            ""name"": ""Airplane"",
            ""id"": ""30fcbcc9-5da1-46ca-a8f1-8e70a9197904"",
            ""actions"": [
                {
                    ""name"": ""LeftX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""2f0d8bc2-5f55-4d4c-981a-69821bbe1e87"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftY"",
                    ""type"": ""PassThrough"",
                    ""id"": ""5e1ac207-a563-4d14-9fc7-d47226f42fa2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightX"",
                    ""type"": ""PassThrough"",
                    ""id"": ""353b872b-fe1d-4644-80b9-79f34248a9c2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d08ee8aa-3edf-4fd1-9013-1b8cd95f8046"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f74c2736-6fc4-48ff-a8c5-8fc276656856"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e1aee66-d910-4a53-9822-8fcb5bfba48b"",
                    ""path"": ""<Gamepad>/rightStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Airplane
        m_Airplane = asset.FindActionMap("Airplane", throwIfNotFound: true);
        m_Airplane_LeftX = m_Airplane.FindAction("LeftX", throwIfNotFound: true);
        m_Airplane_LeftY = m_Airplane.FindAction("LeftY", throwIfNotFound: true);
        m_Airplane_RightX = m_Airplane.FindAction("RightX", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Airplane
    private readonly InputActionMap m_Airplane;
    private List<IAirplaneActions> m_AirplaneActionsCallbackInterfaces = new List<IAirplaneActions>();
    private readonly InputAction m_Airplane_LeftX;
    private readonly InputAction m_Airplane_LeftY;
    private readonly InputAction m_Airplane_RightX;
    public struct AirplaneActions
    {
        private @YokeControls m_Wrapper;
        public AirplaneActions(@YokeControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftX => m_Wrapper.m_Airplane_LeftX;
        public InputAction @LeftY => m_Wrapper.m_Airplane_LeftY;
        public InputAction @RightX => m_Wrapper.m_Airplane_RightX;
        public InputActionMap Get() { return m_Wrapper.m_Airplane; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AirplaneActions set) { return set.Get(); }
        public void AddCallbacks(IAirplaneActions instance)
        {
            if (instance == null || m_Wrapper.m_AirplaneActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_AirplaneActionsCallbackInterfaces.Add(instance);
            @LeftX.started += instance.OnLeftX;
            @LeftX.performed += instance.OnLeftX;
            @LeftX.canceled += instance.OnLeftX;
            @LeftY.started += instance.OnLeftY;
            @LeftY.performed += instance.OnLeftY;
            @LeftY.canceled += instance.OnLeftY;
            @RightX.started += instance.OnRightX;
            @RightX.performed += instance.OnRightX;
            @RightX.canceled += instance.OnRightX;
        }

        private void UnregisterCallbacks(IAirplaneActions instance)
        {
            @LeftX.started -= instance.OnLeftX;
            @LeftX.performed -= instance.OnLeftX;
            @LeftX.canceled -= instance.OnLeftX;
            @LeftY.started -= instance.OnLeftY;
            @LeftY.performed -= instance.OnLeftY;
            @LeftY.canceled -= instance.OnLeftY;
            @RightX.started -= instance.OnRightX;
            @RightX.performed -= instance.OnRightX;
            @RightX.canceled -= instance.OnRightX;
        }

        public void RemoveCallbacks(IAirplaneActions instance)
        {
            if (m_Wrapper.m_AirplaneActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IAirplaneActions instance)
        {
            foreach (var item in m_Wrapper.m_AirplaneActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_AirplaneActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public AirplaneActions @Airplane => new AirplaneActions(this);
    public interface IAirplaneActions
    {
        void OnLeftX(InputAction.CallbackContext context);
        void OnLeftY(InputAction.CallbackContext context);
        void OnRightX(InputAction.CallbackContext context);
    }
}