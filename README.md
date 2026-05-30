# ZoomAndPan WPF Control

A feature-rich, production-ready Zoom and Pan control for WPF. Supports smooth animated zoom, mouse and keyboard panning, scroll-viewer integration, minimap/viewbox, undo/redo, and MVVM-friendly bindings.

[![NuGet](https://img.shields.io/nuget/v/ZoomAndPan.Wpf.svg)](https://www.nuget.org/packages/ZoomAndPan.Wpf)
[![License: AGPL v3](https://img.shields.io/badge/License-AGPL_v3-blue.svg)](LICENSE)

---

## Table of Contents

- [Features](#features)
- [Screenshots](#screenshots)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [XAML Example](#xaml-example)
- [MVVM Usage](#mvvm-usage)
- [Public API](#public-api)
- [Architecture Overview](#architecture-overview)
- [How Zooming and Panning Work](#how-zooming-and-panning-work)
- [Customization](#customization)
- [Known Limitations](#known-limitations)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [Development Setup](#development-setup)
- [Changelog](#changelog)
- [License](#license)

---

## Features

- **Smooth animated zoom** with configurable duration (or instant, no-animation mode)
- **Mouse-wheel zoom** centered on the cursor position (Google Maps-style)
- **Click-and-drag panning**
- **Drag-to-zoom**: Shift + drag to draw a rectangle and zoom to it
- **Shift + click** to zoom in/out incrementally
- **Fit / Fill** the viewport with one command
- **Zoom to a specific percentage** or ratio from the minimum
- **Undo / Redo** zoom history with debounced auto-save
- **ScrollViewer integration** via `IScrollInfo` — scrollbars work natively
- **ZoomAndPanScrollViewer** wrapper for drop-in use with scrollbars
- **ZoomAndPanViewBox** minimap showing the current viewport position
- **MVVM-friendly**: exposes commands (`ZoomInCommand`, `ZoomOutCommand`, `FitCommand`, `FillCommand`, `UndoZoomCommand`, `RedoZoomCommand`, etc.) and bindable properties (`ViewportZoom`, `ContentOffsetX/Y`, `MousePosition`, etc.)
- **Multi-targeting**: .NET 6, 7, 8, and 10 (Windows)
- **Configurable minimum zoom type**: explicit value, fit-screen, or fill-screen

---

## Screenshots

> _Place screenshots or an animated GIF here showing the control in action._
>
> Suggested tools: [ScreenToGif](https://www.screentogif.com/) for recording, then add to a `docs/` folder.

---

## Installation

The package is published to NuGet with each new release:
**[https://www.nuget.org/packages/ZoomAndPan.Wpf/](https://www.nuget.org/packages/ZoomAndPan.Wpf/)**

### NuGet Package Manager

```
Install-Package ZoomAndPan.Wpf
```

### .NET CLI

```bash
dotnet add package ZoomAndPan.Wpf
```

### PackageReference (csproj)

```xml
<PackageReference Include="ZoomAndPan.Wpf" Version="*" />
```

> Replace `*` with the specific version you want to pin to. Check the [NuGet page](https://www.nuget.org/packages/ZoomAndPan.Wpf/) for the latest published version.

---

## Quick Start

1. Add the NuGet package (see above).
2. Add the XML namespace to your XAML:

```xml
xmlns:zap="clr-namespace:ZoomAndPan.Wpf;assembly=ZoomAndPan.Wpf"
```

3. Drop in the control:

```xml
<zap:ZoomAndPanScrollViewer ZoomAndPanInitialPosition="FitScreen">
    <Image Source="yourimage.png" />
</zap:ZoomAndPanScrollViewer>
```

That's it. The control renders scrollbars, handles mouse-wheel zoom, and fits the image on load.

---

## XAML Example

Below is a complete example showing common features and the control toolbar:

```xml
<Window xmlns:zap="clr-namespace:ZoomAndPan.Wpf;assembly=ZoomAndPan.Wpf"
        xmlns:sys="clr-namespace:System;assembly=System.Runtime">

    <!-- Keyboard shortcuts -->
    <Window.InputBindings>
        <KeyBinding Gesture="CTRL+OemPlus"  Command="{Binding ElementName=viewer, Path=ZoomInCommand}" />
        <KeyBinding Gesture="CTRL+OemMinus" Command="{Binding ElementName=viewer, Path=ZoomOutCommand}" />
        <KeyBinding Gesture="CTRL+Z"        Command="{Binding ElementName=viewer, Path=UndoZoomCommand}" />
        <KeyBinding Gesture="CTRL+Y"        Command="{Binding ElementName=viewer, Path=RedoZoomCommand}" />
    </Window.InputBindings>

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <!-- Main zoom/pan area -->
        <zap:ZoomAndPanScrollViewer x:Name="viewer"
                                    Grid.Row="0"
                                    ZoomAndPanInitialPosition="FitScreen"
                                    MinimumZoomType="FitScreen">
            <Image Source="yourimage.png" />
        </zap:ZoomAndPanScrollViewer>

        <!-- Toolbar -->
        <StackPanel Grid.Row="1" Orientation="Horizontal" HorizontalAlignment="Center" Margin="4">
            <Button Content="Undo"  Command="{Binding ElementName=viewer, Path=UndoZoomCommand}" Width="50" />
            <Button Content="Redo"  Command="{Binding ElementName=viewer, Path=RedoZoomCommand}" Width="50" />
            <Button Content="Fit"   Command="{Binding ElementName=viewer, Path=FitCommand}"      Width="50" />
            <Button Content="Fill"  Command="{Binding ElementName=viewer, Path=FillCommand}"     Width="50" />
            <Button Content="100%"  Command="{Binding ElementName=viewer, Path=ZoomPercentCommand}" Width="50" />
            <Button Content="-"     Command="{Binding ElementName=viewer, Path=ZoomOutCommand}"  Width="30" />
            <Slider Width="120"
                    Minimum="{Binding ElementName=viewer, Path=ZoomAndPanContent.MinimumZoomClamped}"
                    Maximum="10"
                    Value="{Binding ElementName=viewer, Path=ViewportZoom, Mode=TwoWay}" />
            <Button Content="+"     Command="{Binding ElementName=viewer, Path=ZoomInCommand}"   Width="30" />
            <TextBlock Text="{Binding ElementName=viewer, Path=ViewportZoom, StringFormat=p0}"
                       VerticalAlignment="Center" Width="55" TextAlignment="Right" />
        </StackPanel>
    </Grid>
</Window>
```

### Mouse Interactions

| Input | Action |
|---|---|
| Left-drag | Pan the content |
| Mouse wheel | Zoom in/out centered on cursor |
| Shift + left-click | Zoom in by ~10% |
| Shift + right-click | Zoom out by ~9% |
| Shift + left-drag | Draw a zoom rectangle; releases to zoom to selection |
| Double-click | (ZoomAndPanViewBox) Snap/pan to that point |

---

## MVVM Usage

Bind `ViewportZoom` two-way to your view model to programmatically control zoom:

```xml
<zap:ZoomAndPanScrollViewer
    ViewportZoom="{Binding Zoom, Mode=TwoWay}"
    ZoomAndPanInitialPosition="FitScreen">
    <Canvas Width="{Binding CanvasWidth}" Height="{Binding CanvasHeight}">
        <!-- your content -->
    </Canvas>
</zap:ZoomAndPanScrollViewer>
```

Expose commands directly from the XAML element name or relay them through your view model. All zoom/pan commands implement `ICommand` and update `CanExecute` automatically.

To programmatically navigate (e.g., from a button in your view model), access the inner `ZoomAndPanContent` property:

```csharp
viewer.ZoomAndPanContent.AnimatedSnapTo(new Point(x, y));
viewer.ZoomAndPanContent.AnimatedZoomTo(new Rect(x, y, width, height));
```

---

## Public API

### `ZoomAndPanControl` (core control)

#### Key Dependency Properties

| Property | Type | Description |
|---|---|---|
| `ViewportZoom` | `double` | Current zoom level (1.0 = 100%). Bindable two-way. |
| `ContentOffsetX` | `double` | Horizontal pan offset in content coordinates. |
| `ContentOffsetY` | `double` | Vertical pan offset in content coordinates. |
| `ContentViewportWidth` | `double` | Width of the visible area in content coordinates. |
| `ContentViewportHeight` | `double` | Height of the visible area in content coordinates. |
| `MinimumZoom` | `double` | Minimum allowed zoom (used when `MinimumZoomType = MinimumZoom`). Default: `0.1`. |
| `MaximumZoom` | `double` | Maximum allowed zoom. Default: `10.0`. |
| `MinimumZoomType` | `MinimumZoomTypeEnum` | How the minimum is calculated: `MinimumZoom`, `FitScreen`, or `FillScreen`. |
| `AnimationDuration` | `double` | Duration of animated transitions in seconds. Default: `0.4`. |
| `UseAnimations` | `bool` | Enable or disable smooth animation. Default: `true`. |
| `ZoomAndPanInitialPosition` | `ZoomAndPanInitialPositionEnum` | Initial zoom/position on load: `Default`, `FitScreen`, `FillScreen`, `OneHundredPercentCentered`. |
| `IsMouseWheelScrollingEnabled` | `bool` | When `true`, mouse wheel scrolls instead of zooms. Default: `false`. |
| `MousePosition` | `Point?` | Current mouse position in content coordinates (null when outside). |

#### Computed Properties

| Property | Type | Description |
|---|---|---|
| `FitZoomValue` | `double` | Zoom level at which the content exactly fits the viewport. |
| `FillZoomValue` | `double` | Zoom level at which the content fills the viewport (no empty space). |
| `MinimumZoomClamped` | `double` | Effective minimum zoom after applying `MinimumZoomType`. |

#### Events

| Event | Description |
|---|---|
| `ContentZoomChanged` | Fires when the zoom level changes. |
| `ContentOffsetXChanged` | Fires when the horizontal pan offset changes. |
| `ContentOffsetYChanged` | Fires when the vertical pan offset changes. |

#### Commands

| Command | Description |
|---|---|
| `ZoomInCommand` | Zoom in by ~10% centered on the current viewport center. |
| `ZoomOutCommand` | Zoom out by ~9% centered on the current viewport center. |
| `FitCommand` | Zoom to fit the content within the viewport. |
| `FillCommand` | Zoom to fill the viewport with content. |
| `ZoomPercentCommand` | Zoom to a percentage (pass as `CommandParameter`; `100` = 100%). |
| `ZoomRatioFromMinimumCommand` | Zoom to a multiple of the minimum zoom (pass as `CommandParameter`; `2` = 2× minimum). |
| `UndoZoomCommand` | Restore the previous zoom/pan state. |
| `RedoZoomCommand` | Re-apply an undone zoom/pan state. |

#### Public Methods

| Method | Description |
|---|---|
| `AnimatedZoomTo(double zoom)` | Animated zoom to a specific scale, centered on the content center. |
| `AnimatedZoomTo(Rect rect)` | Animated zoom to fit a content rectangle into the viewport. |
| `AnimatedZoomTo(double zoom, Rect rect)` | Animated zoom to a scale and rectangle. |
| `ZoomTo(double zoom)` | Instant zoom to a specific scale. |
| `ZoomTo(Rect rect)` | Instant zoom to fit a content rectangle. |
| `ZoomAboutPoint(double zoom, Point focus)` | Instant zoom keeping a content point fixed on screen. |
| `AnimatedZoomAboutPoint(double zoom, Point focus)` | Animated zoom keeping a content point fixed on screen. |
| `AnimatedZoomToCentered(double zoom)` | Animated zoom centered on the content center. |
| `SnapTo(Point contentPoint)` | Instantly pan so a content point is centered in the viewport. |
| `AnimatedSnapTo(Point contentPoint)` | Animated pan to center a content point in the viewport. |
| `SnapContentOffsetTo(Point offset)` | Instantly set the content offset. |
| `AnimatedScaleToFit()` | Animated zoom to fit the content. |
| `ScaleToFit()` | Instant zoom to fit the content. |
| `SaveZoom()` | Push the current state onto the undo stack. |

---

### `ZoomAndPanScrollViewer` (drop-in wrapper)

A `ScrollViewer` subclass that hosts a `ZoomAndPanControl` internally. Use this as your primary control in most scenarios. Exposes the same commands and key properties as `ZoomAndPanControl`, plus:

| Property | Description |
|---|---|
| `ZoomAndPanContent` | Access to the inner `ZoomAndPanControl` for advanced programmatic control. |

---

### `ZoomAndPanViewBox` (minimap)

A minimap control that shows a thumbnail of the content with a draggable rectangle representing the current viewport.

**Setup**: Set the `DataContext` to the `ZoomAndPanControl` or `ZoomAndPanScrollViewer` instance, and set `Visual` to the content element being displayed.

```xml
<zap:ZoomAndPanViewBox DataContext="{Binding ElementName=viewer}"
                       Visual="{Binding ElementName=myContent}" />
```

---

### Enumerations

**`MinimumZoomTypeEnum`**
- `MinimumZoom` — use the `MinimumZoom` property value
- `FitScreen` — minimum is the zoom level that fits the content
- `FillScreen` — minimum is the zoom level that fills the viewport

**`ZoomAndPanInitialPositionEnum`**
- `Default` — no initial positioning
- `FitScreen` — fit content to viewport on load
- `FillScreen` — fill viewport with content on load
- `OneHundredPercentCentered` — 1:1 zoom centered on content

---

## Architecture Overview

The library is split into three controls and several helpers:

```
ZoomAndPan.Wpf/
├── ZoomAndPanControl.cs                  # Core control (ContentControl + IScrollInfo + INPC)
│   ├── ZoomAndPanControl_EventHandlers.cs   # Mouse events and commands
│   ├── ZoomAndPanControl_IScrollInfo.cs     # IScrollInfo implementation (ScrollViewer integration)
│   ├── ZoomAndPanControl_Methods.cs         # Public zoom/pan API
│   └── ZoomAndPanControl_UndoRedo.cs        # Undo/Redo history
├── ZoomAndPanScrollViewer.cs             # ScrollViewer wrapper (primary consumer control)
├── ZoomAndPanViewBox.cs                  # Minimap / viewport indicator
├── AnimationHelper.cs                    # DoubleAnimation helper
├── KeepAliveTimer.cs                     # Debouncing timer for undo saves
├── ViewportHelpers.cs                    # Math helpers (clamp, clip, fit/fill zoom)
├── VisualTreeHelpers.cs                  # Visual tree traversal
├── RelayCommand.cs / RelayCommandGeneric.cs  # ICommand implementations
├── ZoomAdjustConverter.cs                # Log-scale converter for zoom sliders
├── ZoomAndPanViewBoxClampConverter.cs    # Minimap viewport rectangle sizing
└── *Enum.cs                              # MouseHandlingModeEnum, MinimumZoomTypeEnum, etc.
```

The control uses a **partial class pattern** across five files to keep the code organized. The main `ZoomAndPanControl` inherits from `ContentControl`, `IScrollInfo`, and `INotifyPropertyChanged`.

Internally, the content element's `RenderTransform` is set to a `TransformGroup` containing:
1. A `TranslateTransform` (driven by `ContentOffsetX`/`Y`)
2. A `ScaleTransform` (driven by `InternalViewportZoom`)

Template parts required:
- `PART_Content` — the `ContentPresenter` that receives the transforms
- `PART_DragZoomCanvas` — overlay canvas for the drag-zoom rectangle
- `PART_DragZoomBorder` — the visible drag-zoom rectangle

---

## How Zooming and Panning Work

### Zoom

Zoom is stored in `InternalViewportZoom` (a private dependency property used for animations) and exposed publicly as `ViewportZoom`. The coerce callback on `InternalViewportZoom` clamps it between `MinimumZoomClamped` and `MaximumZoom`.

When zooming about a point (e.g., mouse wheel), the control:
1. Records the content-space focus point (`ContentZoomFocusX/Y`)
2. Records the viewport-space anchor (`ViewportZoomFocusX/Y`)
3. Sets `_enableContentOffsetUpdateFromScale = true`
4. Animates `InternalViewportZoom` — the property change handler then updates `ContentOffsetX/Y` to keep the focus point fixed on screen

This is the Google Maps-style zoom-about-point algorithm.

### Pan

Panning updates `ContentOffsetX/Y` directly. A coerce callback on these properties clamps them so the content never scrolls beyond its boundaries. The `TranslateTransform` is updated immediately on each change.

When the content is smaller than the viewport, the control centers it automatically — no offset is applied and the translate transform is calculated to center the content.

### IScrollInfo

`ZoomAndPanControl` implements `IScrollInfo` so it can be placed inside a standard `ScrollViewer`. The extent reports the content size multiplied by the current zoom. Scrollbar offsets are converted to/from content coordinates on every update.

### Undo/Redo

Two `Stack<UndoRedoStackItem>` instances track zoom states. Each item stores `ContentOffsetX/Y`, `ContentViewportWidth/Height`, and `ViewportZoom`. State is pushed:
- Immediately on mouse-down (for discrete operations)
- After 750 ms of inactivity (for scroll-bar drags)
- After 1500 ms of inactivity (for zoom button holds)

---

## Customization

### Control Templates

Both `ZoomAndPanControl` and `ZoomAndPanScrollViewer` are lookless controls — they read their appearance entirely from a `ControlTemplate`. The sample project (`ZoomAndPan.Wpf.Sample/MainWindow.xaml`) contains a complete reference template.

Copy the template into your `App.xaml` or a `ResourceDictionary` to override the defaults.

### Styling the Drag-Zoom Rectangle

The drag-zoom border uses `{TemplateBinding Background}` and `{TemplateBinding BorderBrush}`. Override these on the control instance or in your template:

```xml
<zap:ZoomAndPanScrollViewer Background="#20007ACC" BorderBrush="#007ACC" ... />
```

### Zoom Slider with Logarithmic Scale

For a slider that feels linear to the user, use the built-in `ZoomAdjustConverter`:

```xml
<Slider>
    <Slider.Value>
        <Binding Path="ViewportZoom" Mode="TwoWay"
                 Converter="{zap:ZoomAdjustConverter}" />
    </Slider.Value>
</Slider>
```

### Disabling Animations

```xml
<zap:ZoomAndPanScrollViewer UseAnimations="False" />
```

---

## Known Limitations

- **No touch/stylus support** — the current implementation uses only mouse events. Touch pan and pinch-to-zoom are not implemented.
- **AGPL-3.0 license** — consumers of this library in network-facing applications must make their modified source available. Consider this before embedding the library in a commercial product.
- **Content must be a `FrameworkElement`** for `ZoomAndPanInitialPosition` and `ZoomAndPanViewBox` to work correctly (the control subscribes to `SizeChanged`). Content that is not a `FrameworkElement` is silently ignored.
- **Undo history is unlimited** — there is no cap on the undo stack size. Very long sessions with frequent zoom changes will accumulate memory.
- **`ZoomAndPanViewBox`** — requires `DataContext` to be set to the `ZoomAndPanControl` or `ZoomAndPanScrollViewer` instance; it does not auto-discover the control from the visual tree.

---

## Troubleshooting

**Content doesn't appear / appears clipped**
- Ensure `PART_Content` is named correctly in your `ControlTemplate`.
- Make sure the content element has a defined size (e.g., set `Width`/`Height` on a `Canvas` or use an `Image` with an explicit source).

**Zoom slider jumps or snaps**
- Use the `ZoomAdjustConverter` to apply a logarithmic scale so that slider movement feels linear across the full zoom range.

**`FitCommand` / `FillCommand` are always disabled**
- The commands disable themselves when the zoom is already within 1% of the fit/fill value. Verify that the content has a non-zero `ActualWidth`/`ActualHeight` when the command is checked.

**Mouse wheel scrolls instead of zooming**
- Ensure `IsMouseWheelScrollingEnabled` is `false` (the default). If you place the control in a `ScrollViewer`, set this to control whether the wheel scrolls or zooms.

**`ZoomAndPanInitialPosition` has no effect**
- This is applied when the content's `SizeChanged` first fires. If the content has a fixed size (e.g., `Width`/`Height` set in XAML), it fires during layout. If the content size changes later (e.g., an `Image` loaded async), the initial position is applied at that point. Ensure the content is a `FrameworkElement`.

**Undo/Redo doesn't save the state I expect**
- Call `SaveZoom()` explicitly before any programmatic zoom operation to guarantee a state is captured.

---

## Contributing

Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) before submitting a pull request.

In short:
1. Fork the repository
2. Create a feature branch: `git checkout -b feature/my-feature`
3. Make your changes and ensure the sample project runs correctly
4. Submit a pull request

---

## Development Setup

**Prerequisites**
- Visual Studio 2022+ or Rider with WPF support
- .NET 6 SDK or later (targeting net6/7/8/10-windows)

**Clone and build**

```bash
git clone https://github.com/thebibekbaniya/ZoomAndPan.git
cd ZoomAndPan
dotnet build
```

**Run the sample**

```bash
dotnet run --project ZoomAndPan.Wpf.Sample
```

**Build the NuGet package**

```bash
dotnet pack ZoomAndPan.Wpf/ZoomAndPan.Wpf.csproj -c Release
```

---

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for version history and release notes.

---

## License

This project is licensed under the **GNU Affero General Public License v3.0 (AGPL-3.0)**.  
See the [LICENSE](LICENSE) file for the full text.

The `RelayCommand.cs` and `RelayCommandGeneric.cs` files are derived from work by Laurent Bugnion (GalaSoft) and Josh Smith, used under the MIT license.

---

## Contributors

<table>
  <tr>
    <td align="center">
      <a href="https://github.com/thebibekbaniya">
        <img src="https://avatars.githubusercontent.com/u/50644015?v=4" width="80px;" alt="Bibek Baniya"/><br />
        <sub><b>Bibek Baniya</b></sub>
      </a><br />
      Author &amp; Maintainer
    </td>
  </tr>
</table>
