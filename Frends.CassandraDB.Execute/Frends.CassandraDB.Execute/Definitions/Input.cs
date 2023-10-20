using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace Frends.CassandraDB.Execute.Definitions;

/// <summary>
/// Input parameters.
/// </summary>
public class Input
{
    /// <summary>
    /// Authentication method.
    /// </summary>
    /// <example>AuthenticationMethods.PlainTextAuthProvider</example>
    [DefaultValue(AuthenticationMethods.PlainTextAuthProvider)]
    public AuthenticationMethods AuthenticationMethods { get; set; }

    /// <summary>
    /// Use Ssl.
    /// </summary>
    /// <example>false</example>
    [DefaultValue(false)]
    public bool UseSsl { get; set; }

    /// <summary>
    /// X509 certificate location.
    /// </summary>
    /// <example>c:\temp</example>
    [UIHint(nameof(UseSsl), "", true)]
    public string X509CertificateFilePath { get; set; }

    /// <summary>
    /// X509 certificate's password.
    /// </summary>
    /// <example>FooBar</example>
    [UIHint(nameof(UseSsl), "", true)]
    [PasswordPropertyText]
    public string X509CertificatePassword { get; set; }

    /// <summary>
    /// Username.
    /// </summary>
    /// <example>Foo</example>
    public string Username { get; set; }

    /// <summary>
    /// Password.
    /// </summary>
    /// <example>Bar</example>
    public string Password { get; set; }

    /// <summary>
    /// Connect with 'Input.Username' credentials, but will act as 'Input.AsUser'.
    /// </summary>
    /// <example>Foo2</example>
    [UIHint(nameof(AuthenticationMethods), "", AuthenticationMethods.DsePlainTextAuthProvider)]
    public string AsUser { get; set; }

    /// <summary>
    /// Contact point(s).
    /// </summary>
    /// <example>Object{127.0.0.1}</example>
    public ContactPoint[] ContactPoints { get; set; }

    /// <summary>
    /// Port.
    /// </summary>
    /// <example>9042</example>
    [DefaultValue(9042)]
    public int Port { get; set; }

    /// <summary>
    /// Keyspace (Case-sensitive).
    /// </summary>
    /// <example>foo</example>
    public string Keyspace { get; set; }

    /// <summary>
    /// Query to execute.
    /// </summary>
    /// <example>SELECT bar FROM foo</example>
    public string Query { get; set; }

    internal X509Certificate2Collection CreateCertificate()
    {
        return new X509Certificate2Collection {
            new (X509CertificateFilePath, X509CertificatePassword) };
    }
}

/// <summary>
/// Contactpoint's value.
/// </summary>
public class ContactPoint
{
    /// <summary>
    /// Contactpoint.
    /// </summary>
    /// <example>127.0.0.1</example>
    public string Value { get; set; }
}