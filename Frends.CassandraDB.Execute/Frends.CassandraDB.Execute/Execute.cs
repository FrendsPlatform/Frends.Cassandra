﻿using Frends.CassandraDB.Execute.Definitions;
using System.ComponentModel;
using System;
using System.Threading;
using System.Collections.Generic;
using Cassandra;
using Cassandra.Data.Linq;
using Newtonsoft.Json;
using Cassandra.DataStax.Auth;
using System.Security.Cryptography.X509Certificates;

namespace Frends.CassandraDB.Execute;

/// <summary>
/// CassandraDB Task.
/// </summary>
public class CassandraDB
{
    /// <summary>
    /// CassandraDB Execute operation.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.CassandraDB.Execute)
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <param name="cancellationToken">Token generated by Frends to stop this task.</param>
    /// <returns>Object { bool Success, string QueryResults, List&lt;string&gt; Warnings }</returns>
    public static Result Execute([PropertyTab] Input input, CancellationToken cancellationToken)
    {
        var queryResult = new Dictionary<int, List<QueryResult>>();
        var warnings = new List<string>();
        var cluster = GetCluster(input);

        try
        {
            var statement = string.IsNullOrWhiteSpace(input.AsUser) ? new SimpleStatement(input.Query) : new SimpleStatement(input.Query).ExecutingAs(input.AsUser);
            var rs = cluster.Execute(statement);
            int count = 0;

            foreach (var i in rs)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var list = new List<QueryResult>();
                var rows = i;
                for (var j = 0; j < rs.Columns.Length; j++)
                    list.Add(new QueryResult() { Key = rs.Columns[j].Name, Value = rows[j] });

                queryResult.Add(count, list);
                count++;
            }

            if (rs.Info.Warnings != null)
                foreach (var w in rs.Info.Warnings)
                    warnings.Add(w.ToString());

            return new Result(rs.IsFullyFetched, JsonConvert.SerializeObject(queryResult), warnings.Count > 0 ? warnings : null);
        }
        catch (CqlArgumentException ex)
        {
            throw new Exception($"Execute error (CqlArgumentException): {ex}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Execute error (Exception): {ex}");
        }
        finally
        {
            cluster.ShutdownAsync().Wait(cancellationToken);
        }
    }

    private static ISession GetCluster(Input input)
    {
        Cluster cluster;

        try
        {
            switch (input.AuthenticationMethods)
            {
                case AuthenticationMethods.None:
                    if (input.UseSsl)
                        cluster = Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port)
                            .WithSSL(new SSLOptions().SetCertificateCollection(new X509Certificate2Collection { new X509Certificate2(@$"{input.X509Certificate}", input.X509CertificatePassword) })).Build();
                    else
                        cluster = Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).Build();
                    break;

                case AuthenticationMethods.PlainTextAuthProvider:
                    if (input.UseSsl)
                        cluster = Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).WithCredentials(input.Username, input.Password)
                            .WithSSL(new SSLOptions().SetCertificateCollection(new X509Certificate2Collection { new X509Certificate2(@$"{input.X509Certificate}", input.X509CertificatePassword) })).Build();
                    else
                        cluster = Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).WithCredentials(input.Username, input.Password).Build();
                    break;

                case AuthenticationMethods.DsePlainTextAuthProvider:
                    if (input.UseSsl)
                        cluster = string.IsNullOrWhiteSpace(input.AsUser)
                        ? Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).WithAuthProvider(new DsePlainTextAuthProvider(input.Username, input.Password))
                            .WithSSL(new SSLOptions().SetCertificateCollection(new X509Certificate2Collection { new X509Certificate2(@$"{input.X509Certificate}", input.X509CertificatePassword) })).Build()
                        : Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).WithAuthProvider(new DsePlainTextAuthProvider(input.Username, input.Password, input.AsUser))
                            .WithSSL(new SSLOptions().SetCertificateCollection(new X509Certificate2Collection { new X509Certificate2(@$"{input.X509Certificate}", input.X509CertificatePassword) })).Build();
                    else
                        cluster = string.IsNullOrWhiteSpace(input.AsUser)
                        ? Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).WithAuthProvider(new DsePlainTextAuthProvider(input.Username, input.Password)).Build()
                        : Cluster.Builder().AddContactPoints(GetContactPoints(input)).WithPort(input.Port).WithAuthProvider(new DsePlainTextAuthProvider(input.Username, input.Password, input.AsUser)).Build();
                    break;
                default:
                    throw new Exception("GetCluster error: Authentication method not supported.");
            }

            var session = string.IsNullOrWhiteSpace(input.Keyspace)
                        ? cluster.Connect()
                        : cluster.Connect(input.Keyspace);

            return session;

        }
        catch (CqlArgumentException ex)
        {
            throw new Exception($"GetCluster error (CqlArgumentException): {ex}");
        }
        catch (Exception ex)
        {
            throw new Exception($"GetCluster error (Exception): {ex}");
        }
    }

    private static string[] GetContactPoints(Input input)
    {
        var cps = new List<string>();

        foreach (var i in input.ContactPoints)
            cps.Add(i.Value);

        return cps.ToArray();
    }
}