Here's the updated comparison table based on your latest results:  

| Period         | MongoDB Count | MongoDB Time (ms) | PostgreSQL Count | PostgreSQL Time (ms) |
|--------------|--------------|----------------|----------------|----------------|
| **Total**      | 5,600,000     | 1,880.38       | 5,600,000      | 1,806.01      |
| **Last 1 day** | 4,866         | 2,521.77       | 4,814          | 7,277.83      |
| **Last 3 days** | 15,250        | 2,748.16       | 14,953         | 7,572.63      |
| **Last 7 days** | 35,395        | 2,643.96       | 35,409         | 7,421.71      |
| **Last 10 days** | 50,725        | 2,645.65       | 50,776         | 7,061.56      |
| **Last 15 days** | 76,475        | 2,638.24       | 76,730         | 7,016.53      |
| **Last 30 days** | 152,860       | 2,580.41       | 153,314        | 6,000.30      |
| **Last 180 days** | 918,720       | 2,824.30       | 918,617        | 5,281.46      |
| **Last 360 days** | 1,837,532     | 2,722.81       | 1,838,494      | 5,757.34      |

### Key Observations:
1. **Total Query Performance:**  
   - PostgreSQL is **slightly faster** (1,806 ms vs. 1,880 ms).  

2. **Short-Term Queries (1 to 30 days):**  
   - **MongoDB is consistently faster**, taking around **2,500-2,700 ms**.  
   - PostgreSQL is **significantly slower** for shorter time frames (**7,000+ ms for most queries**).  

3. **Long-Term Queries (180-360 days):**  
   - MongoDB remains **stable (~2,700 ms)** across large periods.  
   - PostgreSQL **performs better for long-term queries** than short-term but is still **almost twice as slow as MongoDB** (5,200-5,700 ms).  

### Conclusion:
- **MongoDB dominates in short-term and long-term queries** with more consistent performance.  
- **PostgreSQL struggles with short-term queries** but is **competitive for full dataset counts**.  

Would you like a **graph/chart** to visualize this better? 🚀
