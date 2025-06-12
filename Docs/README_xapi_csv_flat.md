# xAPI to CSV Mapping (Data Science Friendly)

This document describes the column structure and data transformation logic used when converting [xAPI (Experience API)](https://xapi.com/) statements to CSV format. The goal is to create a **flat**, **clean**, and **data-analysis-friendly** representation of learning activity data.

---

## 🧠 Purpose

- ✅ Suitable for tools like **pandas**, **Excel**, **Power BI**, and **Tableau**
- ✅ Keeps essential **core fields** and supports **extensions**
- ✅ Ensures **column consistency** across variable schemas

---

## 🧩 Column Mapping

| CSV Column              | xAPI Path                                         | Description                                           |
|--------------------------|---------------------------------------------------|-------------------------------------------------------|
| `timestamp`              | `statement.timestamp`                             | Statement issued time (ISO 8601)                      |
| `stored`                 | `statement.stored`                                | Time stored in LRS                                    |
| `actor_name`             | `statement.actor.name` or fallback to `mbox`      | Display name of the actor                             |
| `actor_mbox`             | `statement.actor.mbox`                            | Unique email-style ID (e.g., `mailto:alice@example`)  |
| `actor_type`            | `statement.actor.objectType`                      | `Agent` or `Group`                                    |
| `verb_id`                | `statement.verb.id`                               | Full verb URI (e.g., `http://.../answered`)           |
| `verb_display`           | `statement.verb.display.en`                       | Verb in plain English (e.g., `"answered"`)            |
| `object_id`              | `statement.object.id`                             | Target of the statement                               |
| `object_type`            | `statement.object.objectType`                     | Usually `Activity`                                    |
| `object_name`            | `statement.object.definition.name.en`             | Human-readable name                                   |
| `object_description`     | `statement.object.definition.description.en`      | Object description                                    |
| `result_score_scaled`    | `statement.result.score.scaled`                   | Normalized score [0.0 – 1.0]                          |
| `result_success`         | `statement.result.success`                        | Boolean (`true`/`false`)                              |
| `result_completion`      | `statement.result.completion`                     | Boolean                                               |
| `duration`               | `statement.result.duration`                       | Duration in ISO 8601 (e.g., `PT1M12S`)                |
| `context_instructor`     | `statement.context.instructor.name`               | Name of instructor (if present)                       |
| `context_team`           | `statement.context.team.name`                     | Name of the team/group                                |
| `context_platform`       | `statement.context.platform`                      | Device or platform used                               |
| `context_language`       | `statement.context.language`                      | e.g., `en-US`                                         |
| `statement_id`           | `statement.id`                                    | UUID for the statement                                |

---

## 🧩 Extension Columns

Any xAPI extensions (`context.extensions`, `object.definition.extensions`, etc.) are **flattened** and mapped with the prefix:

- `ext_context_...`
- `ext_object_...`
- `ext_result_...`

Example:
```csv
ext_context_level, ext_result_difficulty
"advanced",        "medium"
```

---

## 🛠 Configuration Options

In code (`CsvFormat` or `xApiStatement.ToCsvFormat()`), the following can be customized:

| Option              | Description                                   |
|---------------------|-----------------------------------------------|
| `IncludedHeaders`   | List of columns to explicitly include         |
| `ExcludedHeaders`   | List of columns to remove                     |
| `PredefinedHeaderOrder` | Priority header order (others appended)    |
| `flatten`           | Flatten nested JSON objects and arrays        |

---

## 📊 Example CSV Output

```csv
timestamp,actor_name,verb_display,object_id,result_score_scaled,ext_context_level
2025-06-08T09:45:00Z,Alice,answered,quiz:math101:q1,0.9,advanced
2025-06-08T09:46:30Z,Bob,passed,quiz:math101:q2,0.7,beginner
```

---

## 🧼 Best Practices

- Always include `statement.timestamp` and `statement.actor`
- Keep `verb_id` in URI form for semantic integrity
- Flatten deeply nested custom data only if needed
- Prefer snake_case column names for pandas compatibility

---

## 📂 Output Use Cases

- Import into `pandas.DataFrame` for ML
- Feed into ELT/ETL pipelines
- Visualize with BI dashboards
- Aggregate or cluster based on verbs, actors, or objects

---

## 💬 Questions?

For integration support or custom mapping, contact the project maintainer or refer to the [xAPI specification](https://github.com/adlnet/xAPI-Spec).
