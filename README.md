# Simulated Medical Device – C# .NET

This project is a simple simulation of a medical device communicating over a serial port, developed in C# and .NET. It serves as a **practical learning project** to explore hardware-near programming.

## 🧠 Purpose

I created this project primarily **to learn and deepen my understanding** of:

- Serial communication in .NET
- Object-oriented programming principles
- Medical device simulation and calibration logic
- Software development practices used in regulated environments

## ⚙️ Features

- Simulated CT calibration device implementing `IMedicalDevice` interface
- Supports commands:
  - `GET_TEMP`
  - `CALIBRATE`
  - `GET_STATUS`
  - `GET_VERSION`
  - `RESET`
- Console application (client) communicating over serial port (e.g., COM10 → COM11)
- Dynamically loads device logic via DLL and Reflection
- Clean and extendable C# code

## 🔧 Technologies Used

- **.NET 9 / C#**
- `System.IO.Ports` for serial communication
- `System.Reflection` for plugin loading
- Console application for interaction and debugging

> You need a virtual serial port pair (e.g., using [com0com](https://sourceforge.net/projects/com0com/)).


   
