![The Singularity Workshop animated moniker](https://raw.githubusercontent.com/TrentBest/TheSingularityWorkshop.Renderer/development/docs/assets/moniker.svg)

# FSM_Bridge.Helix — historical adapter

**Status: deprecated experiment. This package is not intended for new projects, and no further NuGet releases are planned.**

FSM_Bridge.Helix explored connecting [FSM_API](https://github.com/TrentBest/FSM_API) state contexts to selected Helix Toolkit WPF/SharpDX objects. It is preserved as part of The Singularity Workshop's development history and as an example of an FSM-driven rendering adapter.

## Architecture

- **FSM_API** provides finite-state-machine definitions, instances, and transitions.
- **FSM_Bridge.Helix** adapts selected Helix Toolkit objects to that state model.
- **Helix Toolkit / WPF** remains responsible for the underlying 3D presentation.

This adapter is specific to its historical dependencies. It is not an engine-agnostic renderer, and it is not the current direction of the Workshop's rendering work.

## Before using

Check the source and dependency compatibility yourself before incorporating this code into an existing application. The repository may receive maintenance for source clarity or build hygiene, but it is not maintained as a supported package.

- [Repository](https://github.com/TrentBest/FSM_Helix)
- [FSM_API](https://github.com/TrentBest/FSM_API)
- [The Singularity Workshop](https://github.com/TrentBest)

MIT License.

---

<p align="center">
  <a href="https://github.com/TrentBest/TheSingularityWorkshop" aria-label="The Singularity Workshop living avatar">
    <img src="https://raw.githubusercontent.com/TrentBest/WebPage/development/TheSingularityWorkshop/wwwroot/Images/TheSingularityWorkshopLivingAvatar.svg" alt="The Singularity Workshop living avatar — a digital vortex converging on its central eye" width="112" />
  </a><br />
  <strong>The Singularity Workshop</strong><br />
  <em>If a picture is worth a thousand words, what is a living picture worth?</em><br />
  <a href="https://github.com/TrentBest">GitHub</a> · <a href="https://www.patreon.com/TheSingularityWorkshop">Support the Workshop</a>
</p>
