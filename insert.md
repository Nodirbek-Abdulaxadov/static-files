Here's a benchmark table comparing MongoDB and PostgreSQL for inserting 1,000,000 models:  

| Batch Range       | MongoDB (ms) | PostgreSQL (s) |
|------------------|-------------|---------------|
| 0 - 100,000     | 3.9418      | 11.3625       |
| 100,000 - 200,000 | 3.2945    | 9.8015        |
| 200,000 - 300,000 | 3.3045    | 10.0198       |
| 300,000 - 400,000 | 3.3716    | 9.9621        |
| 400,000 - 500,000 | 3.2370    | 9.9019        |
| 500,000 - 600,000 | 3.6272    | 10.0431       |
| 600,000 - 700,000 | 3.8087    | 9.9133        |
| 700,000 - 800,000 | 3.3814    | 9.9895        |
| 800,000 - 900,000 | 3.3168    | 10.2163       |
| 900,000 - 1,000,000 | 3.4210  | 9.8281        |
| **Total (1M rows)** | **34.7059 s** | **101.0397 s** |

MongoDB is significantly faster, inserting all models in **34.7 seconds**, while PostgreSQL takes **101.0 seconds**. 🚀
