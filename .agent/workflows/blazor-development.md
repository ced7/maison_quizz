---
description: Blazor .NET 10 WebAssembly Development Workflow
---

# Blazor .NET 10 WebAssembly Development Workflow

This workflow guides the development of Blazor WebAssembly applications following .NET 10 best practices.

## 1. Project Specification

Before starting development, document the following:

### Application Purpose
- **What**: [Describe what the application does]
- **Who**: [Target users/audience]
- **Why**: [Problem it solves or value it provides]

### Core Features
List the main features/functionality:
1. [Feature 1]
2. [Feature 2]
3. [Feature 3]

### Technical Requirements
- Blazor WebAssembly Standalone
- .NET 10
- Target browsers: Modern browsers, mobile support
- Authentication needed: No
- API integration: No

## 2. Architecture Planning

### Component Structure
- Plan your component hierarchy
- Identify reusable components
- Define component parameters and events

### State Management
- Determine state management approach:
  - Component state (for simple apps)
  - Cascading parameters
  - Service-based state
  - Fluxor or other state management library

### Routing
- Define routes and navigation structure
- Plan for route parameters
- Consider authorization requirements per route

## 3. Development Best Practices

### Component Guidelines
- Use `.razor` files for components
- Place components in `Components/` or `Pages/` directories
- Keep components focused and single-responsibility
- Use code-behind (`.razor.cs`) for complex logic

### Naming Conventions
- Components: PascalCase (e.g., `QuizCard.razor`)
- Parameters: PascalCase with `[Parameter]` attribute
- Services: Interface + Implementation (e.g., `IQuizService`, `QuizService`)
- CSS Isolation: Use `.razor.css` files for component-specific styles

### Performance Optimization
- Use `@key` directive for list rendering
- Implement `ShouldRender()` for expensive components
- Lazy load assemblies when possible
- Minimize JavaScript interop calls
- Use streaming rendering where appropriate

### Dependency Injection
- Register services in `Program.cs`
- Use appropriate service lifetimes:
  - `AddSingleton`: Shared across entire app
  - `AddScoped`: Per user session (WebAssembly scope)
  - `AddTransient`: New instance each time

## 4. File Organization

```
/Layout
  - MainLayout.razor
  - NavMenu.razor
/Shared
  - [Reusable components]
/Pages
  - [Page components with @page directive]
/Services
  - [Service interfaces and implementations]
/Models
  - [Data models and DTOs]
/wwwroot
  - index.html
  /css
    - app.css
    - [component].razor.css (auto-generated)
  /js
    - [JavaScript interop files]
```

## 5. Development Steps

### Step 1: Create Models
Define your data models and DTOs first

### Step 2: Create Services
Implement business logic and data access services

### Step 3: Build Components
Start with layout, then shared components, then pages

### Step 4: Implement Routing
Set up routes and navigation

### Step 5: Add Styling
Apply CSS (using PicoCSS or custom styles)

### Step 6: Test Functionality
// turbo
Test in browser with hot reload:
```bash
dotnet watch run
```

### Step 7: Optimize
Review performance, bundle size, and user experience

## 6. Common Patterns

### Event Handling
```csharp
<button @onclick="HandleClick">Click Me</button>

@code {
    private void HandleClick()
    {
        // Handle event
    }
}
```

### Two-Way Binding
```csharp
<input @bind="inputValue" @bind:event="oninput" />

@code {
    private string inputValue = "";
}
```

### Component Parameters
```csharp
[Parameter]
public string Title { get; set; } = string.Empty;

[Parameter]
public EventCallback<string> OnValueChanged { get; set; }
```

### Lifecycle Methods
- `OnInitialized()` / `OnInitializedAsync()`: Component initialization
- `OnParametersSet()` / `OnParametersSetAsync()`: After parameters set
- `OnAfterRender()` / `OnAfterRenderAsync()`: After component renders
- `Dispose()`: Cleanup (implement `IDisposable`)

## 7. JavaScript Interop

### Calling JavaScript from C#
```csharp
@inject IJSRuntime JS

await JS.InvokeVoidAsync("functionName", args);
var result = await JS.InvokeAsync<ReturnType>("functionName", args);
```

### Calling C# from JavaScript
Use `[JSInvokable]` attribute on static methods

## 8. Testing

### Build the Project
```bash
dotnet build
```

### Run the Application
// turbo
```bash
dotnet run
```

### Run Tests (if configured)
```bash
dotnet test
```

## 9. Deployment Checklist

- [ ] Set `<RunAOTCompilation>true</RunAOTCompilation>` for production (optional)
- [ ] Optimize assets (minify CSS/JS)
- [ ] Configure base href in index.html
- [ ] Test in target browsers
- [ ] Review security (CORS, API keys, etc.)
- [ ] Build for release: `dotnet publish -c Release`

## Notes

- Blazor WebAssembly runs entirely in the browser
- Initial load downloads .NET runtime and assemblies
- No server-side state between requests
- Use browser storage (localStorage/sessionStorage) for persistence
- Consider PWA features for offline support