# FSM_Bridge.Helix

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![NuGet version](https://img.shields.io/nuget/v/TheSingularityWorkshop.FSM_Bridge.Helix?style=flat-square&logo=nuget&logoColor=white)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Bridge.Helix)
[![NuGet downloads](https://img.shields.io/nuget/dt/TheSingularityWorkshop.FSM_Bridge.Helix?logo=nuget&style=flat-square)](https://www.nuget.org/packages/TheSingularityWorkshop.FSM_Bridge.Helix)

[![Build Status](https://img.shields.io/github/actions/workflow/status/TrentBest/FSM_Bridge.Helix/dotnet.yml?branch=master&style=flat-square&logo=github)](https://github.com/TrentBest/FSM_Bridge.Helix/actions?query=workflow%3A%22dotnet.yml%22+branch%3Amaster)
[![Last commit](https://img.shields.io/github/last-commit/TrentBest/FSM_Bridge.Helix/master)](https://github.com/TrentBest/FSM_Bridge.Helix/commits/master)
[![Code Coverage](https://img.shields.io/codecov/c/github/TrentBest/FSM_Bridge.Helix)](https://github.com/TrentBest/FSM_Bridge.Helix/actions?query=workflow%3A%22dotnet.yml%22+branch%3Amaster)
[![Known Vulnerabilities](https://snyk.io/test/github/TrentBest/FSM_Bridge.Helix/badge.svg)](https://snyk.io/test/github/TrentBest/FSM_Bridge.Helix)

[![GitHub stars](https://img.shields.io/github/stars/TrentBest/FSM_Bridge.Helix?style=social)](https://github.com/TrentBest/FSM_Bridge.Helix/stargazers)
[![GitHub contributors](https://img.shields.io/github/contributors/TrentBest/FSM_Bridge.Helix)](https://github.com/TrentBest/FSM_Bridge.Helix/graphs/contributors)
[![Open Issues](https://img.shields.io/github/issues/TrentBest/FSM_Bridge.Helix)](https://github.com/TrentBest/FSM_Bridge.Helix/issues)

[**💖 Support Us**](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

A bridge to a structured reality.

***

🔍 Overview

**FSM_Bridge.Helix** is the first package in the new **Bridge Layer** of The Singularity Workshop's ecosystem. It is designed to allow developers to seamlessly integrate behaviors from the **Helix Toolkit** into the core `FSM_API`. This package acts as an adaptation layer, providing the necessary `IStateContext` implementations and FSM components to manage the state of 3D objects and viewports within applications built on the Helix Toolkit. Its primary purpose is to serve as a pathway for existing Helix content to be consumed within the `AnyApp` and `MyVR` environments.

| Feature                          | FSM_Bridge.Helix ✅ | Traditional Logic ❌ |
|----------------------------------|------------|--------------------|
| Engine agnostic                  | ✅         | ❌                 |
| Runtime-modifiable definitions   | ✅         | ❌                 |
| Deferred mutation safety         | ✅         | ❌                 |
| Named FSMs & Processing Groups   | ✅         | ❌                 |
| Built-in diagnostics & thresholds| ✅         | ❌                 |
| Pure C# with minimal external deps | ✅         | ❌                 |

***

🚀 Quickstart

1. Define a simple context (your data model):
C#
```csharp
public class MeshFSMContext : IStateContext
{
    public MeshGeometryModel3D Mesh { get; set; }
    public Color OriginalColor { get; set; }
    public bool IsSelected { get; set; }
    public bool IsValid => Mesh != null;
    public string Name { get; set; }
}
````

1.  Define and build your FSM:
    C\#

<!-- end list -->

```csharp
// Define a simple condition function
private static bool OnMeshHover(IStateContext ctx)
{
    // A simplified example of checking for mouse hover
    return ((MeshFSMContext)ctx).IsSelected;
}

FSM_API.Create.CreateFiniteStateMachine("MeshSelectionFSM")
    .State("Deselected",
        onEnter: (ctx) => {
            if (ctx is MeshFSMContext c) c.Mesh.Material = new PhongMaterial(new DiffuseMaterial(c.OriginalColor));
        })
    .State("Selected",
        onEnter: (ctx) => {
            if (ctx is MeshFSMContext c) c.Mesh.Material = new PhongMaterial(new DiffuseMaterial(Colors.Yellow));
        })
    .WithInitialState("Deselected")
    // Now define the transitions between states
    .Transition("Deselected", "Selected", OnMeshHover)
    .Transition("Selected", "Deselected", (ctx) => !OnMeshHover(ctx))
    .BuildDefinition();
```

3.  Create an instance for your context:
    C\#

<!-- end list -->

```csharp
var myMeshContext = new MeshFSMContext { Mesh = my3DModel, OriginalColor = Colors.Red };
var handle = FSM_API.Create.CreateInstance("MeshSelectionFSM", myMeshContext, "MainLoop");
```

4.  Tick the FSM from your application's main loop:
    C\#

<!-- end list -->

```csharp
// Process all FSMs in the "MainLoop" group
FSM_API.Interaction.Update("MainLoop");
```

-----

🔧 Core Concepts

```
FSMBuilder: Fluently define states, transitions, and associated OnEnter/OnExit actions.

FSMHandle: Represents a runtime instance of an FSM operating on a specific context.

IStateContext: The interface your custom data models must implement to be used by the FSM. `FSM_Bridge.Helix` provides concrete `IStateContext` implementations for Helix components, such as `MeshGeometryModel3D_FSM_Behavior.cs`.

Processing Groups: Organize and control the update cycles of multiple FSM instances.

Error Handling: Built-in thresholds and diagnostics prevent runaway logic or invalid state contexts.

Thread-Safe by Design: All modifications to FSM definitions and instances are deferred and processed safely on the main thread post-update, eliminating common concurrency issues.
```

-----

## 📦 Features at a Glance

| Capability                      | Description                                                                                                                                                                                                                                                                                                                                    |
| :------------------------------ | :--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| 🔄 **Deterministic State Logic** | Define **predictable state changes** for your 3D models and viewports based on **dynamic conditions** or **explicit triggers**, ensuring your application's behavior is consistent.                                                                    |
| 🎭 **Context-Driven Behavior** | Your FSM logic directly operates on Helix components, enabling **clean separation of concerns** (logic vs. data) for your 3D scenes.                               |
| 🧪 **Flexible Update Control** | Choose how FSMs are processed: **event-driven**, **tick-based** (every N frames), or **manual**. This adaptability is perfect for **real-time 3D simulations or complex user interactions**.                                                              |
| 🧯 **Robust Error Escalation** | Benefit from **per-instance and per-definition error tracking**, providing immediate insights to prevent runaway logic or invalid states **without crashing your application**.                                                                          |
| 🔁 **Runtime Redefinition** | FSM definitions can be **redefined while actively running**, enabling **dynamic updates, live patching, and extreme behavioral variation** without recompilation or downtime.                                                                                      |
| 🎯 **Lightweight & Performant** | Engineered for **minimal memory allocations** and **optimized performance**, ensuring your FSMs are efficient even in demanding simulation scenarios.                                                                                                           |
| ✅ **Easy to Unit Test** | The inherent **decoupling of FSM logic from context data** ensures your state machines are **highly testable in isolation**, leading to more robust and reliable code with simplified unit testing.                                                              |
| 💯 **Mathematically Provable** | With clearly defined states and transitions, the FSM architecture lends itself to **formal verification and rigorous analysis**, providing a strong foundation for high-assurance systems where correctness is paramount.                                                              |
| 🤝 **Collaborative Design** | FSMs provide a **visual and structured way to define complex behaviors**, fostering better communication between developers, designers, and domain experts.   |
|  🎮 Unity Integration Available | Now preparing for submission to the Unity Asset Store. |

-----

📘 What’s Next?

```
📖 Full Documentation & Wiki (TBD)

🧪 Unit Tests & Benchmarks (Currently Under Development)

🔌 Plugins & Extension Framework (e.g., for visual editors, debugging tools)
```

-----

🤝 Contributing

Contributions welcome\! Whether you're integrating FSM\_Bridge.Helix into your enterprise application, designing new extensions, or just fixing typos, PRs and issues are appreciated.

-----

📄 License

MIT License. Use it, hack it, build amazing things with it.

-----

🧠 Brought to you by:

The Singularity Workshop - Tools for the curious, the bold, and the systemically inclined.

<a href="https://www.patreon.com/TheSingularityWorkshop" target="_blank">
    <img src="Visuals/TheSingularityWorkshop.png" alt="Support The Singularity Workshop on Patreon" height="200" style="display: block;">
</a>

Because state shouldn't be a mess.

**Support the project:** [**Donate via PayPal**](https://www.paypal.com/donate/?hosted_button_id=3Z7263LCQMV9J)

```
