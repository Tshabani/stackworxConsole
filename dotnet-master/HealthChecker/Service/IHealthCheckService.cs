using HealthChecker.GraphQL;
using HealthChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthChecker.Service
{
    public interface IHealthCheckService
    {
        Task<Response> GetStatusAsync();
        Task<Server> GetStatusWithRestsharpAsync();
    }
}
