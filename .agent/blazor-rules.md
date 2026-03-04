# Blazor .NET 10 WebAssembly Development Rules

This document contains important rules and guidelines for developing Blazor WebAssembly applications with .NET 10.

## Core Principles

### 1. Component-Based Architecture
- **Always** break UI into reusable components
- **Keep components focused**: One component should do one thing well
- **Use composition**: Build complex UIs from simple components

### 2. Blazor WebAssembly Specifics
- **Remember**: Code runs in the browser, not on a server
- **No server state**: Each user has their own isolated runtime
- **Download size matters**: Minimize dependencies and use lazy loading
- **Security**: Never store secrets in client-side code

## File Structure Rules

### Component Files
- **Pages** go in `/Pages` directory and must have `@page` directive
- **Reusable components** go in `/Shared` directory
- **Layout components** go in `/Layout`
- **Shared components** go in `/Shared`

### Code Organization
- **Use code-behind** (`.razor.cs`) when component logic exceeds ~50 lines
- **Keep razor markup clean**: Move complex C# to code-behind or services
- **Services** go in `/Services` directory with interface + implementation pattern
- **Models** go in `/Models` directory

### Styling
- **Use CSS isolation**: Create `.razor.css` files for component-specific styles
- **Global styles**: Put in `/wwwroot/css/app.css`
- **Framework styles** (like PicoCSS): Link in `wwwroot/index.html`

## Naming Conventions

### Components
- **PascalCase** for component names: `QuizCard.razor`, `UserProfile.razor`
- **Descriptive names**: Name should indicate what the component does
- **Avoid generic names**: Don't use `Component1.razor` or `Page1.razor`

### Parameters and Properties
- **PascalCase** for public parameters: `[Parameter] public string Title { get; set; }`
- **camelCase** for private fields: `private string userName;`
- **Descriptive names**: Avoid abbreviations unless very common

### Services
- **Interface pattern**: `IQuizService` + `QuizService`
- **Suffix with "Service"**: `AuthenticationService`, `DataService`

## Dependency Injection Rules

### Service Registration (in Program.cs)
```csharp
// Singleton: Shared across entire application lifetime
builder.Services.AddSingleton<IMyService, MyService>();

// Scoped: Per user session (in WebAssembly, similar to Singleton)
builder.Services.AddScoped<IQuizService, QuizService>();

// Transient: New instance every time
builder.Services.AddTransient<ITemporaryService, TemporaryService>();
```

### Service Usage
- **Always inject via constructor** or `@inject` directive
- **Don't use `new`** for services that need DI
- **Prefer interfaces** over concrete types

## Component Lifecycle Rules

### Initialization
- **Use `OnInitializedAsync()`** for async initialization
- **Use `OnInitialized()`** for sync initialization
- **Don't do heavy work in constructor**: Use lifecycle methods instead

### Parameter Changes
- **Use `OnParametersSetAsync()`** to react to parameter changes
- **Remember**: This runs on initial render AND when parameters change

### Rendering
- **Use `OnAfterRenderAsync()`** for JavaScript interop
- **Check `firstRender` parameter** to avoid repeated operations
- **Don't call `StateHasChanged()`** in OnAfterRender

### Cleanup
- **Implement `IDisposable`** for cleanup
- **Unsubscribe from events** in `Dispose()`
- **Cancel ongoing operations** when component is disposed

## Data Binding Rules

### One-Way Binding
```csharp
<input value="@myValue" />
```

### Two-Way Binding
```csharp
<input @bind="myValue" />
```

### Event Binding
```csharp
<button @onclick="HandleClick">Click</button>
```

### Binding Modifiers
- **`@bind:event="oninput"`**: Update on every keystroke
- **`@bind:after="OnValueChanged"`**: Call method after binding updates
- **`@bind:format`**: Format dates and numbers

## Performance Rules

### Rendering Optimization
- **Use `@key` directive** when rendering lists to help Blazor track items
- **Override `ShouldRender()`** for expensive components that don't need frequent updates
- **Avoid inline lambda expressions** in render markup (creates new delegate each render)

### Memory Management
- **Dispose of event handlers**: Unsubscribe in `Dispose()`
- **Cancel async operations**: Use `CancellationToken` and cancel in `Dispose()`
- **Avoid memory leaks**: Don't hold references to disposed components

### Loading Optimization
- **Lazy load assemblies**: Use `<Router>` with lazy loading for large apps
- **Minimize initial bundle**: Move large dependencies to lazy-loaded sections
- **Use AOT compilation** for production (adds build time but improves runtime)

