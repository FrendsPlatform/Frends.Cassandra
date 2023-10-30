using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.CassandraDB.ExecuteQuery.Definitions;

namespace Frends.CassandraDB.ExecuteQuery.Tests;

[TestClass]
public class InputTests
{
    [TestMethod]
    public void CreateCertificate_CreatesCertificate()
    {
        var certPath = Path.Join("Files", "example_certificate.txt");
        var input = new Input
        {
            X509CertificateFilePath = certPath,
            X509CertificatePassword = "qwerty"
        };
        var cert = input.CreateCertificate();
        Assert.IsNotNull(cert);
    }
}