# Contributing to ZoomAndPan WPF Control

Thank you for your interest in contributing! This document describes the process for reporting issues, proposing changes, and submitting pull requests.

---

## Table of Contents

- [Code of Conduct](#code-of-conduct)
- [Reporting Bugs](#reporting-bugs)
- [Requesting Features](#requesting-features)
- [Development Setup](#development-setup)
- [Making Changes](#making-changes)
- [Pull Request Guidelines](#pull-request-guidelines)
- [Coding Style](#coding-style)
- [Commit Messages](#commit-messages)

---

## Code of Conduct

Be respectful and constructive. Harassment of any kind will not be tolerated.

---

## Reporting Bugs

Use the GitHub [Bug Report template](.github/ISSUE_TEMPLATE/bug_report.md) and include:

- A clear, concise description of what went wrong
- Steps to reproduce (minimal XAML/code snippet if possible)
- Expected vs. actual behaviour
- .NET version and target framework
- Screenshots or a recording if relevant

---

## Requesting Features

Use the GitHub [Feature Request template](.github/ISSUE_TEMPLATE/feature_request.md). Explain:

- The problem you are trying to solve
- Your proposed solution
- Alternatives you have considered

---

## Development Setup

**Prerequisites**

- Visual Studio 2022 or JetBrains Rider (with WPF/XAML support)
- .NET SDK 6.0 or later

**Clone**

```bash
git clone https://github.com/thebibekbaniya/ZoomAndPan.git
cd ZoomAndPan
```

**Build**

```bash
dotnet build
```

**Run the sample**

```bash
dotnet run --project ZoomAndPan.Wpf.Sample
```

---

## Making Changes

1. **Fork** the repository and create your branch from `main`:

   ```bash
   git checkout -b feature/my-feature
   ```

2. Make your changes in `ZoomAndPan.Wpf/`.

3. Test manually using the sample project (`ZoomAndPan.Wpf.Sample`). Exercise the main scenarios:
   - Mouse-wheel zoom
   - Click-drag pan
   - Shift+drag zoom rectangle
   - Fit / Fill / 100% commands
   - Undo / Redo
   - Resize the window to verify layout re-computation

4. Ensure the project builds with no warnings:

   ```bash
   dotnet build -warnaserror
   ```

5. Update `CHANGELOG.md` with a brief entry under `[Unreleased]`.

---

## Pull Request Guidelines

- Keep pull requests focused on a single concern.
- Reference the issue your PR addresses (`Fixes #123`).
- Fill in the pull request template completely.
- Do not include unrelated reformatting or whitespace changes.
- Squash trivial fixup commits before requesting review.

---

## Coding Style

- Follow the existing code style (C# with WPF idioms).
- Use `var` where the type is obvious from the right-hand side.
- Prefer pattern matching (`is T x`) over casting.
- No nullable annotations are used in this project (nullable is disabled for compatibility).
- Public members should have XML doc comments (`<summary>`).
- Keep helper methods `internal` or `private`; expose only what is needed by consumers.
- Partial classes are used to split `ZoomAndPanControl` — continue using the existing file structure.

---

## Commit Messages

Use the imperative mood and a short description:

```
Fix inverted condition in DelayedSaveZoom1500Miliseconds
Add AnimatedZoomToCentered overload
Update README with MVVM usage example
```

Reference issues where relevant: `Fix #42: null reference in ArrangeOverride`.
