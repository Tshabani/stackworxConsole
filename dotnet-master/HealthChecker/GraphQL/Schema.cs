using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using HealthChecker.Service;

namespace HealthChecker.GraphQL
{

    public class Server
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string HealthCheckUri { get; set; }
        public ErrorObject Error { get; set; }
    }

    public class ErrorObject
    {
        public string status { get; set; }
        public string body { get; set; }
    }

    public class ServerType : ObjectGraphType<Server>
    {
        private IHealthCheckService _service;
        public ServerType(IHealthCheckService service)
        {
            _service = service;
            Name = "Server";
            Description = "A server to monitor";

            Field(h => h.Id);
            Field(h => h.Name);
            Field(h => h.HealthCheckUri);
            Field<StringGraphType>(
                "status",
                // TODO: replace with health check code
                resolve: context => _service.GetStatusAsync().Result.status
            );
            Field<StringGraphType>(
                "lastTimeUp",
                // TODO: replace with health check code
                resolve: context => new DateTime((long)_service.GetStatusAsync().Result.timestamp)
            );

        }
    }

    public class HealthCheckerQuery : ObjectGraphType<object>
    {
        private List<Server> servers = new List<Server>{
            new Server{
                Id = "1",
                Name = "stackworx.io",
                HealthCheckUri = "https://www.stackworx.io",
            },
            new Server{
                Id = "2",
                Name = "prima.run",
                HealthCheckUri = "https://prima.run",
            },
            new Server{
                Id = "3",
                Name = "google",
                HealthCheckUri = "https://www.google.com",
            },
        };

        public HealthCheckerQuery()
        {
            Name = "Query";


            Func<ResolveFieldContext, string, object> serverResolver = (context, id) => this.servers;

            FieldDelegate<ListGraphType<ServerType>>(
                "servers",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id", Description = "id of server" }
                ),
                resolve: serverResolver
            );

            Field<StringGraphType>(
                "hello",
                resolve: context => "world"
            );
        }
    }

    public class HealthCheckerSchema : Schema
    {
        public HealthCheckerSchema(IServiceProvider provider) : base(provider)
        {
            Query = new HealthCheckerQuery();
        }
    }
}
