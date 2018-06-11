$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname testcert.mocosha.com
$pwd = ConvertTo-SecureString -String ‘passw0rd!’ -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath c:\temp\cert.pfx -Password $pwd