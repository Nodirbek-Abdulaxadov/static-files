Here’s the updated comparison table based on the new results:

| Period         | MongoDB Count | MongoDB Time (ms) | PostgreSQL Count | PostgreSQL Time (ms) |
|--------------|--------------|----------------|----------------|----------------|
| **Total**      | 5,600,000     | 30,206.39      | 5,600,000      | 3,668.98      |
| **Last 1 day** | 4,882         | 28,479.13      | 4,829          | 8,391.66      |
| **Last 3 days** | 15,268        | 2,606.85       | 14,974         | 15,635.77     |
| **Last 7 days** | 35,415        | 2,819.06       | 35,430         | 8,321.80      |
| **Last 10 days** | 50,744        | 2,773.99       | 50,799         | 10,908.60     |
| **Last 15 days** | 76,485        | 2,867.40       | 76,760         | 9,235.07      |
| **Last 30 days** | 152,873       | 2,824.73       | 153,339        | 9,213.78      |
| **Last 180 days** | 918,743       | 2,773.43       | 918,636        | 7,300.21      |
| **Last 360 days** | 1,837,552     | 2,732.50       | 1,838,518      | 7,512.45      |

### Key Takeaways:
1. **Total Query Performance:**  
   - PostgreSQL is significantly **faster** (3,668 ms vs. 30,206 ms).
   
2. **Short-Term Queries (1 to 30 days):**  
   - MongoDB performs better in query times **except** for the **1-day query**, which takes a massive **28,479 ms**.
   - PostgreSQL shows **slower** queries, especially for the **3-day range** (15,635 ms).

3. **Long-Term Queries (180-360 days):**  
   - PostgreSQL takes **longer** than MongoDB (7,300-7,512 ms vs. ~2,700 ms).
   - Both databases return nearly identical counts.

### Conclusion:
- If querying **recent** data (1-30 days), MongoDB has **faster response times**, except for **1-day**.
- PostgreSQL is significantly **better** at handling **total dataset queries**.
- Both databases return **similar counts** for longer periods (180-360 days).

Would you like a **visual chart** for better comparison? 🚀
