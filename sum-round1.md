Here are the **separate tables** for `Int Sum`, `Long Sum`, and `Double Sum`, with execution times in **seconds (0.00 format)**.

---

### **Int Sum Comparison**
| **Period**         | **MongoDB Int Sum** | **PostgreSQL Int Sum** | **MongoDB Time (s)** | **PostgreSQL Time (s)** |
|--------------------|-------------------|---------------------|-----------------|-------------------|
| **Total**         | 137,140,596        | 137,203,165        | 3.46            | 18.12            |
| **Last 1 day**    | 115,929            | 116,105            | 2.99            | 7.39             |
| **Last 3 days**   | 371,547            | 364,424            | 2.96            | 7.45             |
| **Last 7 days**   | 865,779            | 865,492            | 2.95            | 7.05             |
| **Last 10 days**  | 1,239,860          | 1,242,446          | 2.93            | 7.11             |
| **Last 15 days**  | 1,874,816          | 1,880,245          | 2.90            | 7.19             |
| **Last 30 days**  | 3,748,767          | 3,763,259          | 2.97            | 7.33             |
| **Last 180 days** | 22,503,301         | 22,497,183         | 3.09            | 17.36            |
| **Last 360 days** | 45,008,966         | 45,013,252         | 3.24            | 17.76            |

---

### **Long Sum Comparison**
| **Period**         | **MongoDB Long Sum** | **PostgreSQL Long Sum** | **MongoDB Time (s)** | **PostgreSQL Time (s)** |
|--------------------|--------------------|----------------------|-----------------|-------------------|
| **Total**         | 2,799,542,601      | 2,796,831,965       | 3.46            | 18.12            |
| **Last 1 day**    | 2,394,792          | 2,332,633          | 2.99            | 7.39             |
| **Last 3 days**   | 7,565,010          | 7,401,414          | 2.96            | 7.45             |
| **Last 7 days**   | 17,631,931         | 17,621,710         | 2.95            | 7.05             |
| **Last 10 days**  | 25,297,958         | 25,304,958         | 2.93            | 7.11             |
| **Last 15 days**  | 38,141,408         | 38,328,101         | 2.90            | 7.19             |
| **Last 30 days**  | 76,393,151         | 76,534,434         | 2.97            | 7.33             |
| **Last 180 days** | 459,294,942        | 459,189,751        | 3.09            | 17.36            |
| **Last 360 days** | 918,673,661        | 918,470,932        | 3.24            | 17.76            |

---

### **Double Sum Comparison**
| **Period**         | **MongoDB Double Sum** | **PostgreSQL Double Sum** | **MongoDB Time (s)** | **PostgreSQL Time (s)** |
|--------------------|---------------------|----------------------|-----------------|-------------------|
| **Total**         | 2,798,895.99         | 2,798,417.50        | 3.46            | 18.12            |
| **Last 1 day**    | 2,371.09             | 2,401.74            | 2.99            | 7.39             |
| **Last 3 days**   | 7,609.37             | 7,461.52            | 2.96            | 7.45             |
| **Last 7 days**   | 17,653.51            | 17,690.65           | 2.95            | 7.05             |
| **Last 10 days**  | 25,325.79            | 25,376.44           | 2.93            | 7.11             |
| **Last 15 days**  | 38,194.69            | 38,293.87           | 2.90            | 7.19             |
| **Last 30 days**  | 76,419.72            | 76,596.96           | 2.97            | 7.33             |
| **Last 180 days** | 458,951.36           | 459,299.30          | 3.09            | 17.36            |
| **Last 360 days** | 918,569.79           | 918,883.93          | 3.24            | 17.76            |

---

### **Key Takeaways:**
- **MongoDB is consistently faster** than PostgreSQL, with **execution times ~2x–5x shorter**.
- **Summation values are nearly identical**, meaning both databases provide accurate results.
- **PostgreSQL slows down significantly** on larger queries, especially when summing across **180+ days**.
- **MongoDB maintains stable performance** across different periods.

Would you like any additional breakdowns or visualizations (graphs, charts)? 🚀
