# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).  
This project uses [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [2.0.0] — 2026

### Fixed

- **`DelayedSaveZoom1500Miliseconds`** — inverted boolean condition (`!Running != true`) caused the cached zoom state to be overwritten while the timer was already running, so the saved state was from the *wrong* moment. Condition corrected to `Running != true` (matching the 750 ms variant).
- **`UndoRedoStackItem.ToString()`** — typo used `Rect.X` twice instead of `Rect.X, Rect.Y`, producing misleading debug output.
- **`MousePosition` dependency property** — had `MinimumOrMaximumZoom_PropertyChanged` set as its property-changed callback, causing a spurious zoom clamp every time the mouse moved. Callback removed.
- **`ArrangeOverride`** — accessing `_content.DesiredSize` without a null guard could throw `NullReferenceException` before the control template is applied. Guard added.
- **`OnContentChanged`** — unsafe cast `((FrameworkElement)newContent)` could throw if content is `null` or not a `FrameworkElement`. Replaced with `is FrameworkElement` pattern match on both old and new content.
- **`AnimatedScaleToFit`** — incorrectly called `ZoomTo(FillZoomValue)` (wrong zoom value, no animation). Corrected to `AnimatedZoomTo(FitZoomValue)`.
- **`ZoomAndPanViewBox.OnMouseDown` / `OnMouseUp`** — overrides called `base.OnMouseLeftButtonDown` / `base.OnMouseLeftButtonUp` instead of `base.OnMouseDown` / `base.OnMouseUp`, potentially causing duplicate event routing for non-left mouse buttons.
- **`ZoomAndPanViewBox.SetBackground`** — `SizeChanged` event handler was added on every call but never removed, leaking a handler reference and causing the canvas to update after the original element was replaced. Handler is now tracked as a named method and unsubscribed in `OnVisualChanged`.
- **`FindChildControl<T>`** — early `return` inside the `for` loop skipped all siblings after the first child if the first child was not of type `T` and had no matching descendants. Loop now continues to the next sibling.

### Changed

- `ZoomAndPanInitialPositionChanged` callback cleaned up (empty body, no dead commented code).

## [1.0.1] — 2024

### Fixed

- Various stability improvements

---

## [1.0.0] — 2024

### Added

- Initial release of `ZoomAndPan.Wpf`
- `ZoomAndPanControl` with animated zoom/pan, mouse-wheel zoom, drag-to-zoom
- `IScrollInfo` implementation for ScrollViewer integration
- `FitCommand`, `FillCommand`, `ZoomInCommand`, `ZoomOutCommand`, `ZoomPercentCommand`