## JavaScript Interop Rules

### Calling JavaScript
```csharp
@inject IJSRuntime JS

// Void return
await JS.InvokeVoidAsync("myFunction", arg1, arg2);

// With return value
var result = await JS.InvokeAsync<string>("myFunction", arg1);
```

### Best Practices
- **Always use async methods**: `InvokeVoidAsync`, `InvokeAsync<T>`
- **Handle exceptions**: JavaScript calls can fail
- **Minimize interop calls**: They have overhead
- **Use `OnAfterRenderAsync`**: Don't call JS before component is rendered

### Calling C# from JavaScript
- **Use `[JSInvokable]`** attribute
- **Make methods static** or use instance references carefully
- **Handle serialization**: Only simple types and JSON-serializable objects

## State Management Rules

### Component State
- **Use for simple, local state**: Single component or parent-child
- **Pass via parameters**: Use `[Parameter]` for parent-to-child
- **Use EventCallback**: For child-to-parent communication

### Cascading Values
- **Use for cross-cutting concerns**: Theme, user info, etc.
- **Don't overuse**: Can make dependencies unclear
- **Name cascading parameters**: Use `Name` property for multiple values of same type

### Service-Based State
- **Use for shared state**: Multiple components need same data
- **Implement change notifications**: Use events or observables
- **Call `StateHasChanged()`**: When state changes externally

## Routing Rules

### Page Directive
```csharp
@page "/quiz/{QuizId:int}"
```

### Route Constraints
- **Use constraints**: `:int`, `:bool`, `:datetime`, `:guid`
- **Make optional with `?`**: `{QuizId:int?}`
- **Catch-all with `*`**: `{*pageRoute}`

### Navigation
```csharp
@inject NavigationManager Navigation

Navigation.NavigateTo("/quiz/5");
Navigation.NavigateTo("/quiz/5", forceLoad: true); // Force full reload
```

## Security Rules

### Client-Side Security
- **Never trust client**: All validation must be server-side too
- **No secrets in code**: API keys, passwords must be server-side
- **Use HTTPS**: Always in production
- **Validate all inputs**: Even if validated client-side

### Authentication
- **Use proper auth**: Don't roll your own
- **Secure API calls**: Use tokens, not credentials
- **Handle expired tokens**: Refresh or redirect to login

## Error Handling Rules

### Component Errors
```csharp
@try
{
    // Component markup
}
@catch (Exception ex)
{
    <p>Error: @ex.Message</p>
}
```

### Error Boundaries
- **Use `<ErrorBoundary>`**: Wrap sections that might fail
- **Provide fallback UI**: Show user-friendly error messages
- **Log errors**: Send to logging service

### Async Errors
- **Always await**: Don't use `.Result` or `.Wait()`
- **Handle exceptions**: Use try-catch in async methods
- **Show loading states**: While async operations run

## Testing Rules

### Unit Testing
- **Test component logic**: Extract to services for easier testing
- **Use bUnit**: For component testing
- **Mock dependencies**: Use interfaces for easy mocking

### Integration Testing
- **Test user flows**: Complete scenarios
- **Test API integration**: If using backend
- **Test in target browsers**: Blazor behavior can vary

## Build and Deployment Rules

### Development
```bash
dotnet watch run  # Hot reload during development
```

### Production Build
```bash
dotnet publish -c Release
```

### Configuration
- **Use appsettings.json**: For configuration
- **Environment-specific settings**: `appsettings.Development.json`, etc.
- **Set base href**: In `index.html` for non-root deployments

### AOT Compilation (Optional)
- **Pros**: Better runtime performance
- **Cons**: Longer build times, larger download
- **Enable in .csproj**: `<RunAOTCompilation>true</RunAOTCompilation>`

## Common Pitfalls to Avoid

1. **Don't use `StateHasChanged()` excessively**: Blazor handles most cases automatically
2. **Don't forget to dispose**: Memory leaks are common
3. **Don't block the UI thread**: Use async/await properly
4. **Don't put business logic in components**: Use services
5. **Don't ignore the browser console**: Errors often appear there first
6. **Don't use `InvokeAsync` before `OnAfterRender`**: Component must be rendered first
7. **Don't forget `@key` in loops**: Causes rendering issues
8. **Don't use `NavigationManager.NavigateTo()` in `OnInitialized`**: Can cause issues

## Resources

- Official Docs: https://learn.microsoft.com/en-us/aspnet/core/blazor/
- Blazor University: https://blazor-university.com/
- Awesome Blazor: https://github.com/AdrienTorris/awesome-blazor
