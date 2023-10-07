# LeoECS Lite Baking - Unity Conversion Workflow for [Leopotam ECS Lite](https://github.com/Leopotam/ecslite)

![License](https://img.shields.io/github/license/g0dzZz-coder/ecslite.baking?style=rounded-square)
![Last Commit](https://img.shields.io/github/last-commit/g0dzZz-coder/ecslite.baking?style=rounded-square)
![Code Size](https://img.shields.io/github/languages/code-size/g0dzZz-coder/ecslite.baking?style=rounded-square)

<div>
    <strong><a href="README.md">English</a> | <a href="README.RU.md">Русский</a></strong>
</div>

<details>
<summary>Table of Contents</summary>

- [Introduction](#-introduction)
    - [Features](#-features)
- [Installation](#-installation)
- [Usage Examples](#-usage-examples)
    - [Create a custom component](#create-a-custom-component)
    - [Create a new Authoring Component](#create-a-new-authoringcomponent)
    - [Selecting conversion mode](#selecting-conversion-mode)
    - [Converting Your GameObjects to Entity](#converting-your-gameobjects-to-entity)
    - [Spawning Prefabs](#spawning-prefabs)
    - [Working with Unity Editor Extension](#working-with-unity-editor-extension)
- [Dependencies](#-dependencies)
- [Collaboration](#-collaboration)
- [Support](#-support)
- [License](#-license)

</details>

## 🧾 Introduction

This package extends the functionality of the [Leopotam ECS Lite](https://github.com/Leopotam/ecslite) library
with tools for configuring entities through the **Unity Inspector** in scenes and prefabs.

### 💡 Features

- **Open Source**: This library is open source and free to use.
- **Easy to use**: Simply add `AuthoringComponent` to your component and add `ConvertScene` method to
  your `IEcsSystems`.
- **Convert Modes**: You can choose how to convert **GameObjects to **Entity**.
- **Prefab support**: You can spawn Prefabs with `AuthoringComponent`
  and it will be converted to **Entity** after spawn.
- **Extensibility**: Flexible architecture for extending functionality according to your needs.
- **Entities-like**: This library is similar to **Unity.Entities** conversion workflow.
- **Lightweight**: The library contains a small amount of code and has only one dependency.
- **Declarative**: You can control your component values within the **Unity Inspector**.

## 📥 Installation

### 📦 Using **UPM**:

1. Open the **Unity Package Manager** window.
2. Click the **+** button in the upper right corner of the window.
3. Select **Add package from git URL...**.
4. Enter the [Leopotam ECS Lite repository link](https://github.com/Leopotam/ecslite.git).
5. Click **Add**.
6. Repeat steps 2-5 for [this repository](https://github.com/g0dzZz-coder/ecslite.baking.git).

### ⚙️ Manual:

Add the following lines to `Packages/manifest.json` in the `dependencies` section:

```
"com.leopotam.ecslite": "https://github.com/Leopotam/ecslite.git",
"com.leopotam.ecslite.baking": "https://github.com/g0dzZz-coder/ecslite.baking.git"
```

## 📋 Usage Examples

### Create a custom component

```csharp
[Serializable] // <- Important to add Serializable attribute!
public struct Health
{
    public float Value;
}
```

Now you need to control health value within the **Unity Inspector**,
but **Unity Engine** works only with `MonoBehaviour` classes.
That mean you need to create `AuthoringComponent` for our component.

### Create a new AuthoringComponent.

1. With the standard baker:

```csharp
public sealed class HealthAuthoringComponent : AuthoringComponent<HealthComponent> { }
```

2. Or with a custom one:

```csharp
public sealed class HealthAuthoringComponent : AuthoringComponent<HealthComponent> 
{
    public override IBaker<HealthComponent> CreateBaker(EcsPackedEntityWithWorld entity) => new Baker(entity);

    private sealed class Baker : IBaker<HealthComponent> 
    {
        public void Bake(IAuthoring authoring) 
        {
            // Implement your logic here.
        }
    }
}
```

<details>
  <summary>Inspector preview</summary>

![Health Authoring Component](https://i.postimg.cc/Tw7K7nmS/health-component.jpg)
</details>

3. If you don't like the nesting of `Value`, you can create your own implementation of `IAuthoring`:

```csharp
public sealed class HealthAuthoringComponent : MonoBehaviour, IAuthoring
{
    [Min(0)] [SerializeField] private float _value;

    public IBaker CreateBaker(PackedEntityWithWorld entity) => new Baker(_value, entity);

    private readonly struct Baker : IBaker
    {
        private readonly float _value;
        private readonly PackedEntityWithWorld _entity;

        public Baker(float value, PackedEntityWithWorld entity)
        {
            _value = value;
            _entity = entity;
        }

        void IBaker.Bake(IAuthoring authoring)
        {
            if (_entity.Unpack(out var world, out var entity))
            {
                world.Pool<Health>().Replace(entity, _value);
            }
        }
    }
}
```

<details>
  <summary>Inspector preview</summary>

![Health Authoring Component](https://i.postimg.cc/Dy1f4KVC/health-component.jpg)
</details>

Add `HealthAuthoringComponent` to the **Inspector**.

`AuthoringEntity` will be automatically added to the **GameObject**.
This component is necessary for finding baked roots in the scene and store the packed entity from the ECS world.

Now you can configure component values within the Inspector. Congratulations!

> ⚠️ Currently, you **cannot** control values from the Inspector **during Runtime**.

### Selecting conversion mode

You can choose how to convert **GameObjects** to **Entity**.
Currently, there are 3 modes available:

<details>
  <summary>Inspector preview</summary>

![Conversion Mode](https://i.postimg.cc/4xkmSf7J/convert-method.jpg)
</details>

| Mode                | Description                                                                    |
|---------------------|--------------------------------------------------------------------------------|
| Convert and Inject  | Simply creates entities with components based on GameObjects.                  |
| Convert and Destroy | Deletes the GameObject after conversion.                                       |
| Convert and Save    | Stores the associated GameObject as an entity in the `AuthoringEntity` Script. |

You can also retrieve the value from `AuthoringEntity`:

```csharp
if (_authoringEntity.TryGetEntity().HasValue) 
{
    _authoringEntity.TryGetEntity().Value;
}
```

### Converting Your GameObjects to Entity

To automatically convert GameObjects to Entity,
create (or use existing) `IEcsSystems` and add the `ConvertScene` method:

```csharp
private void Start() 
{
    _world = new EcsWorld();    
    _systems = new EcsSystems(_world);
    _systems
        .ConvertScene() // <- Need to add this method.
        .Add(new ExampleSystem());
    
    _systems.Init();
 }
```

`ConvertScene` automatically scans the scene,
finds GameObjects with `AuthoringEntity` and `IAuthoring`,
creates an entity, and adds components to the **Entity** from the ECS world.

### Spawning Prefabs

You can create prefabs with `AuthoringComponent`,
and they will be converted to `Entity` after creation.

```csharp
Object.Instantiate(gameObject, position, rotation);
// Also works with 3rd party Assets:
PhotonNetwork.Instantiate(...)
```

### Working with Unity Editor Extension

Please, add `ConvertScene` method **after** UnityEditor extensions:

```csharp
#if UNITY_EDITOR
        // Add debug systems for custom worlds here, for example:
        .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
        .ConvertScene() // <- Need to add this method.
```

## 🖇️ Dependencies

- [Leopotam ECS Lite](https://github.com/Leopotam/ecslite) - the base ECS library.

## 🤝 Collaboration

I welcome feature requests and bug reports in
the [issues section](https://github.com/g0dzZz-coder/ecslite.baking/issues),
and I also accept [pull requests](https://github.com/g0dzZz-coder/ecslite.baking/pulls).

## 🫂 Support

I am an independent developer, and most of the development of this project is done in my free time. If you are
interested in collaborating or hiring me for a project, please check out
my [portfolio](https://github.com/Depra-Inc) and [contact me](mailto:g0dzZz1lla@yandex.ru)!

## 🔐 License

This project is distributed under the
**[Apache-2.0 license](https://github.com/g0dzZz-coder/ecslite.baking/blob/main/LICENSE.md)**

Copyright (c) 2023 Nikolay Melnikov
[n.melnikov@depra.org](mailto:n.melnikov@depra.org)