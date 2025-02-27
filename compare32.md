I'll generate a simple CRUD API for **ASP.NET Core Web API**, **FastAPI (Python)**, **Gin (Go)**, and Axum (Rust) so you can compare their syntax and structure. Each API will handle a **"Product"** entity with a basic filter example (`GET /products?name=xyz`).  

---

## **1️⃣ ASP.NET Core Web API (C#)**
Using **.NET 7/8** with **Entity Framework Core**.

### **Install Dependencies**
```sh
dotnet new webapi -n AspNetCoreAPI
cd AspNetCoreAPI
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
```

### **Setup the Model (`Models/Product.cs`)**
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
```

### **Setup Database Context (`Data/AppDbContext.cs`)**
```csharp
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
```

### **Create Controller (`Controllers/ProductController.cs`)**
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/products")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] string? name)
    {
        var query = _context.Products.AsQueryable();
        if (!string.IsNullOrEmpty(name))
            query = query.Where(p => p.Name.Contains(name));

        return Ok(await query.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        return product == null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        _context.Entry(product).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return NotFound();
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
```

### **Configure Services (`Program.cs`)**
```csharp
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt => opt.UseSqlite("Data Source=products.db"));
var app = builder.Build();
app.MapControllers();
app.Run();
```

---

## **2️⃣ FastAPI (Python)**
Using **FastAPI** with **SQLAlchemy**.

### **Install Dependencies**
```sh
pip install fastapi[all] sqlalchemy sqlite3
```

### **Setup Database & Model (`database.py`)**
```python
from sqlalchemy import create_engine, Column, Integer, String, Float
from sqlalchemy.ext.declarative import declarative_base
from sqlalchemy.orm import sessionmaker

DATABASE_URL = "sqlite:///./products.db"
engine = create_engine(DATABASE_URL, connect_args={"check_same_thread": False})
SessionLocal = sessionmaker(bind=engine, autoflush=False)
Base = declarative_base()

class Product(Base):
    __tablename__ = "products"
    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    price = Column(Float)

Base.metadata.create_all(bind=engine)
```

### **Create API (`main.py`)**
```python
from fastapi import FastAPI, Depends, HTTPException
from sqlalchemy.orm import Session
from database import SessionLocal, Product

app = FastAPI()

def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

@app.get("/products")
def get_products(name: str = None, db: Session = Depends(get_db)):
    query = db.query(Product)
    if name:
        query = query.filter(Product.name.contains(name))
    return query.all()

@app.get("/products/{id}")
def get_product(id: int, db: Session = Depends(get_db)):
    product = db.query(Product).filter(Product.id == id).first()
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")
    return product

@app.post("/products")
def create_product(product: Product, db: Session = Depends(get_db)):
    db.add(product)
    db.commit()
    db.refresh(product)
    return product

@app.put("/products/{id}")
def update_product(id: int, product: Product, db: Session = Depends(get_db)):
    existing = db.query(Product).filter(Product.id == id).first()
    if not existing:
        raise HTTPException(status_code=404, detail="Product not found")
    existing.name = product.name
    existing.price = product.price
    db.commit()
    return existing

@app.delete("/products/{id}")
def delete_product(id: int, db: Session = Depends(get_db)):
    product = db.query(Product).filter(Product.id == id).first()
    if not product:
        raise HTTPException(status_code=404, detail="Product not found")
    db.delete(product)
    db.commit()
    return {"message": "Deleted successfully"}
```

---

## **3️⃣ Gin (Go)**
Using **Gin** and **GORM** for database operations.

### **Install Dependencies**
```sh
go get -u github.com/gin-gonic/gin gorm.io/gorm gorm.io/driver/sqlite
```

### **Setup Database & Model (`database.go`)**
```go
package main

import (
	"gorm.io/driver/sqlite"
	"gorm.io/gorm"
)

var DB *gorm.DB

type Product struct {
	ID    uint   `json:"id" gorm:"primaryKey"`
	Name  string `json:"name"`
	Price float64 `json:"price"`
}

func ConnectDatabase() {
	database, _ := gorm.Open(sqlite.Open("products.db"), &gorm.Config{})
	database.AutoMigrate(&Product{})
	DB = database
}
```

### **Create API (`main.go`)**
```go
package main

import (
	"github.com/gin-gonic/gin"
	"net/http"
)

func main() {
	r := gin.Default()
	ConnectDatabase()

	r.GET("/products", func(c *gin.Context) {
		var products []Product
		name := c.Query("name")
		query := DB
		if name != "" {
			query = query.Where("name LIKE ?", "%"+name+"%")
		}
		query.Find(&products)
		c.JSON(http.StatusOK, products)
	})

	r.GET("/products/:id", func(c *gin.Context) {
		var product Product
		if err := DB.First(&product, c.Param("id")).Error; err != nil {
			c.JSON(http.StatusNotFound, gin.H{"message": "Product not found"})
			return
		}
		c.JSON(http.StatusOK, product)
	})

	r.POST("/products", func(c *gin.Context) {
		var product Product
		if err := c.ShouldBindJSON(&product); err != nil {
			c.JSON(http.StatusBadRequest, gin.H{"error": err.Error()})
			return
		}
		DB.Create(&product)
		c.JSON(http.StatusCreated, product)
	})

	r.DELETE("/products/:id", func(c *gin.Context) {
		DB.Delete(&Product{}, c.Param("id"))
		c.JSON(http.StatusOK, gin.H{"message": "Deleted successfully"})
	})

	r.Run(":8080")
}
```

---

### **4️⃣ Axum (Rust)**
For Rust, **Axum** (from the Tokio project) is a great choice for building web APIs. It’s built on **Hyper** and **Tokio**, making it **fast and async-first**.

---

