using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using StudioLand;


public class EventTester : EditorWindow
{
    
    // Event Channel Fields
    BaseField<Object> voidChannelField;
    BaseField<Object> floatChannelField;
    BaseField<Object> minigameChannelField;

    // Data Fields
    BaseField<float> floatDataField;
    BaseField<Object> minigameDataField;

    // Event Channels
    VoidEventChannelSO voidEventChannel;
    FloatEventChannelSO floatEventChannel;
    MinigameEventChannelSO minigameEventChannel;

    // Data
    float floatData;
    MinigameSO minigameData;
    EventChannelType currentChannelType;


    // UI Element References
    VisualElement root;
    Button broadcastButton;
    VisualElement voidSection;
    VisualElement floatSection;
    VisualElement minigameSection;

    [MenuItem("Window/Event Tester")]
    public static void ShowExample()
    {
        EventTester wnd = GetWindow<EventTester>();
        wnd.titleContent = new GUIContent("Event Tester");
    }

    private void OnEnable()
    {
        // Each editor window contains a root VisualElement object
        root = rootVisualElement;

        // Import UXML -- NOTE: the "visual tree" is the structure of the UI created in UI Builder
        var original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/StudioLandCore/Editor/EventTester.uxml");
        TemplateContainer visualTreeInstance = original.CloneTree();
        root.Add(visualTreeInstance);

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/StudioLandCore/Editor/EventTester.uss");
        root.styleSheets.Add(styleSheet);


        // Event Channel set up
        voidChannelField = root.Q<BaseField<Object>>("VoidChannel");
        floatChannelField = root.Q<BaseField<Object>>("FloatChannel");
        minigameChannelField = root.Q<BaseField<Object>>("MinigameChannel");

        voidChannelField.RegisterValueChangedCallback(HandleNewVoidEventChannel);
        floatChannelField.RegisterValueChangedCallback(HandleNewFloatEventChannel);
        minigameChannelField.RegisterValueChangedCallback(HandleNewMinigameEventChannel);

        // Data set up
        floatDataField = root.Q<BaseField<float>>("FloatData");
        minigameDataField = root.Q<BaseField<Object>>("MinigameData");

        floatDataField.RegisterValueChangedCallback(HandleNewFloatData);
        minigameDataField.RegisterValueChangedCallback(HandleNewMinigameData);

        // Section set up
        voidSection = root.Q("Void");
        floatSection = root.Q("Float");
        minigameSection = root.Q("Minigame");

        // Input set up
        broadcastButton = root.Q<Button>("Broadcast");
        broadcastButton.RegisterCallback<ClickEvent>(HandleBroadcastPressed);

        root.Q<EnumField>("ChannelType").RegisterValueChangedCallback(HandleNewChannelType);

        // TODO: Show proper fields based on enum choice
    }

    private void HandleNewChannelType(ChangeEvent<System.Enum> evt)
    {
        currentChannelType = (EventChannelType)evt.newValue;
        switch((EventChannelType)evt.newValue)
        {
            case EventChannelType.Void:
                voidSection.style.display = DisplayStyle.Flex;
                floatSection.style.display = DisplayStyle.None;
                minigameSection.style.display = DisplayStyle.None;
                break;
            case EventChannelType.Float:
                voidSection.style.display = DisplayStyle.None;
                floatSection.style.display = DisplayStyle.Flex;
                minigameSection.style.display = DisplayStyle.None;
                break;
            case EventChannelType.Minigame:
                voidSection.style.display = DisplayStyle.None;
                floatSection.style.display = DisplayStyle.None;
                minigameSection.style.display = DisplayStyle.Flex;
                break;
        }
    }

    

    
    

    

    private void HandleBroadcastPressed(ClickEvent evt)
    {
        switch(currentChannelType)
        {
            case EventChannelType.Void:
                if(voidEventChannel)
                    voidEventChannel.RaiseEvent();
                break;
            case EventChannelType.Float:
                if(floatEventChannel)
                    floatEventChannel.RaiseEvent(floatData);
                break;
            case EventChannelType.Minigame:
                if(minigameEventChannel && minigameData)
                    minigameEventChannel.RaiseEvent(minigameData);
                break;
        }
    }


    private void HandleNewVoidEventChannel(ChangeEvent<Object> evt)
    {
        if(evt.newValue != null)
        {
            voidEventChannel = evt.newValue as VoidEventChannelSO;
        }
    }

    private void HandleNewFloatEventChannel(ChangeEvent<Object> evt)
    {
        if(evt.newValue != null)
        {
            floatEventChannel = evt.newValue as FloatEventChannelSO;
        }
    }
    private void HandleNewMinigameEventChannel(ChangeEvent<Object> evt)
    {
        if(evt.newValue != null)
        {
            minigameEventChannel = evt.newValue as MinigameEventChannelSO;
        } 
    }



    private void HandleNewFloatData(ChangeEvent<float> evt)
    {
        floatData = evt.newValue;
    }

    private void HandleNewMinigameData(ChangeEvent<Object> evt)
    {
        if(evt.newValue != null)
        {
            minigameData = evt.newValue as MinigameSO;
        }
        
    }
}