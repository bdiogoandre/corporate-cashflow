
var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", value: "admin", secret: true);
var password = builder.AddParameter("password", value: "123456", secret: true);

var postgres = builder.AddPostgres("cashflow", userName: username, password: password, port: 5432);

var cashFlowSql = postgres.AddDatabase("CashFlowSql", databaseName: "cash-flow");
var identityServerSql = postgres.AddDatabase("IdentityServerSql", databaseName: "identity-server");

var kafka = builder.AddKafka("Kafka", port: 9092)
                   .WithDataVolume(isReadOnly: false)
                   .WithKafkaUI();

builder.AddProject<Projects.Corporate_Cashflow_IdentityServer_Api>("cashflow-identityserver-api")
    .WithReference(identityServerSql)
    .WaitFor(identityServerSql);


builder.AddProject<Projects.Corporate_CashFlow_Api>("cash-flow-api")
    .WithReference(cashFlowSql)
    .WaitFor(cashFlowSql)
    .WithReference(kafka)
    .WaitFor(kafka);

builder.AddProject<Projects.Corporate_Cashflow_Worker_Consumer>("cashflow-consumer")
    .WithReference(cashFlowSql)
    .WaitFor(cashFlowSql)
    .WithReference(kafka)
    .WaitFor(kafka);


builder.Build().Run();
