New-SelfSignedCertificate -certstorelocation cert:\CurrentUser\my -KeySpec "Signature" -KeyUsage "DigitalSignature" -Subject "Document signing test" -FriendlyName "Document signing test" -NotAfter $([datetime]::now.AddYears(5))