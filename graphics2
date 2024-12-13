Creating a simple window with basic actions like minimize, maximize, and close, involves platform-specific window management code. Below is an example that creates a cross-platform native graphics SDK using **C++** for window creation and control, and a **C#** wrapper to interact with it. We'll use **GLFW** for cross-platform window management in C++.

---

### **Step 1: Setup Project**

1. Install **GLFW** for cross-platform window handling.
   - On macOS:
     ```bash
     brew install glfw
     ```
   - On Linux:
     ```bash
     sudo apt install libglfw3-dev
     ```
   - On Windows:
     Download and link the GLFW binaries.

2. Create a shared library (`WindowEngine`) in **C++**.

---

### **Step 2: C++ Implementation**

**CMakeLists.txt**:
```cmake
cmake_minimum_required(VERSION 3.15)
project(WindowEngine)

set(CMAKE_CXX_STANDARD 17)

# Find GLFW
find_package(glfw3 3.3 REQUIRED)
find_package(OpenGL REQUIRED)

add_library(WindowEngine SHARED src/WindowEngine.cpp)
target_include_directories(WindowEngine PUBLIC ${GLFW_INCLUDE_DIRS})
target_link_libraries(WindowEngine PUBLIC glfw ${OPENGL_LIBRARIES})
```

**WindowEngine.h**:
```cpp
#ifndef WINDOW_ENGINE_H
#define WINDOW_ENGINE_H

#ifdef _WIN32
#define EXPORT __declspec(dllexport)
#else
#define EXPORT
#endif

extern "C" {
    EXPORT void InitializeWindow();
    EXPORT void TerminateWindow();
}

#endif // WINDOW_ENGINE_H
```

**WindowEngine.cpp**:
```cpp
#include <GLFW/glfw3.h>
#include <iostream>

// Global variable for the GLFW window
GLFWwindow* window = nullptr;

// Callback for window resize
void FramebufferSizeCallback(GLFWwindow* window, int width, int height) {
    glViewport(0, 0, width, height);
}

// Initialize the window
extern "C" void InitializeWindow() {
    if (!glfwInit()) {
        std::cerr << "Failed to initialize GLFW!" << std::endl;
        return;
    }

    // Create a window
    window = glfwCreateWindow(800, 600, "Simple Window", nullptr, nullptr);
    if (!window) {
        std::cerr << "Failed to create GLFW window!" << std::endl;
        glfwTerminate();
        return;
    }

    // Make the OpenGL context current
    glfwMakeContextCurrent(window);

    // Set callbacks
    glfwSetFramebufferSizeCallback(window, FramebufferSizeCallback);

    std::cout << "Window initialized successfully!" << std::endl;

    // Event loop
    while (!glfwWindowShouldClose(window)) {
        // Poll events
        glfwPollEvents();

        // Render (empty for now)
        glClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        glClear(GL_COLOR_BUFFER_BIT);

        // Swap buffers
        glfwSwapBuffers(window);
    }
}

// Terminate the window and clean up
extern "C" void TerminateWindow() {
    if (window) {
        glfwDestroyWindow(window);
        window = nullptr;
    }
    glfwTerminate();
    std::cout << "Window terminated." << std::endl;
}
```

---

### **Step 3: C# Wrapper**

**Interop.cs**:
```csharp
using System;
using System.Runtime.InteropServices;

public static class WindowInterop
{
    [DllImport("WindowEngine.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void InitializeWindow();

    [DllImport("WindowEngine.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void TerminateWindow();
}
```

**WindowAPI.cs**:
```csharp
public class WindowAPI
{
    public void Initialize()
    {
        WindowInterop.InitializeWindow();
    }

    public void Terminate()
    {
        WindowInterop.TerminateWindow();
    }
}
```

---

### **Step 4: C# Application**

**Program.cs**:
```csharp
using System;

class Program
{
    static void Main(string[] args)
    {
        var windowAPI = new WindowAPI();

        Console.WriteLine("Initializing window...");
        windowAPI.Initialize();

        Console.WriteLine("Press Enter to terminate the window.");
        Console.ReadLine();

        Console.WriteLine("Terminating window...");
        windowAPI.Terminate();
    }
}
```

---

### **Step 5: Build Instructions**

1. **Build the C++ Library**:
   ```bash
   mkdir build
   cd build
   cmake ..
   cmake --build .
   ```
   This generates the shared library (`WindowEngine.dll` on Windows, `libWindowEngine.so` on Linux, `libWindowEngine.dylib` on macOS).

2. **Reference the Library in C#**:
   Place the shared library in the output directory of your C# project.

3. **Run the C# Application**:
   - On Windows:
     Ensure `WindowEngine.dll` is in the same directory as the executable.
   - On macOS/Linux:
     Ensure `libWindowEngine.so` or `libWindowEngine.dylib` is in the same directory as the executable or in the library path.

---

### **Actions (Minimize, Maximize, Close)**

Add functionality for actions like minimize, maximize, and close:

#### C++ Additions
**WindowEngine.cpp**:
```cpp
extern "C" void MinimizeWindow() {
    if (window) {
        glfwIconifyWindow(window);
    }
}

extern "C" void MaximizeWindow() {
    if (window) {
        glfwMaximizeWindow(window);
    }
}

extern "C" void CloseWindow() {
    if (window) {
        glfwSetWindowShouldClose(window, GLFW_TRUE);
    }
}
```

**WindowEngine.h**:
```cpp
extern "C" void MinimizeWindow();
extern "C" void MaximizeWindow();
extern "C" void CloseWindow();
```

#### C# Additions
**Interop.cs**:
```csharp
[DllImport("WindowEngine.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern void MinimizeWindow();

[DllImport("WindowEngine.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern void MaximizeWindow();

[DllImport("WindowEngine.dll", CallingConvention = CallingConvention.Cdecl)]
public static extern void CloseWindow();
```

**WindowAPI.cs**:
```csharp
public void Minimize() => WindowInterop.MinimizeWindow();
public void Maximize() => WindowInterop.MaximizeWindow();
public void Close() => WindowInterop.CloseWindow();
```

---

### **Run the Program**

1. Open the window:
   ```csharp
   windowAPI.Initialize();
   ```

2. Perform actions:
   ```csharp
   windowAPI.Minimize();
   windowAPI.Maximize();
   windowAPI.Close();
   ```

3. Cleanup:
   ```csharp
   windowAPI.Terminate();
   ```

---

### **Next Steps**

1. **Add Input Handling**:
   Capture mouse and keyboard inputs.
2. **Rendering**:
   Extend the library to include rendering capabilities (shapes, textures, shaders).
3. **Cross-Platform Deployment**:
   Package libraries for Windows, macOS, and Linux using NuGet or similar tools.

This example creates a window with minimize, maximize, and close functionality and provides an extensible framework for future enhancements. Let me know if you need further assistance!
