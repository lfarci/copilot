# Overview

This project, `APL2007M2Sample2`, is a .NET 6.0 application designed to control a fan and communicate with a BME280 sensor to measure temperature and humidity. The application utilizes several libraries, including `IoT.Device.Bindings`, `System.Device.Gpio`, and `Microsoft.Azure.Devices.Client`, to interact with hardware components and send data to Azure IoT Hub.

Key features:
- Controls a fan based on sensor readings.
- Measures temperature and humidity using a BME280 sensor.
- Sends sensor data to Azure IoT Hub for remote monitoring.
- Provides console output with color-coded messages for easy readability.

For more details, refer to the [README.md](README.md) file.

## Features

The `APL2007M2Sample2` project includes the following features:

1. **Fan Control**:
     - The application controls a fan based on sensor readings.
     - The fan state is managed using the [`SetFanState`](../../../../../C:/Development/Sandbox/SampleApps/APL2007M2Sample2/Program.cs) function.

2. **Temperature and Humidity Measurement**:
     - Utilizes a BME280 sensor to measure temperature and humidity.
     - The sensor is accessed via the [`s_bme280`](../../../../../C:/Development/Sandbox/SampleApps/APL2007M2Sample2/Program.cs) variable.

3. **Azure IoT Hub Integration**:
     - Sends sensor data to Azure IoT Hub for remote monitoring.
     - Uses the `Microsoft.Azure.Devices.Client` package for communication.

4. **Console Output**:
     - Provides color-coded console output for easy readability of messages.

5. **Configuration**:
     - Allows setting a desired humidity limit using the [`DesiredHumidityLimit`](../../../../../C:/Development/Sandbox/SampleApps/APL2007M2Sample2/Program.cs) variable.

## Requirements

### Functional Requirements

1. **Fan Control**
    - The application must control a fan based on sensor readings.
    - The fan state must be managed using the `SetFanState` function.

2. **Temperature and Humidity Measurement**
    - The application must measure temperature and humidity using a BME280 sensor.
    - The sensor must be accessed via the `s_bme280` variable.

3. **Azure IoT Hub Integration**
    - The application must send sensor data to Azure IoT Hub for remote monitoring.
    - The communication must be handled using the `Microsoft.Azure.Devices.Client` package.

4. **Console Output**
    - The application must provide color-coded console output for easy readability of messages.

5. **Configuration**
    - The application must allow setting a desired humidity limit using the `DesiredHumidityLimit` variable.

### Non-Functional Requirements

1. **Performance**
    - The application must respond to sensor readings and control the fan in real-time.
    - Data transmission to Azure IoT Hub must occur without significant delay.

2. **Reliability**
    - The application must handle sensor errors and communication failures gracefully.
    - The fan control logic must be robust against sensor inaccuracies.

3. **Usability**
    - The console output must be clear and color-coded for easy readability.
    - Configuration settings must be easily adjustable.

4. **Maintainability**
    - The code must be well-documented and follow standard coding practices.
    - Dependencies must be managed using NuGet packages as specified in the [project file](../../../../../C:/Development/Sandbox/SampleApps/APL2007M2Sample2/APL2007M2Sample2.csproj).

5. **Scalability**
    - The application must be designed to handle additional sensors and actuators in the future.
    - The Azure IoT Hub integration must support scaling to multiple devices.

6. **Security**
    - Data transmission to Azure IoT Hub must be secure.
    - The application must ensure that sensitive information, such as connection strings, is not exposed.# Overview

This project, `APL2007M2Sample2`, is a .NET 6.0 application designed to control a fan and communicate with a BME280 sensor to measure temperature and humidity. The application utilizes several libraries, including `IoT.Device.Bindings`, `System.Device.Gpio`, and `Microsoft.Azure.Devices.Client`, to interact with hardware components and send data to Azure IoT Hub.

Key features:
- Controls a fan based on sensor readings.
- Measures temperature and humidity using a BME280 sensor.
- Sends sensor data to Azure IoT Hub for remote monitoring.
- Provides console output with color-coded messages for easy readability.

## Project Dependencies

The project uses the following NuGet packages:

- `IoT.Device.Bindings` (Version 2.1.0)
  - **Description**: Provides device bindings for various sensors and actuators.
  - **Purpose**: Used to interact with the BME280 sensor for temperature and humidity measurements.

- `System.Device.Gpio` (Version 2.1.0)
  - **Description**: Provides APIs for interacting with GPIO pins.
  - **Purpose**: Used to control the GPIO pins for fan control and sensor communication.

- `Microsoft.Azure.Devices.Client` (Version 1.41.1)
  - **Description**: Provides APIs for communicating with Azure IoT Hub.
  - **Purpose**: Used to send sensor data to Azure IoT Hub for remote monitoring.

- `Microsoft.Azure.Devices.Shared` (Version 1.30.2)
  - **Description**: Provides shared libraries for Azure IoT Hub device and service clients.
  - **Purpose**: Used to manage device twin properties and other shared functionalities.

- `Newtonsoft.Json` (Version 13.0.2)
  - **Description**: Popular high-performance JSON framework for .NET.
  - **Purpose**: Used for serializing and deserializing JSON data, particularly for communication with Azure IoT Hub.



