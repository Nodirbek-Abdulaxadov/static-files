Here's your **comparable table** with execution times converted to **seconds (0.00 format)**:

| **Period**         | **MongoDB Int Sum** | **PostgreSQL Int Sum** | **MongoDB Long Sum** | **PostgreSQL Long Sum** | **MongoDB Double Sum** | **PostgreSQL Double Sum** | **MongoDB Time (s)** | **PostgreSQL Time (s)** |
|--------------------|-------------------|---------------------|-------------------|---------------------|-------------------|---------------------|-----------------|-------------------|
| **Total**         | 137,140,596        | 137,203,165        | 2,799,542,601     | 2,796,831,965      | 2,798,895.99      | 2,798,417.50       | 3.46            | 18.12            |
| **Last 1 day**    | 115,929            | 116,105            | 2,394,792         | 2,332,633         | 2,371.09         | 2,401.74          | 2.99            | 7.39             |
| **Last 3 days**   | 371,547            | 364,424            | 7,565,010         | 7,401,414         | 7,609.37         | 7,461.52          | 2.96            | 7.45             |
| **Last 7 days**   | 865,779            | 865,492            | 17,631,931        | 17,621,710        | 17,653.51        | 17,690.65         | 2.95            | 7.05             |
| **Last 10 days**  | 1,239,860          | 1,242,446          | 25,297,958        | 25,304,958        | 25,325.79        | 25,376.44         | 2.93            | 7.11             |
| **Last 15 days**  | 1,874,816          | 1,880,245          | 38,141,408        | 38,328,101        | 38,194.69        | 38,293.87         | 2.90            | 7.19             |
| **Last 30 days**  | 3,748,767          | 3,763,259          | 76,393,151        | 76,534,434        | 76,419.72        | 76,596.96         | 2.97            | 7.33             |
| **Last 180 days** | 22,503,301         | 22,497,183         | 459,294,942       | 459,189,751       | 458,951.36       | 459,299.30        | 3.09            | 17.36            |
| **Last 360 days** | 45,008,966         | 45,013,252         | 918,673,661       | 918,470,932       | 918,569.79       | 918,883.93        | 3.24            | 17.76            |

### **Observations:**
- **MongoDB is significantly faster** for total sums, executing in ~3.46s vs. PostgreSQL's ~18.12s.
- PostgreSQL takes **~2x–5x longer** in most cases, especially for long-range queries (180+ days).
- For smaller queries (1–30 days), PostgreSQL is **still slower** but the difference is less pronounced.
- Summation results are **nearly identical** across databases, meaning accuracy is maintained.

Would you like any further breakdown or optimizations? 🚀
