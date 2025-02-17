﻿using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.Bmxx80;
using Iot.Device.Bmxx80.ReadResult;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System.Text;

namespace CheeseCaveDotnet;

/// <summary>
/// Represents a device that controls a fan and communicates with a BME280 sensor to measure temperature and humidity.
/// </summary>
class Device
{
    /// <summary>
    /// The pin number for controlling the fan.
    /// </summary>
    private static readonly int s_pin = 21;

    /// <summary>
    /// The GPIO controller for interacting with the GPIO pins.
    /// </summary>
    private static GpioController s_gpio;

    /// <summary>
    /// The I2C device for communicating with the BME280 sensor.
    /// </summary>
    private static I2cDevice s_i2cDevice;

    /// <summary>
    /// The BME280 sensor for measuring temperature and humidity.
    /// </summary>
    private static Bme280 s_bme280;

    /// <summary>
    /// The acceptable range above or below the desired temperature, in degrees Fahrenheit.
    /// </summary>
    const double DesiredTempLimit = 5;

    /// <summary>
    /// The acceptable range above or below the desired humidity, in percentages.
    /// </summary>
    const double DesiredHumidityLimit = 10;

    /// <summary>
    /// The interval at which telemetry is sent to the cloud, in milliseconds.
    /// </summary>
    const int IntervalInMilliseconds = 5000;

    /// <summary>
    /// The Azure IoT Hub device client for connecting to the IoT Hub.
    /// </summary>
    private static DeviceClient s_deviceClient;

    /// <summary>
    /// The state of the fan.
    /// </summary>
    private static stateEnum s_fanState = stateEnum.off;

    /// <summary>
    /// The connection string for the Azure IoT Hub device.
    /// </summary>
    private static readonly string s_deviceConnectionString = "YOUR DEVICE CONNECTION STRING HERE";

    /// <summary>
    /// The possible states of the fan.
    /// </summary>
    enum stateEnum
    {
        off,
        on,
        failed
    }

    /// <summary>
    /// The entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    private static void Main(string[] args)
    {
        s_gpio = new GpioController();
        s_gpio.OpenPin(s_pin, PinMode.Output);

        var i2cSettings = new I2cConnectionSettings(1, Bme280.DefaultI2cAddress);
        s_i2cDevice = I2cDevice.Create(i2cSettings);

        s_bme280 = new Bme280(s_i2cDevice);

        ColorMessage("Cheese Cave device app.\n", ConsoleColor.Yellow);

        s_deviceClient = DeviceClient.CreateFromConnectionString(s_deviceConnectionString, TransportType.Mqtt);

        s_deviceClient.SetMethodHandlerAsync("SetFanState", SetFanState, null).Wait();

        MonitorConditionsAndUpdateTwinAsync();

        Console.ReadLine();
        s_gpio.ClosePin(s_pin);
    }

    /// <summary>
    /// Monitors the conditions (temperature and humidity) and updates the device twin in the Azure IoT Hub.
    /// </summary>
    private static async void MonitorConditionsAndUpdateTwinAsync()
    {
        while (true)
        {
            Bme280ReadResult sensorOutput = s_bme280.Read();

            await UpdateTwin(
                    sensorOutput.Temperature.Value.DegreesFahrenheit,
                    sensorOutput.Humidity.Value.Percent);

            await Task.Delay(IntervalInMilliseconds);
        }
    }

    /// <summary>
    /// Sets the state of the fan based on the method request.
    /// </summary>
    /// <param name="methodRequest">The method request.</param>
    /// <param name="userContext">The user context.</param>
    /// <returns>The method response.</returns>
    private static Task<MethodResponse> SetFanState(MethodRequest methodRequest, object userContext)
    {
        if (s_fanState is stateEnum.failed)
        {
            string result = "{\"result\":\"Fan failed\"}";
            RedMessage("Direct method failed: " + result);
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
        }
        else
        {
            try
            {
                var data = Encoding.UTF8.GetString(methodRequest.Data);

                data = data.Replace("\"", "");

                s_fanState = (stateEnum)Enum.Parse(typeof(stateEnum), data);
                GreenMessage("Fan set to: " + data);

                s_gpio.Write(s_pin, s_fanState == stateEnum.on ? PinValue.High : PinValue.Low);

                string result = "{\"result\":\"Executed direct method: " + methodRequest.Name + "\"}";
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 200));
            }
            catch
            {
                string result = "{\"result\":\"Invalid parameter\"}";
                RedMessage("Direct method failed: " + result);
                return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(result), 400));
            }
        }
    }

    /// <summary>
    /// Updates the device twin in the Azure IoT Hub with the current temperature and humidity.
    /// </summary>
    /// <param name="currentTemperature">The current temperature.</param>
    /// <param name="currentHumidity">The current humidity.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private static async Task UpdateTwin(double currentTemperature, double currentHumidity)
    {
        var reportedProperties = new TwinCollection();
        reportedProperties["fanstate"] = s_fanState.ToString();
        reportedProperties["humidity"] = Math.Round(currentHumidity, 2);
        reportedProperties["temperature"] = Math.Round(currentTemperature, 2);
        await s_deviceClient.UpdateReportedPropertiesAsync(reportedProperties);

        GreenMessage("Twin state reported: " + reportedProperties.ToJson());
    }

    /// <summary>
    /// Writes a colored message to the console.
    /// </summary>
    /// <param name="text">The text of the message.</param>
    /// <param name="clr">The color of the message.</param>
    private static void ColorMessage(string text, ConsoleColor clr)
    {
        Console.ForegroundColor = clr;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    /// <summary>
    /// Writes a green message to the console.
    /// </summary>
    /// <param name="text">The text of the message.</param>
    private static void GreenMessage(string text) =>
        ColorMessage(text, ConsoleColor.Green);

    /// <summary>
    /// Writes a red message to the console.
    /// </summary>
    /// <param name="text">The text of the message.</param>
    private static void RedMessage(string text) =>
        ColorMessage(text, ConsoleColor.Red);
}
