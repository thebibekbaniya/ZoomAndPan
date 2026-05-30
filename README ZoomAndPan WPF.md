# ZoomAndPan WPF Control

A feature-rich Zoom and Pan control for WPF. Supports animated zoom, mouse-wheel zoom, click-drag pan, drag-to-zoom, fit/fill viewport, undo/redo, ScrollViewer integration, and a minimap control.

## NuGet

The latest release is available at:
**https://www.nuget.org/packages/ZoomAndPan.Wpf/**

A new package is published to NuGet with each release.

## Installation

**Package Manager**
```
Install-Package ZoomAndPan.Wpf
```

**.NET CLI**
```
dotnet add package ZoomAndPan.Wpf
```

**PackageReference**
```xml
<PackageReference Include="ZoomAndPan.Wpf" Version="*" />
```

## Quick Start

```xml
xmlns:zap="clr-namespace:ZoomAndPan.Wpf;assembly=ZoomAndPan.Wpf"

<zap:ZoomAndPanScrollViewer ZoomAndPanInitialPosition="FitScreen">
    <Image Source="yourimage.png" />
</zap:ZoomAndPanScrollViewer>
```

## Documentation & Source

Full documentation, samples, and source code:
**https://github.com/thebibekbaniya/ZoomAndPan**
