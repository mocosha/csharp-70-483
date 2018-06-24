$cert = New-SelfSignedCertificate -certstorelocation cert:\CurrentUser\my -KeySpec "Signature" -KeyUsage "DigitalSignature" -Subject "Document signing test" -FriendlyName "Document signing test" -NotAfter $([datetime]::now.AddYears(5))
$pwd = ConvertTo-SecureString -String passw0rd! -Force -AsPlainText
Export-PfxCertificate -Cert $cert -FilePath Cryptography\self_signed_cert.pfx -Password $pwd