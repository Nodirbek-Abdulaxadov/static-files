Bennchmark results:

| Method | Old Avg (us) | New Avg (us) | % Change | Old Min (us) | New Min (us) | Old Max (us) | New Max (us) |
| ------ | ------------ | ------------ | -------- | ------------ | ------------ | ------------ | ------------ |
| GetAll | 10,894.19    | 10,631.78    | -2.41%   | 6,328.3      | 6,398.5      | 24,008.9     | 21,047.7     |
| Get    | 768.37       | 790.97       | +2.94%   | 722.3        | 731.3        | 883.0        | 916.0        |

This makes it clear that:

* **GetAll** still has the widest range and saw a slight speedup.
* **Get** has a much tighter range but experienced a small slowdown.

---

Here’s the high-level **Old vs New** summary from your benchmarks:

| Aspect                   | **GetAll**                                                                                                  | **Get**                                                                       |
| ------------------------ | ----------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------- |
| **Average speed change** | Essentially no change (−0.12% faster on average)                                                            | Slightly slower (+1.77%)                                                      |
| **Memory allocations**   | \~0.5 KB lower on average                                                                                   | \~3 KB higher on average                                                      |
| **Consistency**          | Performance differences are tiny and vary by sort type                                                      | New version consistently allocates more memory                                |
| **Sort impact**          | `Name` sort dominates runtime (\~18–24 ms) in both versions, far slower than `Id` or `CreatedAt` (\~6–7 ms) | Same pattern, sort choice has minimal effect on speed since queries are short |
| **Overall verdict**      | New **GetAll** is performance-neutral vs Old                                                                | New **Get** is slightly slower and heavier in allocations                     |

**In short:**

* **GetAll**: New version ≈ Old version in speed and memory.
* **Get**: New version is \~14 µs slower and uses \~3 KB more memory.
* **Sorting by Name** is the biggest cost driver, not the new vs old implementation.

I can also break this down per `SortLabel` to see if the new versions win in some cases and lose in others — that would show if there’s a pattern.
