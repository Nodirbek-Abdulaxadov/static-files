Got it — here’s your data rewritten in the format you want:

---

**Benchmark results:**

| Method | Old Avg (us) | New Avg (us) | % Change | Old Min (us) | New Min (us) | Old Max (us) | New Max (us) |
| ------ | ------------ | ------------ | -------- | ------------ | ------------ | ------------ | ------------ |
| GetAll | 13,943.6     | 13,927.1     | -0.12%   | 6,074.1      | 6,117.2      | 23,733.5     | 23,576.9     |
| Get    | 784.6        | 798.5        | +1.77%   | 723.3        | 730.1        | 885.4        | 911.6        |

This makes it clear that:

* **GetAll** performance is basically unchanged — tiny average improvement, minimal change in range.
* **Get** slowed slightly and uses more memory.

---

**High-level Old vs New summary:**

| Aspect                   | **GetAll**                                                                                                  | **Get**                                                                       |
| ------------------------ | ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| **Average speed change** | -0.12% faster on average                                                                                    | +1.77% slower                                                                 |
| **Memory allocations**   | \~0.5 KB lower on average                                                                                   | \~3 KB higher on average                                                      |
| **Consistency**          | Performance differences are tiny and vary by sort type                                                      | New version consistently allocates more memory                                |
| **Sort impact**          | `Name` sort dominates runtime (\~18–24 ms) in both versions, far slower than `Id` or `CreatedAt` (\~6–7 ms) | Same pattern, sort choice has minimal effect on speed since queries are short |
| **Overall verdict**      | New **GetAll** is performance-neutral vs Old                                                                | New **Get** is slightly slower and heavier in allocations                     |

**In short:**

* **GetAll**: New version ≈ Old version in speed and memory.
* **Get**: New version is \~14 µs slower and uses \~3 KB more memory.
* **Sorting by Name** is the biggest cost driver in both versions.

---

Do you want me to also add **memory allocation columns** to the benchmark results table so it’s fully side-by-side? That would make the differences even easier to see.
