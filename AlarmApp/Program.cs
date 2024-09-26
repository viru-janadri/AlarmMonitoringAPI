using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
	public static void Main(string[] args)
	{
		var host = CreateHostBuilder(args).Build();

		// Get the AlarmMonitor service and start it
		var monitor = host.Services.GetRequiredService<AlarmMonitor>();
		//var cancellationTokenSource = new System.Threading.CancellationTokenSource();

		// Start monitoring the database for alarms
		_ = monitor.StartMonitoringAsync();

		// Start the host (GraphQL server)
		host.Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();  // Delegate to Startup class
				webBuilder.UseUrls("http://localhost:4000");  // Set the URL and port
			});

}
