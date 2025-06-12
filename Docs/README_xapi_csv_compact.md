
# xAPI CSV Export (Non-Flattened)

This document describes how to export xAPI statements to CSV format **without flattening nested objects**. Instead of breaking down each nested structure into multiple columns, the full object is serialized into a compact JSON string and stored in a single CSV field.

## 📦 Output Format

Each row in the CSV corresponds to a full xAPI statement. Key parts of the statement like `actor`, `verb`, `object`, and `context` are serialized as JSON and written to the CSV file.

### Example CSV Columns

| Column     | Value Description                              |
|------------|------------------------------------------------|
| `id`       | Statement ID (UUID)                            |
| `timestamp`| ISO 8601 timestamp of the event                |
| `actor`    | JSON string of the actor object                |
| `verb`     | JSON string of the verb object                 |
| `object`   | JSON string of the object                      |
| `result`   | JSON string of the result object               |
| `context`  | JSON string of the context object              |

### Example Row

```csv
id,timestamp,actor,verb,object,result,context
"550e8400-e29b-41d4-a716-446655440000","2025-06-08T12:00:00Z","{""name"":""Alice"",""mbox"":""mailto:alice@example.com""}","{""id"":""http://adlnet.gov/expapi/verbs/answered""}","{""id"":""http://example.com/question/1""}","{""success"":true}","{""platform"":""Oculus Quest""}"
```

## ✅ Benefits

- Preserves full xAPI structure inside each CSV row
- Keeps data science tools JSON-friendly
- Easier to maintain and parse when schema changes frequently

## ⚠️ Considerations

- Not easily filterable in spreadsheet tools (e.g., Excel)
- Requires post-processing to interpret nested data

## 🛠 Recommended Usage

- Ideal for ingestion into data pipelines that support JSON (e.g., pandas, Spark, BigQuery)
- Use in combination with JSON-aware tools

## 🔁 Alternative

If you need each field accessible as a column, use the **flattened** export approach. See `README_xapi_csv.md`.

---

© 2025 OmiLAXR · Exporting xAPI for analysis and ML workflows
