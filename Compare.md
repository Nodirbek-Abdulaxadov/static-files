# Compare languages with  1billion loop
---
 by Nodirbek Abdulakhadov
---

# Fastest Group

## 1. C

```c
#include <stdio.h>
#include <time.h>
int main() {
    struct timespec start, end;
    clock_gettime(CLOCK_MONOTONIC, &start);
    for (long long i = 0; i < 1000000000LL; ++i) { }
    clock_gettime(CLOCK_MONOTONIC, &end);
    long long elapsed = (end.tv_sec - start.tv_sec) * 1e9 + (end.tv_nsec - start.tv_nsec);
    printf("Time: %lld nanoseconds\n", elapsed);
    return 0;
}
```

| **Metric**        | **Value**        |
|-------------------|------------------|
| Memory Usage      | 1.748 MB         |
| Runtime           | 0.0000 seconds   |
| Execution Time    | 391 nanoseconds  |


## 2. C++
```cpp
#include <iostream>
#include <chrono>
using namespace std;
int main() {
  auto start = chrono::high_resolution_clock::now();
  for (long long i = 0; i < 1000000000LL; ++i) { }

  auto end = chrono::high_resolution_clock::now();
  auto duration = chrono::duration_cast<chrono::nanoseconds>(end - start).count();
  cout << "Time: " << duration << " nanoseconds" << endl;
  return 0;
}
```

| **Metric**        | **Value**        |
|-------------------|------------------|
| Memory Usage      | 3.404 MB         |
| Runtime           | 0.0000 seconds   |
| Execution Time    | 311 nanoseconds  |


## 3. Rust

```rust
use std::time::Instant;
fn main() {
  let start = Instant::now();
  for _ in 0..1_000_000_000 { }

  let duration = start.elapsed().as_nanos();
  println!("Time: {} nanoseconds", duration);
}
```

| **Metric**        | **Value**        |
|-------------------|------------------|
| Memory Usage      | 2.236 MB         |
| Runtime           | 0.0000 seconds   |
| Execution Time    | 619 nanoseconds  |

# Moderate Group

## 4. C#

```csharp
using System;
using System.Diagnostics;

class Program {
  static void Main() {
    Stopwatch sw = Stopwatch.StartNew();
    for (long i = 0; i < 1_000_000_000L; i++) { }
    sw.Stop();
    Console.WriteLine("Time: {0} nanoseconds", sw.Elapsed.TotalMilliseconds * 1_000_000);
  }
}
```

| **Metric**        | **Value**            |
|-------------------|----------------------|
| Memory Usage      | 16.844 MB            |
| Runtime           | 1.2100 seconds       |
| Execution Time    | 1,173,758,100 nanoseconds |


## 5. Java

```java
public class Main {
  public static void main(String[] args) {
    long start = System.nanoTime();
    for (long i = 0; i < 1_000_000_000L; i++) { }

    long end = System.nanoTime();
    System.out.println("Time: " + (end - start) + " nanoseconds");
  }
}
```

| **Metric**        | **Value**            |
|-------------------|----------------------|
| Memory Usage      | 38.892 MB            |
| Runtime           | 0.4100 seconds       |
| Execution Time    | 736,457,436 nanoseconds |


## 6.  Go

```go
package main
import (
    "fmt"
    "time"
)

func main() {
    start := time.Now()
    for i := 0; i < 1_000_000_000; i++ { }
    duration := time.Since(start).Nanoseconds()
    fmt.Printf("Time: %d nanoseconds\n", duration)
}
```

| **Metric**        | **Value**            |
|-------------------|----------------------|
| Memory Usage      | 1.736 MB             |
| Runtime           | 0.5500 seconds       |
| Execution Time    | 549,918,761 nanoseconds |


## 7.  Kotlin

```kotlin
fun main() {
    val start = System.nanoTime()
    for (i in 0 until 1_000_000_000) { }
    val end = System.nanoTime()
    println("Time: ${end - start} nanoseconds")
}
```

| **Metric**        | **Value**            |
|-------------------|----------------------|
| Memory Usage      | 49.432 MB            |
| Runtime           | 0.1700 seconds       |
| Execution Time    | 1,636,417 nanoseconds |


# Slower Group

## 8. Python

```python
import time
start = time.perf_counter_ns()
for i in range(10**9):
  pass
end = time.perf_counter_ns()
print(f"Time: {end - start} nanoseconds")
```

| **Metric**       | **Value**               |
|-------------------|-------------------------|
| Memory Usage      | 8.22 MB                 |
| Runtime           | 4.9700 seconds          |
| Execution Time    | 31,363,746,800 nanoseconds |


## 9. JavaScript

```js
const start = process.hrtime.bigint();
for (let i = 0; i < 1e9; i++) { }
const end = process.hrtime.bigint();
console.log(`Time: ${end - start} nanoseconds`);
```

| **Metric**        | **Value**            |
|-------------------|----------------------|
| Memory Usage      | 42.672 MB            |
| Runtime           | 0.9200 seconds       |
| Execution Time    | 871,030,873 nanoseconds |