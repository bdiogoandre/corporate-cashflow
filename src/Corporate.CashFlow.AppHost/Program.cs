var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", value: "admin", secret: true);
var password = builder.AddParameter("password", value: "123456", secret: true);

var postgres = builder.AddPostgres("Postgres", userName: username, password: password, port: 5432);

var rabbitmq = builder.AddRabbitMQ("RabbitMQ")
    .WithArgs(
        "sh",
        "-c",
        "rabbitmq-plugins enable --offline rabbitmq_stream && rabbitmq-server"
    )
    .WithEndpoint(targetPort: 5552, name: "streams")
    .WithEnvironment("RABBITMQ_SERVER_ADDITIONAL_ERL_ARGS", "-rabbitmq_stream advertised_host localhost")
    .WithEnvironment("RABBITMQ_STREAM_LOG_RETENTION_SIZE", "100000000")
    .WithEnvironment("RABBITMQ_STREAM_MAX_SEGMENT_SIZE_BYTES", "10000000");



builder.Build().Run();
