# .NET 11 and C# 15 demos

Demos for the "What's new in .NET 11 and C# 15" talk.
Built and verified with .NET SDK `11.0.100-rc.2.26504.105` (pinned in `global.json`).

```bash
dotnet build Net11Demo.slnx
```

## Net11Demo - runtime, libraries and C# 15

Console app; press Enter to move to the next demo.

```bash
dotnet run --project Net11Demo
```

| File | Feature |
| --- | --- |
| `1RuntimeAsync.cs` | Runtime Async: compare the stack trace with `<Features>runtime-async=on</Features>` removed from the csproj |
| `2ProcessApi.cs` | `Process.RunAndCaptureTextAsync`, `ReadAllLinesAsync`, `StartSuspended` + `Resume`, `KillOnParentExit`, `StartAndForget`, `TryGetProcessById` |
| `3LinqJoins.cs` | `LeftJoin`, `RightJoin`, `FullJoin` |
| `4AsyncValidation.cs` | `AsyncValidationAttribute`, `IAsyncValidatableObject`, `Validator.TryValidateObjectAsync` |
| `5Libraries.cs` | `StringStream`, `ReadOnlyMemoryStream`, `StringBuilder.MoveChunks`, `ZstandardStream`, AES-encrypted ZIP entries, System.Text.Json naming and ignore attributes |
| `6CollectionExpressionArguments.cs` | `[with(capacity: ...), ..]` and comparers in collection expressions |
| `7Unions.cs` | `union` types with exhaustive `switch` |
| `8ClosedHierarchies.cs` | `closed` class hierarchies |
| `9ExtensionIndexers.cs` | Extension indexers |
| `10LabeledBreak.cs` | `break label;` and `continue label;` |

## Scripts - file-based app with `#:include`

```bash
cd Scripts
dotnet run app.cs -e GREETING=Hi
```

## Net11Web - ASP.NET Core 11 and EF Core 11

```bash
dotnet run --project Net11Web --urls http://localhost:5214
```

Use `Net11Web/Net11Web.http` to call every endpoint.

- Async validation with localized messages for Minimal APIs (`AddValidation`, `LocalizerProvider`).
- `[ShortCircuit]` on `/healthz`.
- OpenAPI 3.2 document at `/openapi/v1.json`: `HTTP QUERY`, union types as `anyOf`, typed Server-Sent Events with `itemSchema`, `format: binary` for files, `deprecated: true` from `[Obsolete]`.
- SignalR authentication refresh without reconnect and client-side cancellation reaching the hub method: `/signalr-demo`.
- Native OpenTelemetry tracing (console exporter) and Zstandard response compression.
- EF Core 11 with SQLite: complex type mapped to JSON in a TPT hierarchy, split query without the redundant JOIN, no-op CAST removal, `MaxByAsync` and `FullJoin` translation.
  Each endpoint returns the SQL it executed.

## Net11Mcp - MCP server from `dotnet new mcpserver`

```bash
dotnet run --project Net11Mcp
```

Register the server in an MCP client (VS Code, Visual Studio, Claude) as a stdio server.
`DotNet11Tools.GetDotNet11Features` returns the feature list for an area of the stack.

## Net11Blazor - Blazor 11

```bash
dotnet run --project Net11Blazor
```

- `/forms`: client-side validation in Static SSR, `<DisplayName>`, `[SupplyParameterFromTempData]` with POST-Redirect-GET, async validation in an interactive `EditForm` with `IsValidationPending`.
- `/virtualize`: variable item height, `InitialItemIndex`, `AnchorMode`, `ScrollToItemAsync`.
- `/circuit`: server-initiated circuit pause with `Circuit.RequestCircuitPauseAsync`, state restored through `[PersistentState]`.
- `/chat`: experimental `Microsoft.AspNetCore.Components.AI` `ChatPage` with an offline `IChatClient`.
- `/`: `<EnvironmentView>` and `<CacheView>`; `WithBrowserOptions` is configured in `Program.cs`.

## Net11Maui - .NET MAUI 11

```bash
dotnet build Net11Maui -t:Run -f net11.0-windows10.0.19041.0
```

- The first tab shows that the app runs on CoreCLR, a gradient `BoxView.Fill` and `LongPressGestureRecognizer`.
- The second tab uses C# expressions in XAML (preview, needs `EnablePreviewFeatures` and the XAML source generator).
- XAML files use implicit namespaces (no `xmlns` declarations).

## Net11Maui.Linux - experimental GTK4 backend

Created with the `maui-linux-gtk4` template from [dotnet/maui-labs](https://github.com/dotnet/maui-labs).
It builds on Windows; running it needs Linux with GTK4.

```bash
dotnet new install Microsoft.Maui.Platforms.Linux.Gtk4.Templates --prerelease
dotnet run --project Net11Maui.Linux
```

## Notes from RC2

- `Process.ReadAllLinesAsync` requires `RedirectStandardOutput` and `RedirectStandardError`.
- ZIP passwords are passed to `ZipArchive.CreateEntry(name, password, ZipEncryptionMethod.Aes256)` when writing and to `ZipArchiveEntry.Open(password)` when reading.
- `[JsonNamingPolicy]` takes a `JsonKnownNamingPolicy` value.
- Validation localization applies to explicit `ErrorMessage` strings and `[Display]` names.
- SignalR authentication refresh needs a `NameIdentifier` claim; without a user identifier the refresh is rejected with 403.
- Circuit pause keeps only state marked with `[PersistentState]`; the resumed circuit gets a new id.
- `Microsoft.AspNetCore.Components.AI` splits text into paragraphs only; markdown needs an app-provided mapper (for example Markdig).
  `ChatPage` fills the viewport (`100dvh`), so it is constrained with scoped CSS when embedded in a layout.
- `x:Code` is not available in RC2; the MAUI demo shows C# expressions in XAML instead.
- With `EnablePreviewFeatures`, the XAML source generator cannot build `<TabBar>` children, so `AppShell` is written in C#.
