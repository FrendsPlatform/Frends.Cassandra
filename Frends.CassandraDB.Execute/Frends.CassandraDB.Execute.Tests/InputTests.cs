using Microsoft.VisualStudio.TestTools.UnitTesting;
using Frends.CassandraDB.Execute.Definitions;

namespace Frends.CassandraDB.Execute.Tests;

[TestClass]
public class InputTests
{
    [TestMethod]
    public void CreateCertificate_CreatesCertificate()
    {
        var tmpFile = Path.GetTempFileName();
        File.WriteAllText(tmpFile, @"-----BEGIN CERTIFICATE-----
MIICuDCCAaACCQD5fsMPyPYqvjANBgkqhkiG9w0BAQsFADAeMQswCQYDVQQGEwJG
STEPMA0GA1UECgwGRnJlbmRzMB4XDTIzMDkyOTA4MzYyOFoXDTI0MDkyODA4MzYy
OFowHjELMAkGA1UEBhMCRkkxDzANBgNVBAoMBkZyZW5kczCCASIwDQYJKoZIhvcN
AQEBBQADggEPADCCAQoCggEBALDUcXB7NQoTZ/TzAYemdU6cgKtYVVZt3j15T5jQ
Zg/PYybgDJtLhBStgNfhxln+aW838ZWo/hU84RXX6lJ8Nr/9IqW6+i7DPTlWdzVv
/+jwMtM1u5RmO0+fzMBduUqwAv6V1AfEm+R7y5V2yFaQOGNRSLJY8kigvIMJQSZi
vu6zqSUKMZuZY9bXwR4vd+DZXWyy0FNfb7YwyhXOgdGFeLiU+lz3jb93k/Gvl5XQ
g2XXt1+1eRNCaFlkoF80UVigS5ovhhqgH+CIXtpyn2Fswb6N5eHMp4yn1qXBP6LI
22ffGIrILIIvWm5llkrx0SF8G23MDZrRDdpyagCYSHU2zWUCAwEAATANBgkqhkiG
9w0BAQsFAAOCAQEAaQSkOSrzr4HEftvb6d4TnI7mq5K0RSjGYrvy4PYOH55kLFo3
4Q+oGSZokC18dsZ/QXm4Trf8+qiVO9+mhOUjifVHtP4sfIKHJBGcs922p75uKScJ
+QTenBcCKLxUhdvYTkzMrCE/GlW5LZONkxHKBSmS0nwf23xo1VUmQfyg/eCnhi0n
F1vSfPY5quXFSfgujNsw8njTe11WnQxQojJTsj+9oQ5oKpi70kpQLLeGwk4dOUkv
qaBe3r02o8qTbOoyaAI81W/xUpn73OBgm+J4Uhc4Xb+RJAybUo54MDfGLPAzNnEU
D0GyyHxM3a/800m5ZGn051CMbmcYvY+s4N8idA==
-----END CERTIFICATE-----");
        var input = new Input {
            X509CertificateFilePath = tmpFile,
            X509CertificatePassword = "qwerty"
        };
        var cert = input.CreateCertificate();
        Assert.IsNotNull(cert);
    }
}