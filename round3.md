Here’s an updated comparison table including **PostgreSQL (ADO.NET) results** alongside **MongoDB** and **PostgreSQL (standard query results).**  

---

### **Count Comparison**
| **Period         _**         | **MongoDB Count** | **MongoDB Time (s)** | **PostgreSQL Count** | **PostgreSQL Time (s)** | **PostgreSQL (ADO.NET) Count** | **PostgreSQL (ADO.NET) Time (s)** |
|--------------------|-----------------|----------------|-----------------|----------------|----------------------|----------------------|
| **Total**         | 5,600,000        | 1.88           | 5,600,000       | 18.12          | 5,600,000            | **0.63**             |
| **Last 1 day**    | 4,866            | 2.52           | 4,814           | 7.39           | 4,499                | **5.74**             |
| **Last 3 days**   | 15,250           | 2.75           | 14,953          | 7.45           | 14,645               | **1.89**             |
| **Last 7 days**   | 35,395           | 2.64           | 35,409          | 7.05           | 35,111               | **1.87**             |
| **Last 10 days**  | 50,725           | 2.65           | 50,776          | 7.11           | 50,426               | **1.77**             |
| **Last 15 days**  | 76,475           | 2.64           | 76,730          | 7.19           | 76,415               | **1.75**             |
| **Last 30 days**  | 152,860          | 2.58           | 153,314         | 7.33           | 152,994              | **1.75**             |
| **Last 180 days** | 918,720          | 2.82           | 918,617         | 17.36          | 918,315              | **1.80**             |
| **Last 360 days** | 1,837,532        | 2.72           | 1,838,494       | 17.76          | 1,838,205            | **1.79**             |

---

### **Key Insights**
- **MongoDB consistently outperforms standard PostgreSQL queries**, with execution times at least **2-6x faster**.
- **PostgreSQL (ADO.NET) is significantly faster than standard PostgreSQL queries**, reducing execution time by **up to 90%**.
- **PostgreSQL (ADO.NET) is now the fastest approach**, even beating MongoDB in most cases, particularly on **large datasets**.
- **For total records (5.6M), ADO.NET processed the query in just 0.63s**, compared to **1.88s for MongoDB** and **18.12s for standard PostgreSQL**.

Would you like further breakdowns or visualizations? 🚀
