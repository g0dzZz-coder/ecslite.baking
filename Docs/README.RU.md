# Depra.Ecs.Baking - Unity Conversion Workflow для[Leopotam ECS Lite](https://github.com/Leopotam/ecslite)

![License](https://img.shields.io/github/license/Depra-Inc/Ecs.Baking?style=rounded-square)
![Last Commit](https://img.shields.io/github/last-commit/Depra-Inc/Ecs.Baking?style=rounded-square)
![Code Size](https://img.shields.io/github/languages/code-size/Depra-Inc/Ecs.Baking?style=rounded-square)

<div>
    <strong><a href="README.md">English</a> | <a href="README.RU.md">Русский</a></strong>
</div>

<details>
<summary>Содержание</summary>

- [Введение](#-введение)
    - [Особенности](#-особенности)
- [Установка](#-установка)
- [Примеры использования](#-примеры-использования)
    - [Создание собственного компонента](#создание-собственного-компонента)
    - [Создание нового Authoring Component](#создание-нового-authoringcomponent)
    - [Выбор режима конвертации](#выбор-режима-конвертации)
    - [Конвертация ваших объектов в Entity](#конвертация-ваших-объектов-в-entity)
    - [Спавн префабов](#спавн-префабов)
    - [Работа с расширением Unity Editor](#работа-с-расширением-unity-editor)
- [Зависимости](#-зависимости)
- [Сотрудничество](#-сотрудничество)
- [Поддержка](#-поддержка)
- [Лицензия](#-лицензия)

</details>

## 🧾 Введение

Пакет расширяет функциональность библиотеки [Leopotam ECS Lite](https://github.com/Leopotam/ecslite)
инструментами для конфигурации сущностей через **Unity Inspector** на сцене и в префабах.

### 💡 Особенности

- **Открытый исходный код**: Эта библиотека с открытым исходным кодом и бесплатна для использования.
- **Прост в использовании**: Просто добавьте `AuthoringComponent` к вашему компоненту
  и добавьте метод `ConvertScene` к вашим `IEcsSystems`.
- **Режимы конвертации**: Вы можете выбрать, как конвертировать **GameObjects** в **Entity**.
- **Поддержка префабов**: Вы можете создавать префабы с `AuthoringComponent`,
  и они будут конвертироваться в **Unity.Entity** после создания.
- **Расширяемость**: Гибкая архитектура для расширения функциональности в соответствии с вашими потребностями.
- **Похож на Entities**: Эта библиотека похожа на процесс конвертации **Entities**.
- **Легковесный**: Эта библиотека легковесная и имеет только одну зависимость.
- **Декларативный**: Вы можете управлять значениями вашего компонента в **Unity Inspector**.

## 📥 Установка

### 📦 С использованием **UPM**:

1. Откройте окно **Unity Package Manager**.
2. Нажмите кнопку **+** в верхнем правом углу окна.
3. Выберите **Add package from git URL...**.
4. Введите [ссылку на репозиторий Leopotam ECS Lite](https://github.com/Leopotam/ecslite.git).
5. Нажмите **Add**.
6. Повторите шаги 2-5 для [этого репозитория](https://github.com/g0dzZz-coder/ecslite.baking.git).

### ⚙️ Вручную:

Добавьте следующие строки в `Packages/manifest.json` в раздел `dependencies`:

```
"com.leopotam.ecslite": "https://github.com/Leopotam/ecslite.git",
"com.leopotam.ecslite.baking": "https://github.com/g0dzZz-coder/ecslite.baking.git"
```

## 📋 Примеры использования

### Создание собственного компонента

```csharp
[Serializable] // <- Важно добавить атрибут Serializable!
public struct Health
{
    public float Value;
}
```

Для управления значением `Value` через **Unity Inspector** мы можем работать только с классами `MonoBehaviour`.
Поэтому следующим шагом будет создание `AuthoringComponent` для нашего компонента.

### Создание нового AuthoringComponent

1. С стандартным бейкером:

```csharp
public sealed class HealthAuthoringComponent : AuthoringComponent<HealthComponent> { }
```

2. Или с кастомным:

```csharp
public sealed class HealthAuthoringComponent : AuthoringComponent<HealthComponent> 
{
    public override IBaker<HealthComponent> CreateBaker(PackedEntityWithWorld entity) => new Baker(entity);

    private sealed class Baker : IBaker<HealthComponent> 
    {
        public void Bake(IAuthoring authoring) 
        {
            // Реализуйте вашу логику здесь.
        }
    }
}
```

Добавьте `HealthAuthoringComponent` в **Inspector**.
<details>
  <summary>Вид в инспекторе</summary>

![Health Authoring Component](https://i.postimg.cc/Tw7K7nmS/health-component.jpg)
</details>

`AuthoringEntity` будет автоматически добавлена к **GameObject**.
Этот компонент необходим для поиска конвертированных корней в сцене и хранения упакованной сущности из мира ECS.

Теперь вы можете настроить значения компонента в инспекторе. Поздравляю!

> ⚠️ В данный момент Вы **не можете** управлять значениями из инспектора **во время выполнения**.

### Выбор режима конвертации

Вы можете выбрать, как конвертировать **GameObjects** в **Entity**.
На текущий момент доступно 3 режима:

<details>
  <summary>Вид в инспекторе</summary>

![Conversion Mode](https://i.postimg.cc/4xkmSf7J/convert-method.jpg)
</details>

| Режим               | Описание                                                                       |
|---------------------|--------------------------------------------------------------------------------|
| Convert and Inject  | Просто создает сущности с компонентами на основе GameObject.                   |
| Convert and Destroy | Удаляет GameObject после конвертации.                                          |
| Convert and Save    | Сохраняет ассоциированный GameObject как сущность в скрипте `AuthoringEntity`. |

Вы также можете извлечь значение из `AuthoringEntity`:

```csharp
if (_authoringEntity.TryGetEntity().HasValue) 
{
    _authoringEntity.TryGetEntity().Value;
}
```

### Конвертация ваших объектов в Entity

Чтобы автоматически конвертировать GameObjects в Entity,
cоздайте (или используйте существующие) `IEcsSystems` и добавьте метод `ConvertScene`:

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

`ConvertScene` автоматически сканирует cцену,
находит GameObjects с `AuthoringEntity` и `IAuthoring`,
создает сущность и добавляет компоненты к **Entity** из мира ECS.

### Спавн префабов

Вы можете создавать префабы с `AuthoringComponent`,
и они будут конвертироваться в **Entity** после создания.

```csharp
Object.Instantiate(gameObject, position, rotation);
// Также работает с 3rd party Assets:
PhotonNetwork.Instantiate(...)
```

### Работа с расширением Unity Editor

Пожалуйста, добавьте метод `ConvertScene` **после** расширений UnityEditor:

```csharp
#if UNITY_EDITOR
        // Добавьте отладочные системы для пользовательских миров здесь, например:
        .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
        .ConvertScene() // <- Необходимо добавить этот метод.
```

## 🖇️ Зависимости

- [Leopotam ECS Lite](https://github.com/Leopotam/ecslite) - базовая ECS библиотека.

## 🤝 Сотрудничество

Я рад приветствовать запросы на добавление новых функций и сообщения об ошибках
в разделе [issues](https://github.com/g0dzZz-coder/ecslite.baking/issues)
и также принимать [pull requests](https://github.com/g0dzZz-coder/ecslite.baking/pulls).

## 🫂 Поддержка

Я независимый разработчик, и большая часть разработки этого проекта выполняется в свободное время.
Если вы заинтересованы в сотрудничестве или найме меня для проекта,
ознакомьтесь с моим [портфолио](https://github.com/Depra-Inc)
и [свяжитесь со мной](mailto:g0dzZz1lla@yandex.ru)!

## 🔐 Лицензия

Этот проект распространяется под лицензией
**[Apache-2.0](https://github.com/g0dzZz-coder/ecslite.baking/blob/main/LICENSE.md)**

Copyright (c) 2023 Николай Мельников
[n.melnikov@depra.org](mailto:n.melnikov@depra.org)

