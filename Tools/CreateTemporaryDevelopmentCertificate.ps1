New-SelfSignedCertificate -Type Custom `
                          -Subject "CN=OS Development Group" `
                          -KeyUsage DigitalSignature `
                          -FriendlyName "Temporary Development Certificate for OS Development Group" `
                          -CertStoreLocation "Cert:\CurrentUser\My" `
                          -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.3", "2.5.29.19={text}")