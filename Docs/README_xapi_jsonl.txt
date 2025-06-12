
# xAPI Export: JSON Lines (.jsonl / .txt)

This document describes the format and structure used for exporting xAPI statements as **JSON Lines (JSONL)** — a newline-delimited JSON format that is ideal for large-scale analytics and processing.

## 📄 What is JSON Lines?

JSON Lines (also known as `.jsonl` or `.ndjson`) is a format where:
- Each line in the file is a valid JSON object.
- There is no outer array or root object.
- Files can be read incrementally, one JSON object at a time.

## 📦 xAPI JSONL Export

Each line in the exported file corresponds to **one full xAPI statement**, encoded as a JSON object. All properties from the xAPI specification are preserved, including nested structures such as `actor`, `object`, `context`, and `result`.

### Example Output

```json
{"id":"550e8400-e29b-41d4-a716-446655440000","timestamp":"2025-06-08T12:00:00Z","actor":{"name":"Alice","mbox":"mailto:alice@example.com"},"verb":{"id":"http://adlnet.gov/expapi/verbs/answered"},"object":{"id":"http://example.com/question/1"}}
{"id":"550e8400-e29b-41d4-a716-446655440001","timestamp":"2025-06-08T12:00:01Z","actor":{"name":"Bob","mbox":"mailto:bob@example.com"},"verb":{"id":"http://adlnet.gov/expapi/verbs/viewed"},"object":{"id":"http://example.com/video/1"}}
```

## ✅ Benefits of JSONL

- **Streaming-friendly**: Ideal for large datasets and incremental processing.
- **Line-by-line parsing**: Supports efficient line-by-line ingestion in Python (`pandas`, `jsonlines`), Spark, and BigQuery.
- **Schema-flexible**: New fields in future statements won’t break the format.

## 🛠 Best Practices

- Always use UTF-8 encoding.
- End each line with `\n` (not a comma).
- Avoid nested arrays unless explicitly needed — they complicate streaming.

## 🔁 Comparison with CSV

| Feature       | CSV             | JSONL              |
|---------------|------------------|---------------------|
| Human-readable| ✅               | ✅ (less readable)  |
| Schema-fixed  | ✅               | ❌                 |
| Nested support| ❌ (flattened)   | ✅ (native JSON)    |
| Parse speed   | Fast             | Streaming-capable   |
| Ideal for ML  | ⚠️ (preprocess)  | ✅                 |

## 📂 Recommended File Extension

- `.jsonl`
- `.ndjson`
- `.txt` (if required for legacy systems)

---

© 2025 OmiLAXR · Exporting xAPI statements for high-volume analytics and machine learning workflows
