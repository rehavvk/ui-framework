# Unity UI Framework
A flexible UI framework designed specifically for the Unity Engine.

The framework is built around **declarative data binding** using **member expressions**, enabling a clean, refactor-friendly way to connect UI elements to data.

## Table of Contents

- [📥 Installation](#-installation)
- [🧠 Core Concept: What Binding Means in this Framework](#-core-concept-what-binding-means-in-this-framework)
- [🔗 What a Binding Is](#-what-a-binding-is)
- [🔁 Automatic Updates](#-automatic-updates)
- [🔗 Nested & Chained Bindings](#-nested--chained-bindings)
- [🧩 Architecture Is Optional](#-architecture-is-optional)
- [💻 Using the Framework with MVVM (Recommended)](#-using-the-framework-with-mvvm-recommended)
- [🧱 Controls](#-controls)
- [🚀 Control Groups](#-control-groups)
- [📋 UI List](#-ui-list)
  - [Data Receiver](#data-receiver)
  - [Lifecycle Events](#lifecycle-events)
  - [Custom List Item Strategy](#custom-list-item-strategy)
- [📱 UI Panel](#-ui-panel)
  - [Custom Visibility Strategy](#custom-visibility-strategy)
- [📢 Commands](#-commands)
- [📜 Binding Strategies](#-binding-strategies)
- [🔄 Bi-Directional Binding](#-bi-directional-binding)
- [🔀 Converters](#-converters)
- [🧩 Combiners](#-combiners)
- [📯 Binding Change Callback](#-binding-change-callback)
- [🔗 Binding Connections](#-binding-connections)
- [🔗 Binding Tags](#-binding-tags)
- [♻️ Pooling](#️-pooling)
- [📦️ Third Party Integrations](#️-pooling)

## 📥 Installation

#### 1. Install via git URL

Open the Package Manager in Unity and choose Add package from git URL, then enter:

```
https://github.com/rehavvk/ui-framework.git
```

from the `Add package from git URL` option.

#### 2. Validate `SerializeReference` support

This framework relies on Unity’s `SerializeReference` feature for flexible, polymorphic selection of .

- **If you are using Odin Inspector:**

  No additional setup is required. Odin provides full editor support, including dropdown selection and proper visualization of serialized reference fields.

- **If you are not using Odin Inspector:**

  Unity’s default Inspector has limited support for SerializeReference.
  To ensure a usable editor experience, you must install an additional support package that adds dropdowns and improved visualization.

  <br>Installation guide:
  https://github.com/mackysoft/Unity-SerializeReferenceExtensions

> ℹ️ **This requirement affects editor usability only. Runtime behavior is unaffected.**

## 🧠 Core Concept: What Binding Means in this Framework

At its heart, this framework is about keeping UI and data in sync automatically.

The core idea is simple:

> **UI elements bind to properties on a binding context.<br>**
> **When those properties change, the UI updates automatically.**

To make this work reliably, the framework relies on one **fundamental requirement**:

## 🔑 The Core Requirement: `INotifyPropertyChanged`

The binding system is built on top of `INotifyPropertyChanged`.

This interface allows an object to notify listeners when one of its properties changes.

```csharp
public interface INotifyPropertyChanged
{
    event PropertyChangedEventHandler PropertyChanged;
}
```

### Why this matters

Bindings only work if the framework can detect when a value has changed.

If an object:

- Implements INotifyPropertyChanged
- Raises PropertyChanged when a property changes

…then:

- Any UI element bound to that property will update automatically
- No manual refresh calls are needed
- No custom events are required

This is the **only hard requirement** for a binding source.

## 🔗 What a Binding Is

A binding is a **declarative connection** between:

> **Source Property → Binding → Target Property (UI Element)**

#### Example:

```csharp
Bind(() => exampleLabel.Text)
    .To(() => ViewModel.ExampleNumber)
    .ConvertToString();
```

#### What happens here:

1. `ViewModel.ExampleNumber` is the **source**
2. `exampleLabel.Text` is the **target**
3. The binding listens for `PropertyChanged`
4. When the value changes, the UI updates automatically

Bindings are defined using **member expressions**, which makes them:

- Mostly type-safe
- Refactor-friendly
- Easy to read and reason about

## 🔁 Automatic Updates

As long as the source object raises change notifications, updates are effortless.

```csharp
ExampleNumber++;
```

That’s it.

The framework:

- Detects the change via binding
- Updates all bound UI elements
- Avoids unnecessary updates

No events.<br>
No observers to clean up.<br>
No manual refresh logic.

## 🔗 Nested & Chained Bindings

Bindings are not limited to flat properties.

```csharp
Bind(() => exampleLabel.Text)
    .To(() => ViewModel.ExampleModel.ExampleNumber)
    .ConvertToString();
```

This allows bindings to traverse **entire object graphs**.

If **any part of the chain** changes:

- The binding reacts
- The UI updates automatically

This makes it easy to:

- Keep models clean
- Avoid duplicated data in view layers
- Express complex UI relationships declaratively

## 🧩 Architecture Is Optional

The framework **does not enforce MVVM** or any other architecture.

It only cares about one thing:

> **Does the binding source notify property changes?**

The binding context can be:

- A ViewModel
- A Controller
- A Manager
- A Game System
- A ScriptableObject wrapper
- A MonoBehaviour on the same GameObject

As long as it:

- Exposes properties
- Implements `INotifyPropertyChanged`

…it can be bound to.

## 💻 Using the Framework with MVVM (Recommended)

While not required, the framework works exceptionally well with [MVVM](https://de.wikipedia.org/wiki/Model_View_ViewModel) (Model–View–ViewModel), 
because MVVM naturally aligns with property-based binding.

**Model**
- Holds **pure application data**
- Contains **business logic**
- Has **no knowledge of UI**

```csharp
public class ExampleModel
{
    public int ExampleNumber { get; private set; }
    
    public void CountUp() => ExampleNumber++;
    public void CountDown() => ExampleNumber--;
}
```

**View Model**
- Acts as the **bridge between data and UI**
- Exposes **bindable properties**
- Notifies the View when data changes

```csharp
public class ExampleViewModel : ViewModelBase
{
    private int exampleNumber;
    
    public int ExampleNumber
    {
        get => exampleNumber;
        private set => SetField(ref exampleNumber, value);
    }
}
```

`SetField` automatically triggers change notifications used by bindings.

**View**
- References **UI Elements**
- Defines **bindings only**
- Contains **no logic besides input forwarding**

```csharp
public class ExampleView : ViewBase<ExampleViewModel>
{
    [SerializeField] private UILabelBase exampleLabel;

    protected override void SetupBindings()
    {
        Bind(() => exampleLabel.Text)
            .To(() => ViewModel.ExampleNumber)
            .ConvertToString();
    }
}
```

### 🔍 What the Base Classes Really Are

The framework comes with some MVVM base classes. These are just a **thin abstractions**, not magic.

| MVVM Name       | Underlying Type                    | Purpose                                         |
|-----------------|------------------------------------|-------------------------------------------------|
| `ViewBase<T>`   | `UIContextControlBase<T>`          | Defines a binding context and lifecycle         |
| `ViewModelBase` | `BindableMonoBehaviourBase`        | Provides bindable properties on a MonoBehaviour |
| `Model`         | `BindableBase`                     | Provides bindable properties on plain objects   |

This means:
- There is **nothing special** about “View”, “ViewModel”, or “Model”
- They are **naming conventions**, not hard rules

They exist for **convenience and clarity**, not enforcement.

## 🧱 Controls

While it's possible to use every type as binding target the framework comes with a bunch of wrappers for Unity UI Components like **TextMeshProUGUI**, **Images**, **Buttons** and **Sliders**.

> ℹ️ **It is recommended to always expose the most fitting base class of these wrappers to the inspector to be able to assign different implementations of them later when designing the actual UI.**

| Wrapper        | Unity Component       |
|----------------|-----------------------|
| `UILabel`      | `TextMeshProUGUI`     |
| `UIImage`      | `Image`               |
| `UIRawImage`   | `RawImage`            |
| `UIButton`     | `Button`              |
| `UIInputField` | `TMP_InputField`      |
| `UISlider`     | `Slider`              |
| `UIToggle`     | `Toggle`              |

All controls are derived of `UIElementBase` in some way. By this they all have a `IsVisible` property to enable or disabled their appearance.

## 🚀 Control Groups

Most of these wrappers have a group version. These components can be used to apply the setup binding to multiple other components of this wrapper type in the inspector.

## 📋 UI List

A special case is binding to lists. The framework comes with a solution for this not requiring any changes to the binding expression.
The control UIList let you bind to its property Items and handles the creation and destruction of list items. The items get pooled by default and sorted based on the chosen item factory.

Add a UIList and its dependencies to your view.

```csharp
[SerializeField] private UIListBase itemList;
```

Set their item strategy.

```csharp
private void Awake()
{
    itemList.SetItemStrategy(new PrefabUIListItemStrategy(itemRoot, itemPrefab));
}
```

And last but not least, bind it to your item source.

```csharp
Bind(() => itemList.Items)
    .To(() => Context.Items);
```

There are multiple implementations of item strategies you can choose from. 

| Strategy                             | Use Case                                                                        |
|--------------------------------------|---------------------------------------------------------------------------------|
| `PrefabUIListItemStrategy`           | Define an item prefab which is spawned for each item data.                      |
| `PredefinedUIListItemStrategy`       | Define multiple item game objects which will get populated with item data.      |
| `ContextualPrefabUIListItemStrategy` | Define a callback to chose the item prefab which is spawned for each item data. |

All item strategies come with a serializable dependencies class to expose their dependencies to the inspector.

```csharp
[SerializeField] private UIListBase itemList;
[SerializeField] private PrefabUIListItemStrategy.Dependencies itemListDependencies;

private void Awake()
{
    itemList.SetItemStrategy(new PrefabUIListItemStrategy(itemListDependencies.itemRoot, itemListDependencies.itemPrefab));
}
```

### Data Receiver

You can setup a control or any other type of MonoBehaviour to receive the item data from your list.
Simply let it implement the `IUIListItemReceiver` interface. 

The `SetListItem` method gets an `ListItem` object with the populated data and its index in the list.

```csharp
public class MyItemListElement : UIContextControlBase<Item>, IUIListItemReceiver
{
    [SerializeField] private UILabelBase titleLabel;

    protected override void SetupBindings()
    {
        base.SetupBindings();

        Bind(() => titleLabel.Text)
            .To(() => Context.Title);
    }

    void IUIListItemReceiver.SetListItem(ListItem listItem)
    {
        SetContext(listItem.Data);
    }
}
```

The UIList will check for components implementing the `IUIListItemReceiver` interface and inject the populated item data to it.

Additionally, you can specify which exactly type should be used as an item data receiver.

```csharp
private void Awake()
{
    itemList.SetItemStrategy(new PrefabUIListItemStrategy(itemListDependencies.itemRoot, itemListDependencies.itemPrefab));
    itemList.SetItemReceiverType<MyItemListElement>();
}

private void OnListItemActivated(int index, GameObject item, object data)
{
    // Do what every you want.
}

```

### Lifecycle Events

You can also react to different lifecycle events of item objects by adding a callback to them.

```csharp
private void Awake()
{
    itemList.SetItemStrategy(new PrefabUIListItemStrategy(itemListDependencies.itemRoot, itemListDependencies.itemPrefab));
    itemList.SetItemCallback(UIListItemCallback.Activated, OnListItemActivated);
}

private void OnListItemActivated(int index, GameObject item, object data)
{
    // Do what every you want.
}
```

| Strategy                           | Timing                                                            |
|------------------------------------|-------------------------------------------------------------------|
| `UIListItemCallback.Activated`     | Item object receives new data.                                    |
| `UIListItemCallback.Deactivated`   | Item object gets obsolete.                                        |
| `UIListItemCallback.Initialized`   | Item object is created or populated with data for the first time. |

### Custom List Item Strategy

You can implement your own **List Item Strategy** by implementing the `IUIListItemStrategy` strategy.

## 📱 UI Panel

UI Panels are the recommended way to enable and disable views and parts of your UI in this framework.

Add them to your view and bind their `IsVisible` property.

```csharp
[SerializeField] private UIPanelBase myPanel;

protected override void SetupBindings()
{
    base.SetupBindings();

    Bind(() => myPanel.IsVisble)
        .To(() => Context.IsActive);
}

```

In the inspector you can choose from different **VisibilityStrategies**

| Strategy                          | Use Case                                   |
|-----------------------------------|--------------------------------------------|
| `CanvasVisibilityStrategy`        | Controls a `Canvas` enable state.          |
| `CanvasGroupVisibilityStrategy`   | Controls a `CanvasGroup` alpha value.      |
| `RootVisibilityStrategy`          | Controls a `RectTransform` isActive state. |

### Custom Visibility Strategy

You can implement your own **Visibility Strategy** by deriving from `VisibilityStrategyBase` and make it `Serializable`.

## 📢 Commands

The framework comes with a command system for UI event handling. 

You can implement your own commands by implementing the `ICommand` interface or use the `ActionCommand` as quick way. 

```csharp
public class MyViewModel : ViewModelBase
{
    public ICommand MyCommand { get; private set; }
    
    private void Awake()
    {
        MyCommand = new ActionCommand(args =>
        {
            // Do something.
        });
    }
}
```

Then you can bind to it.

```csharp
public class MyView : ViewBase<MyViewModel>
{
    [SerializeField] private UIButtonBase myButton;

    protected override void SetupBindings()
    {
        base.SetupBindings();

        Bind(() => myButton.ClickCommand)
            .To(() => Context.MyCommand);
    }
}
```

## 📜 Binding Strategies

Next to member expressions you can bind to a couple of different sources.

The binding language of the framework comes with multiple strategies already built in.

| Strategy        | Use Case                                    |
|-----------------|---------------------------------------------|
| `To<T>`         | Bind to a member expression.                |
| `ToProperty`    | Bind directly to a property of an instance. |
| `ToCallback<T>` | Bind to read and write callbacks.           |
| `ToValue<T>`    | Bind directly to an instance.               |  
| `ToCustom`      | Bind you own custom `IBindingStrategy`      |

```csharp
Bind(() => myInputField.Value, BindingDirection.TwoWay)
    .To(() => Context.MyInput);
```

## 🔄 Bi-Directional Binding

You can define in which directions the binding works. 

As an example, this lets you create **input fields** which **update** data in your **view model**.

```csharp
Bind(() => myInputField.Value, BindingDirection.TwoWay)
    .To(() => Context.MyInput);
```

## 🔀 Converters

Not always is the type your model is providing the type your control is requesting.

For these cases the framework comes with conversion capabilities. 

```csharp
Bind(() => iconImage.IsVisible)
    .To(() => Context.IsIconInactive)
    .ConvertToBool();
```

There are a couple of converters available.

| Converter Method                           | Use Case                                        |
|--------------------------------------------|-------------------------------------------------|
| `ConvertTo<T>`                             | Convert to generic type.                        |
| `ConvertToBool`                            | Convert to bool.                                |
| `ConvertToInvertedBool`                    | Convert to inverted bool.                       |
| `ConvertToInt`                             | Convert to integer.                             |
| `ConvertToFloat`                           | Convert to float.                               |
| `ConvertToDouble`                          | Convert to double.                              |
| `ConvertToString`                          | Convert to string.                              |
| `ConvertToString(string format)`           | Convert to string by provided format.           |                                                 |
| `ConvertToDateTimeString(string format)`   | Convert to date time string by provided format. |
| `ConvertBy(IValueConverter converter)`     | Convert by custom `IValueConverter`.            |
| `ConvertByFunction((object value) => {})`  | Convert by delegate.                            |
| `ConvertByFunction<T>((T value) => {})`    | Convert by generic delegate.                    |

You can also provide **Converters** directly **as optional parameter** to binding methods like `To<T>`.

## 🧩 Combiners

Sometimes your model does not provide the value your control is requesting as a single property.

For these cases the framework comes with combining capabilities.

You can bind to multiple sources and define a combiner to bring these values together. 

```csharp
Bind(() => healthLabel.Text)
    .To(() => Context.Player.Health)
    .To(() => Context.Player.MaxHealth)
    .CombineByFormat("{0}/{1}");
```

There are some combiners available.

| Combiner Method                              | Use Case                            |
|----------------------------------------------|-------------------------------------|
| `CombineByFunction((object[] values) => {})` | Combine by a provided delegate      |
| `CombineByFormat(string format)`             | Combine to string by format.        |
| `CombineBy(IValueCombiner valueCombiner)`    | Convert by custom `IValueCombiner`. |

## 📯 Binding Change Callback

You can add a callback to a binding which is executed when the binding changed.

```csharp
Bind(() => healthFillerBarImage.FillAmount)
    .To(() => Context.Health)
    .OnChanged(() => 
    {
        // Do something.
    })
```

## 🔗 Binding Connections

You can connect bindings to member expressions to update when they get updated.

```csharp
Bind(() => healthFillerBarImage.FillAmount)
    .To(() => Context.Health)
    .ReevaluateWhenChanged(() => Context.MaxHealth)
```

## 🔗 Binding Tags

Sometimes it's useful to force a binding to update. You can do this by tagging the binding.

```csharp
Bind(() => healthFillerBarImage.FillAmount)
    .To(() => Context.Health)
    .WithTag("Health")
```

Then you can set specific tags dirty as needed. This method is available on all components derived from `UIControlBase`.

```csharp
SetDirty("Health", "MaxHealth");
```

## ♻️ Pooling

By default the framework spawns and destroys game objects via `Object.Instantiate` and `Object.Destroy`. 

You are able to alter that by calling the `UIGameObjectFactory.Setup` in some bootstrapping code
and override the `createAction` and `destroyAction`.

```csharp
UIGameObjectFactory.Setup((GameObject prefab, Transform parent) => 
{
    // Spawn object and return it.
},
(GameObject obj) =>
{
    // Despawn object.
});
```

---

***Happy binding with Unity UI Framework!***
