namespace Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Represents an X.509 certificate
    /// </summary>
    public class Certificate
    {
        private X509Certificate2 certificate;
        private X509Chain chain;

        /// <summary>
        /// Chain status and error information
        /// </summary>
        public X509ChainStatus[] ChainStatus { get { return chain == null ? null : chain.ChainStatus; } }

        /// <summary>
        /// Certificate Subject
        /// </summary>
        public string Subject { get { return certificate.Subject; } }
        /// <summary>
        /// Certificate Version
        /// </summary>
        public int Version { get { return certificate.Version; } }
        /// <summary>
        /// Certificate Valid Date (NotBefore)
        /// </summary>
        public DateTime ValidDate { get { return certificate.NotBefore; } }
        /// <summary>
        /// Certificate Expiry Date (NotAfter)
        /// </summary>
        public DateTime ExpiryDate { get { return certificate.NotAfter; } }
        /// <summary>
        /// Certificate Thumbprint
        /// </summary>
        public string Thumbprint { get { return certificate.Thumbprint; } }
        /// <summary>
        /// Certificate SerialNubmer
        /// </summary>
        public string SerialNumber { get { return certificate.SerialNumber; } }
        /// <summary>
        /// Certificate Friendly Name
        /// </summary>
        public string FriendlyName { get { return certificate.PublicKey.Oid.FriendlyName; } }
        /// <summary>
        /// Certificate Public Key Format
        /// </summary>
        public string PublicKeyFormat { get { return certificate.PublicKey.EncodedKeyValue.Format(true); } }
        /// <summary>
        /// Certificate Row Data Length
        /// </summary>
        public int RowDataLength { get { return certificate.RawData.Length; } }
        /// <summary>
        /// Returns a string representation of the current Certificate object
        /// </summary>
        public string CertificateToString { get { return certificate.ToString(true); } }
        /// <summary>
        /// Gets a private key associated with a certificate
        /// </summary>
        public RSACryptoServiceProvider CryptoServiceProvider { get { return (RSACryptoServiceProvider)certificate.PrivateKey; } }
        /// <summary>
        /// Gets a private key associated with a certificate as RSA object
        /// </summary>        
        public RSA PrivateKey { get { return (RSA)certificate.PrivateKey; } }
        /// <summary>
        /// Gets a public key associated with a certificate
        /// </summary>
        public PublicKey PublicKey { get { return certificate.PublicKey; } }
        /// <summary>
        /// Gets a original certificate
        /// </summary>
        public X509Certificate2 X509Certificate2 { get { return certificate; } }
        /// <summary>
        /// Default Constructor, initialize a certificate!
        /// </summary>
        public Certificate()
        {
        }

        /// <summary>
        /// Initalize from X509Certificate2
        /// </summary>
        /// <param name="certificate"></param>
        public Certificate(X509Certificate2 certificate)
        {
            this.certificate = certificate;
        }

        /// <summary>
        /// Initialize Certificate using default store ('My') and subject
        /// </summary>
        /// <param name="certSubject">Part of the subject</param>
        public Certificate(string certSubject)
        {
            InitializeFromStore(StoreName.My, StoreLocation.CurrentUser, certSubject);
        }

        /// <summary>
        /// Initialize Certificate using a certificate file name and password
        /// </summary>
        /// <param name="fileName">Certification file name</param>
        /// <param name="password">Password</param>
        public void InitializeFromFile(string fileName, string password)
        {
            certificate = new X509Certificate2(fileName, password);
        }

        /// <summary>
        /// Initialize Certificate using a Store and subject
        /// </summary>
        /// <param name="storeName">My</param>
        /// <param name="storeLocation">"Local User"</param>
        /// <param name="certSubject">Part of the subject</param>        
        public void InitializeFromStore(StoreName storeName, StoreLocation storeLocation, string certSubject)
        {
            // Check the args.
            if (null == certSubject)
                throw new ArgumentNullException("Certificate Name");

            // Load the certificate from the certificate store.
            X509Certificate2 cert = null;

            X509Store store = new X509Store(storeName, storeLocation);

            try
            {
                // Open the store.
                //store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                store.Open(OpenFlags.ReadOnly);

                // Get the certs from the store.
                X509Certificate2Collection CertCol = store.Certificates;

                // Find the certificate with the specified subject.
                foreach (X509Certificate2 c in CertCol)
                {
                    if (c.Subject.Contains(certSubject))
                    {
                        cert = c;
                        break;
                    }
                }

                // Throw an exception of the certificate was not found.
                if (cert == null)
                {
                    throw new CryptographicException("The certificate could not be found.");
                }
            }
            finally
            {
                // Close the store even if an exception was thrown.
                store.Close();
            }

            certificate = cert;
        }

        /// <summary>
        /// Initialize Certificate using a Store and Friendly name
        /// </summary>
        /// <param name="certFriendlyName">Certificate Friendly Name</param>        
        public void InitializeByFriendlyName(string certFriendlyName)
        {
            // Check the args.
            if (null == certFriendlyName)
                throw new ArgumentNullException("Certificate Name");

            // Load the certificate from the certificate store.
            X509Certificate2 cert = null;

            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);

            try
            {
                // Open the store.                
                store.Open(OpenFlags.ReadOnly);

                // Get the certs from the store.
                X509Certificate2Collection CertCol = store.Certificates;

                // Find the certificate with the specified subject.
                foreach (X509Certificate2 c in CertCol)
                {
                    if (c.FriendlyName.StartsWith(certFriendlyName))
                    {
                        cert = c;
                        break;
                    }
                }

                // Throw an exception of the certificate was not found.
                if (cert == null)
                {
                    throw new CryptographicException("The certificate could not be found.");
                }
            }
            finally
            {
                // Close the store even if an exception was thrown.
                store.Close();
            }

            certificate = cert;
        }

        /// <summary>
        /// Validate certificate against policy chain
        /// </summary>
        /// <returns></returns>
        public bool Verificate()
        {
            return Verificate(X509RevocationFlag.EntireChain,
                    X509RevocationMode.Online,
                    new TimeSpan(0, 0, 30),
                    X509VerificationFlags.AllFlags);
        }

        /// <summary>
        /// Validate certificate against policy chain
        /// </summary>
        /// <param name="revocationFlag"></param>
        /// <param name="revocationMode"></param>
        /// <param name="retrievalTimeout"></param>
        /// <param name="verificationFlags"></param>
        /// <returns></returns>
        public bool Verificate(X509RevocationFlag revocationFlag, X509RevocationMode revocationMode, TimeSpan retrievalTimeout, X509VerificationFlags verificationFlags)
        {
            //chain information of the selected certificate.
            chain = new X509Chain();
            chain.ChainPolicy.RevocationFlag = revocationFlag;
            chain.ChainPolicy.RevocationMode = revocationMode;
            chain.ChainPolicy.UrlRetrievalTimeout = retrievalTimeout;
            chain.ChainPolicy.VerificationFlags = verificationFlags;

            return chain.Build(certificate);
        }
    }
}
