# Azurion — Hardware Test Simulator & Automation Trainer

A learning project for **hardware test automation in healthcare**, modelled on the Philips
**Azurion** interventional X-ray system. It pairs a software **simulator of a motorised C-arm and
patient table** (the "system under test") with a **Reqnroll (C#/.NET) Gherkin** test framework that
drives it — and a **live web dashboard** so you can *watch* the hardware move while your tests run.

The whole point: hardware testing is harder than web testing because movements take real time, settle
late, and rest a little off-target. Scripts that assert too early are **flaky**. This project lets you
feel that first-hand and learn the discipline that fixes it — *wait until the system settles, then
assert with a tolerance*.

> **New to all this? Start with the friendly illustrated guide:** open
> [`docs/guide.html`](docs/guide.html) in a browser. It explains the whole project in plain language —
> no experience needed — and walks through running it step by step.

```
Azurion.slnx
├─ src/
│  ├─ Azurion.Simulator/   the system under test: axes, motion engine, limits, collisions, flakiness
│  └─ Azurion.Web/         ASP.NET Core host: shared simulator, REST API, live dashboard
└─ tests/
   └─ Azurion.Tests/       Reqnroll + NUnit: feature files, step defs, wait helpers — plus unit tests
```

## Prerequisites

- **.NET 10 SDK** (`dotnet --version` → 10.x)
- A modern web browser (for the dashboard)

> This repo pins NuGet to the public feed via `nuget.config`, so it restores independently of any
> machine-level private feeds.

## Quick start

### 1. Build & run the tests headless (no setup)

```bash
dotnet test
```

With nothing else running, the test suite boots the web app **in-process** (via
`WebApplicationFactory`) and runs fully headless. Everything should pass (8 unit tests + the Gherkin
scenarios).

### 2. Watch your tests drive the hardware (the fun way)

In **terminal 1**, start the simulator + dashboard:

```bash
dotnet run --project src/Azurion.Web
```

Open the dashboard at **http://localhost:5023** — you'll see the C-arm (side and head views), every
axis with live readouts, jog controls, and a flakiness toggle.

In **terminal 2**, run the tests:

```bash
dotnet test
```

The suite auto-detects the running server and drives **that** instance, so the C-arm visibly moves in
your browser as scenarios execute. (Override the target with `AZURION_BASE_URL` if you run on a
different port.)

### 3. Feel the flakiness lesson

On the dashboard, set **Flakiness → Harsh**, then run the tests again. Watch the arm overshoot, drift,
and settle late — yet the scenarios in `Flakiness.feature` still pass, because they wait for the
system to settle and assert within tolerance. That contrast is the core skill the project teaches.

## What's modelled

**Axes** — Stand: rotation (RAO/LAO), angulation (CRAN/CAUD), roll (propeller), SID. Table:
longitudinal, lateral, height, tilt, cradle. Each has hard travel limits and a travel speed.

**Behaviour** — commands set a *target*; the motion plays out over real time on a 20 Hz loop. Axes
report `IsMoving` / `IsSettled`.

**Safety** — out-of-range commands raise an `OutOfRange` fault (no silent clamp); a steep angulation
with a raised, off-centre table trips a `Collision` fault and halts motion; an emergency stop blocks
all commands until `reset`.

**Flakiness** (`Off` / `Mild` / `Harsh`) — injects settle delay, resting-position jitter, overshoot,
and speed variance, all seeded so runs are reproducible.

## REST API

`GET /api/system/state` and `POST` to `/move`, `/moveby`, `/speed`, `/stop`, `/home`,
`/emergency-stop`, `/reset`, `/clear-faults`, `/flakiness`. See
[`src/Azurion.Web/Program.cs`](src/Azurion.Web/Program.cs).

## The test framework

- [`Features/`](tests/Azurion.Tests/Features) — Gherkin: `Positioning`, `Limits`, `Collision`,
  `Flakiness`.
- [`Steps/PositioningSteps.cs`](tests/Azurion.Tests/Steps/PositioningSteps.cs) — maps Gherkin to the
  client.
- [`Support/AzurionClient.cs`](tests/Azurion.Tests/Support/AzurionClient.cs) — the "page object" for
  hardware (typed REST wrapper).
- [`Support/Wait.cs`](tests/Azurion.Tests/Support/Wait.cs) — poll-until-settled helpers (**the heart
  of the lesson**).
- [`Support/TestHost.cs`](tests/Azurion.Tests/Support/TestHost.cs) — external-vs-in-process target
  resolution.
- [`Unit/MovementEngineTests.cs`](tests/Azurion.Tests/Unit/MovementEngineTests.cs) — fast in-process
  tests of the simulator's physics.

### Useful commands

```bash
dotnet test --filter "FullyQualifiedName~Unit"          # just the fast simulator unit tests
dotnet test --filter "FullyQualifiedName~Flakiness"     # just the flakiness scenarios
```

## Where to take it next

The simulator is data-driven (one `Axis` type, a list of axes), so extending it to the **acquisition**
or **viewing** domains the interview mentioned is mostly adding axes/commands — no rearchitecting.
Other good exercises: add a tag-driven flakiness profile per scenario, write a scenario that retries a
stalled axis, or add a collision *pre-check* that refuses the command before any motion.
