﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ParallelAsyncExample
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Represents the main window of the application.
        /// </summary>
        public partial class MainWindow : Window
        {
            private readonly HttpClient _client = new HttpClient { MaxResponseContentBufferSize = 1_000_000 };

            private readonly IEnumerable<string> _urlList = new string[]
            {
                "https://docs.microsoft.com",
                "https://docs.microsoft.com/azure",
                "https://docs.microsoft.com/powershell",
                "https://docs.microsoft.com/dotnet",
                "https://docs.microsoft.com/aspnet/core",
                "https://docs.microsoft.com/windows",
                "https://docs.microsoft.com/office",
                "https://docs.microsoft.com/enterprise-mobility-security",
                "https://docs.microsoft.com/visualstudio",
                "https://docs.microsoft.com/microsoft-365",
                "https://docs.microsoft.com/sql",
                "https://docs.microsoft.com/dynamics365",
                "https://docs.microsoft.com/surface",
                "https://docs.microsoft.com/xamarin",
                "https://docs.microsoft.com/azure/devops",
                "https://docs.microsoft.com/system-center",
                "https://docs.microsoft.com/graph",
                "https://docs.microsoft.com/education",
                "https://docs.microsoft.com/gaming"
            };

            /// <summary>
            /// Event handler for the Start button click event.
            /// </summary>
            private void OnStartButtonClick(object sender, RoutedEventArgs e)
            {
                _startButton.IsEnabled = false;
                _resultsTextBox.Clear();

                Task.Run(() => StartSumPageSizesAsync());
            }

            /// <summary>
            /// Starts the asynchronous process of summing the page sizes.
            /// </summary>
            private async Task StartSumPageSizesAsync()
            {
                await SumPageSizesAsync();
                await Dispatcher.BeginInvoke(() =>
                {
                    _resultsTextBox.Text += $"\nControl returned to {nameof(OnStartButtonClick)}.";
                    _startButton.IsEnabled = true;
                });
            }

            /// <summary>
            /// Asynchronously sums the sizes of the pages.
            /// </summary>
            private async Task SumPageSizesAsync()
            {
                var stopwatch = Stopwatch.StartNew();

                IEnumerable<Task<int>> downloadTasksQuery =
                    from url in _urlList
                    select ProcessUrlAsync(url, _client);

                Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

                int[] lengths = await Task.WhenAll(downloadTasks);
                int total = lengths.Sum();

                await Dispatcher.BeginInvoke(() =>
                {
                    stopwatch.Stop();

                    _resultsTextBox.Text += $"\nTotal bytes returned:  {total:#,#}";
                    _resultsTextBox.Text += $"\nElapsed time:          {stopwatch.Elapsed}\n";
                });
            }

            /// <summary>
            /// Asynchronously processes the specified URL and returns the size of the downloaded content.
            /// </summary>
            /// <param name="url">The URL to process.</param>
            /// <param name="client">The HttpClient instance to use for downloading.</param>
            /// <returns>The size of the downloaded content in bytes.</returns>
            private async Task<int> ProcessUrlAsync(string url, HttpClient client)
            {
                try
                {
                    byte[] byteArray = await client.GetByteArrayAsync(url);
                    await DisplayResultsAsync(url, byteArray);

                    return byteArray.Length;
                }
                catch (Exception ex)
                {
                    // Handle the exception here, for example, display an error message.
                    await Dispatcher.BeginInvoke(() =>
                    {
                        _resultsTextBox.Text += $"Error downloading {url}: {ex.Message}\n";
                    });

                    return 0; // Return 0 or any other default value to indicate failure.
                }
            }

            /// <summary>
            /// Asynchronously displays the results of the URL processing.
            /// </summary>
            /// <param name="url">The URL that was processed.</param>
            /// <param name="content">The downloaded content.</param>
            /// <returns>A Task representing the asynchronous operation.</returns>
            private Task DisplayResultsAsync(string url, byte[] content) =>
                Dispatcher.BeginInvoke(() =>
                    _resultsTextBox.Text += $"{url,-60} {content.Length,10:#,#}\n")
                          .Task;

            /// <summary>
            /// Overrides the OnClosed method to dispose the HttpClient instance.
            /// </summary>
            /// <param name="e">The event arguments.</param>
            protected override void OnClosed(EventArgs e) => _client.Dispose();
        }
    }
}
