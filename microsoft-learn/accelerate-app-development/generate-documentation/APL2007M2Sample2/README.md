# APL2007M2Sample2

## Overview
APL2007M2Sample2 is a .NET 6.0 sample application that demonstrates the use of various packages including IoT device bindings, GPIO control, and Azure IoT Hub integration.

## Project Structure

```
APL2007M2Sample2.csproj
APL2007M2Sample2.sln
bin/
	Debug/
		net6.0/
media/
obj/
	APL2007M2Sample2.csproj.nuget.dgspec.json
	APL2007M2Sample2.csproj.nuget.g.props
	APL2007M2Sample2.csproj.nuget.g.targets
	Debug/
		net6.0/
			.NETCoreApp,Version=v6.0.AssemblyAttributes.cs
			APL2007M2Sample2.AssemblyInfo.cs
			APL2007M2Sample2.AssemblyInfoInputs.cache
			APL2007M2Sample2.assets.cache
			...
	project.assets.json
	project.nuget.cache
Program.cs
README.md
```

## Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or any other compatible IDE

## Setup

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/APL2007M2Sample2.git
    cd APL2007M2Sample2
    ```

2. Restore the NuGet packages:
    ```sh
    dotnet restore
    ```

## Build and Run

To build and run the project, use the following commands:

```sh
dotnet build
dotnet run
```

## Project Dependencies

The project uses the following NuGet packages:

- `IoT.Device.Bindings` (Version 2.1.0)
- `System.Device.Gpio` (Version 2.1.0)
- `Microsoft.Azure.Devices.Client` (Version 1.41.1)
- `Microsoft.Azure.Devices.Shared` (Version 1.30.2)
- `Newtonsoft.Json` (Version 13.0.2)

## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any changes.

## License
This project is licensed under the MIT License. See the LICENSE file for details.