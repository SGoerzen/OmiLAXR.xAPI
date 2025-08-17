# Changelog

## [2.1.1] - 2025-08-17

### Added
- Handler in `InputSystemComposer` to generate xAPI-Statements for `OnReleasedAnyButton`.
- Pipeline delegates for richer events.
- License information added to each file.
- XML `<summary>` documentation added to each file, field, and function, plus additional code comments.
- All composers assigned to a composer group.

### Changed
- Platform information format updated to  
  `"OmiLAXR.{module}:{composerGroup}.{composerName}:v{version}:{Application.platform}"`.

### Fixed
- `LearningRecordStore` now works on WebGL.
- Platform version corrected.
- Improved reliability of the `actor ended game` statement.

### Deprecated
- None.

### Removed
- None.

### Security
- None.

## [2.1.0] - 2025-06-12

### Added
- **xApiStatement**
  - Added `.GetResultValue<T>(string key)`, `.GetContextValue<T>(string key)`, and `.GetExtensionValue<T>(string key)` for value access.
  - Introduced `WithValue(xAPI_Extensions extensions)` and `GetValue<T>(xAPI_Definition definition, T defaultValue = null)` to access and set values in activity, result, or context.
  - Implemented `ToCsvFormat(bool flatten)` for CSV conversion.
  - Added xAPI-compliant field methods:
    - `WithInteractionType(InteractionType)`
    - `WithCorrectResponses(params string[] | List<string>)`
    - `WithRef(Guid guid)`
    - `WithDuration(TimeSpan duration)`

- **Hooks**
  - Added support for timestamp pseudonymization and anonymization:
    - `ReferenceTimestampPseudonymizerHook`: Sets day to a reference date.
    - `TimestampPseudonymizerHook`: Applies a fixed date offset (default: 10 days).
    - `TimestampAnonymizerHook`: Applies a random date offset.
  - Platform metadata now includes composer and module info, e.g., `OmiLAXR.xAPI:TransformComposer:v2.1.0:OSXEditor`.

- **Samples**
  - Added samples under `Window / Package Manager / OmiLAXR.xAPI / Samples`:
    - xAPI definition files and registry used in this version.
    - `PositionDataCsvEndpoint` as a lightweight example using `IEndpoint`.
    - `CsvFileExampleEndpoint` using `CsvFile` helper class.

- **Other**
  - Added `LocalFileEndpoint` to Prefab.
  - Added duration information to teleport statements.

### Changed
- Statement URI resolution moved to composers via `xApiRegistry` in `DataProvider`.
- Renamed `xApiStatement.ActorRole` to `xApiStatement.Builder`.
- Statement `id` is now generated client-side.
- `StatementPrinter` is now enabled by default.
- `LearningRecordStore` and `BasicAuthCredentialsFileLoader` are now disabled by default.
- `Mouse` and `Keyboard` composers are now disabled by default.
- Improved fallback behavior for missing xAPI registry entries.

### Fixed
- **Critical**: Fixed an infinite loop when installing in large projects.
- Endpoint was incorrectly started even when `enabled = false`.
- General performance and compatibility improvements in the Unity Editor.

### Deprecated
- Removed `IStatement.SetComposer(IComposer)` method.

---

## [2.0.18] - 2025-06-06

### Changed
- Renamed `EventTrackingBehaviour` to `ObjectlessTrackingBehaviour` to avoid confusion with `TrackingBehaviourEvent`.
  - **Need for action:** If any xAPI Tracking Behaviours extend `EventTrackingBehaviour`, rename them to `ObjectlessTrackingBehaviour`.
- Made core xAPI Tracking Behaviours more extensible (`protected`, `virtual`).
- Improved Unity Package Manager compatibility for Unity ≥2020.3.15f.

---

## [2.0.13 - 2.0.17] - 2025-05-21

### Fixed
- `LrsCredentialsLoader` no longer starts if it is disabled.

---

## [2.0.13 - 2.0.17] - 2025-05-21

### Added
- No xAPI-specific additions in this release.

---

## [2.0.11] - 2024-11-25

### Added
- `[Global Settings]` component added to the OmiLAXR prefab.
- Support for path-based tracking names via `obj.GetTrackingName()` when enabled in `[Global Settings]`.

### Changed
- `LocalEndpoint` now allows selection of storage location: `persistentDataPath`, `temporaryCachePath`, or a custom path.

### Fixed
- Fixed incorrect path for `example.credentials.json`.

---

## [2.0.10] - 2024-11-11

### Fixed
- Fixed compatibility errors for Unity 2021.

### Changed
- Updated version.

---

## [2.0.9] - 2024-11-11

### Added
- Heart rate is now included as a result extension for each actor pipeline using a `HeartRateProvider`.

### Deprecated
- None.

### Notes
- Need for action: Ensure you're using the latest xAPI Registry JSON files. See:  
  https://gitlab.com/learntech-rwth/xapi/-/merge_requests/43/diffs

---

## [2.0.7] - 2025-06-06

### Added
- Scene name is now included in the `Context` field of xAPI statements.
- Tracking added for UI elements: `Toggle`, `Scrollbar`, `TextField`, `TextFieldTMP`.
- `"System paused Game"` and `"System resumed Game"` xAPI statements.