### **🔹 Install Dependencies**
Create a new Rust project and add dependencies:

```sh
cargo new rust_axum_api
cd rust_axum_api
```

Edit `Cargo.toml`:

```toml
[dependencies]
tokio = { version = "1", features = ["full"] }
axum = "0.7"
serde = { version = "1", features = ["derive"] }
serde_json = "1"
sqlx = { version = "0.7", features = ["sqlite", "runtime-tokio"] }
uuid = { version = "1", features = ["v4", "serde"] }
```

---

### **🔹 Database & Model (`models.rs`)**
```rust
use serde::{Deserialize, Serialize};
use sqlx::FromRow;
use uuid::Uuid;

#[derive(Debug, Serialize, Deserialize, FromRow)]
pub struct Product {
    pub id: Uuid,
    pub name: String,
    pub price: f64,
}
```

---

### **🔹 Database Connection (`db.rs`)**
```rust
use sqlx::{Pool, Sqlite};
use crate::models::Product;
use uuid::Uuid;

pub async fn create_product(db: &Pool<Sqlite>, name: &str, price: f64) -> Result<Product, sqlx::Error> {
    let id = Uuid::new_v4();
    sqlx::query("INSERT INTO products (id, name, price) VALUES (?, ?, ?)")
        .bind(id.to_string())
        .bind(name)
        .bind(price)
        .execute(db)
        .await?;
    
    Ok(Product { id, name: name.to_string(), price })
}

pub async fn get_products(db: &Pool<Sqlite>, name_filter: Option<String>) -> Result<Vec<Product>, sqlx::Error> {
    let products = if let Some(name) = name_filter {
        sqlx::query_as::<_, Product>("SELECT * FROM products WHERE name LIKE ?")
            .bind(format!("%{}%", name))
            .fetch_all(db)
            .await?
    } else {
        sqlx::query_as::<_, Product>("SELECT * FROM products")
            .fetch_all(db)
            .await?
    };

    Ok(products)
}
```

---

### **🔹 API Handlers (`handlers.rs`)**
```rust
use axum::{extract::{Path, Query, State}, Json};
use sqlx::SqlitePool;
use std::sync::Arc;
use crate::{db, models::Product};
use serde::Deserialize;

#[derive(Deserialize)]
pub struct ProductQuery {
    name: Option<String>,
}

pub async fn list_products(
    State(db): State<Arc<SqlitePool>>,
    Query(query): Query<ProductQuery>,
) -> Json<Vec<Product>> {
    let products = db::get_products(&db, query.name).await.unwrap();
    Json(products)
}

pub async fn create_product(
    State(db): State<Arc<SqlitePool>>,
    Json(product): Json<Product>,
) -> Json<Product> {
    let new_product = db::create_product(&db, &product.name, product.price).await.unwrap();
    Json(new_product)
}
```

---

### **🔹 Main Server (`main.rs`)**
```rust
mod db;
mod models;
mod handlers;

use axum::{routing::get, routing::post, Router};
use sqlx::SqlitePool;
use std::sync::Arc;
use tokio::net::TcpListener;

#[tokio::main]
async fn main() {
    let db = SqlitePool::connect("sqlite://products.db").await.unwrap();
    sqlx::migrate!().run(&db).await.unwrap();
    let db = Arc::new(db);

    let app = Router::new()
        .route("/products", get(handlers::list_products))
        .route("/products", post(handlers::create_product))
        .with_state(db.clone());

    let listener = TcpListener::bind("127.0.0.1:3000").await.unwrap();
    axum::serve(listener, app).await.unwrap();
}
```

---

### **🔹 Running the Rust API**
```sh
cargo run
```

Now, you can make requests:

- **List Products**:  
  `GET /products`
- **Create Product**:  
  `POST /products` with JSON `{ "name": "Laptop", "price": 1200.00 }`
- **Filter by Name**:  
  `GET /products?name=laptop`

---

### **🔍 Comparison Summary**
| Framework  | Language | Performance | Syntax Complexity |
|------------|----------|-------------|-------------------|
| ASP.NET Core | C# | High | Medium |
| FastAPI | Python | Fast | Simple |
| Gin | Go | Very Fast | Minimal |
| Axum | Rust | **Blazing Fast** | Medium-High |

Rust with Axum provides a **super-fast**, **memory-safe**, and **async-first** API but has a **steeper learning curve** than Python or Go.  

Let me know if you need further improvements! 🚀

### **🔍 Updated Comparison Summary with Scalability (100k+ req/s)**  

| Framework    | Language | Performance | Syntax Complexity | Scalability (100k+ req/s) |
|-------------|----------|-------------|-------------------|--------------------------|
| **ASP.NET Core** | C# | High | Medium | ✅ Possible with Kestrel + Load Balancing |
| **FastAPI** | Python | Fast | Simple | ❌ Limited (GIL & Uvicorn) |
| **Gin** | Go | Very Fast | Minimal | ✅ Yes (High Concurrency & Goroutines) |
| **Axum** | Rust | 🚀 Blazing Fast | Medium-High | ✅ Yes (Async + Tokio + Hyper) |

### **🔹 Explanation**  
- **ASP.NET Core**: Can handle **100k+ req/s** with **Kestrel + Nginx/Load Balancers** but requires tuning.  
- **FastAPI**: Uvicorn struggles beyond **50k req/s** due to Python’s **Global Interpreter Lock (GIL)**.  
- **Gin (Go)**: **Built for high concurrency**, handles 100k+ req/s easily.  
- **Axum (Rust)**: **Tokio + Hyper** provides **high scalability & low latency**—ideal for ultra-high-performance APIs.  

If you're targeting **100k+ requests per second**, **Rust (Axum) or Go (Gin) are the best choices**. 🚀
