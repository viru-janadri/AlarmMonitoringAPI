using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
	private const string ALL_ORIGINS_POLICY = "_allOrigins";
	private static readonly TimeSpan KEEP_ALIVE_INTERVAL = TimeSpan.FromMilliseconds(10000); // 10-second keep-alive

	public IConfiguration Configuration { get; }

	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;

	}

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddLogging();
		// Add CORS policy to allow all origins, headers, and methods
		services.AddCors(options =>
		{
			options.AddPolicy(ALL_ORIGINS_POLICY, builder =>
			{
				builder.AllowAnyOrigin()
					   .AllowAnyHeader()
					   .AllowAnyMethod();
			});
		});

		// Add WebSocket with keep-alive interval
		services.AddWebSockets(options =>
		{
			options.KeepAliveInterval = KEEP_ALIVE_INTERVAL;

			//By default this allows all Origins

		});

		// Add your connection string
		//var connectionString = "Host=ae11b406836524e51a65db98d46c52f8-557629353.us-east-1.elb.amazonaws.com;Port=5433;Username=yugabyte;Database=yugabyte;";
		var connectionString = "Host=ae11b406836524e51a65db98d46c52f8-557629353.us-east-1.elb.amazonaws.com;Port=5433;Username=yugabyte;Database=yugabyte;";

		// Register necessary services
		services.AddSingleton(new AlarmRepository(connectionString));

		// Register the AlarmMonitor and inject the event sender
		services.AddSingleton<AlarmMonitor>(sp =>
		{
			var alarmRepository = sp.GetRequiredService<AlarmRepository>();
			var eventSender = sp.GetRequiredService<ITopicEventSender>();
			return new AlarmMonitor(alarmRepository, eventSender);
		});

		services.AddSingleton<AlarmSubscriptions>();

		// Set up the GraphQL server with subscriptions
		services.AddGraphQLServer()
			.AddType<Alarm>()
			.AddQueryType<Query>()
			.AddSubscriptionType<AlarmSubscriptions>()
			.AddInMemorySubscriptions();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		app.UseRouting();

		// Enable CORS with the all-origins policy
		app.UseCors(ALL_ORIGINS_POLICY);

		// Enable WebSockets
		app.UseWebSockets();

		// Set up GraphQL endpoint
		app.UseEndpoints(endpoints =>
		{
			endpoints.MapGraphQL();  // Map GraphQL to the root endpoint
		});

	}
}
