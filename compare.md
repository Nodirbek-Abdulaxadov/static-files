I'll generate a simple CRUD API for **ASP.NET Core Web API**, **FastAPI (Python)**, and **Gin (Go)**, so you can compare their syntax and structure. Each API will handle a **"Product"** entity with a basic filter example (`GET /products?name=xyz`).  

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

## **🔍 Comparison Summary**
| Framework | Language | Performance | Syntax Complexity |
|-----------|----------|-------------|-------------------|
| ASP.NET Core | C# | High | Medium |
| FastAPI | Python | Fast | Simple |
| Gin | Go | Very Fast | Minimal |

Let me know if you need further improvements! 🚀
