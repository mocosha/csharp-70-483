using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Linq;

namespace DigitalSignatureLibrary
{
    /// <summary>
    /// Certificate management utility class
    /// </summary>
    public static class CertificateUtility
    {
        /// <summary>
        /// Find certificate from current user personal store by thumbprint
        /// </summary>
        /// <param name="certificateThumbprint">certificate thumbprint</param>
        /// <returns>certificate</returns>
        public static X509Certificate2 FindCertificateByThumbprint(string certificateThumbprint)
        {
            var certificates = FindCertificateByProperty(X509FindType.FindByThumbprint, certificateThumbprint);                                    
            return certificates.FirstOrDefault();
        }

        /// <summary>
        /// Find certificate from current user personal store by specified property type and value
        /// </summary>
        /// <param name="storeName">store name</param>
        /// <param name="storeLocation">store location CurrentUser/LocalMachine</param>
        /// <param name="certificateThumbprint">certificate thumbprint</param>
        /// <returns>certificate</returns>
        public static X509Certificate2 FindCertificateByThumbprint(StoreName storeName, StoreLocation storeLocation, string certificateThumbprint)
        {
            var certificates = FindCertificateByProperty(storeName, storeLocation, X509FindType.FindByThumbprint, certificateThumbprint);
            return certificates.FirstOrDefault();
        }

        /// <summary>
        /// Find certificates from current user personal store by specified property type and value
        /// </summary>
        /// <param name="findType">property type</param>
        /// <param name="findValue">property value</param>
        /// <returns>certificates</returns>
        public static IEnumerable<X509Certificate2> FindCertificateByProperty(X509FindType findType, string findValue)
        {
            return FindCertificateByProperty(StoreName.My, StoreLocation.CurrentUser, findType, findValue);
        }

        /// <summary>
        /// Find certificates from specified store and location by property type and value
        /// </summary>
        /// <param name="storeName">store name</param>
        /// <param name="storeLocation">store location CurrentUser/LocalMachine</param>
        /// <param name="findType">property type</param>
        /// <param name="findValue">property value</param>
        /// <returns>certificates</returns>
        public static IEnumerable<X509Certificate2> FindCertificateByProperty(StoreName storeName, StoreLocation storeLocation, X509FindType findType, string findValue)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var certificates = store.Certificates.Find(findType, findValue, false);
            store.Close();

            foreach (var cert in certificates)
                yield return cert;
        }

        /// <summary>
        /// Load Certificates from store
        /// </summary>
        /// <param name="storeName">store name</param>
        /// <param name="storeLocation">store location CurrentUser/LocalMachine</param>
        /// <returns>certificate collection</returns>
        public static X509Certificate2Collection LoadCertificates(StoreName storeName, StoreLocation storeLocation)
        {            
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            return store.Certificates;
        }

        /// <summary>
        /// Validate certificate against policy chain
        /// </summary>
        /// <param name="certificate">certificate</param>
        /// <returns>certificate verification result</returns>
        public static CertificateVerificationResult Verify(X509Certificate2 certificate)
        {
            return Verify(certificate, X509RevocationFlag.EntireChain,
                    X509RevocationMode.Online,
                    new TimeSpan(0, 0, 30),
                    X509VerificationFlags.AllFlags);
        }

        /// <summary>
        /// Validate certificate against policy chain
        /// </summary>
        /// <param name="certificate">certificate</param>
        /// <param name="revocationFlag">revocation flag</param>
        /// <param name="revocationMode">revocation mode</param>
        /// <param name="retrievalTimeout">retrieval timeout</param>
        /// <param name="verificationFlags">verification flags</param>
        /// <returns>certificate verification result</returns>
        public static CertificateVerificationResult Verify(X509Certificate2 certificate, X509RevocationFlag revocationFlag, X509RevocationMode revocationMode, TimeSpan retrievalTimeout, X509VerificationFlags verificationFlags)
        {
            //chain information of the selected certificate.
            var chain = new X509Chain();
            chain.ChainPolicy.RevocationFlag = revocationFlag;
            chain.ChainPolicy.RevocationMode = revocationMode;
            chain.ChainPolicy.UrlRetrievalTimeout = retrievalTimeout;
            chain.ChainPolicy.VerificationFlags = verificationFlags;

            var isValid = chain.Build(certificate);

            return new CertificateVerificationResult
            {
                IsCertificateValid = isValid,
                ChainStatus = chain.ChainStatus
            };
        }

        /// <summary>
        /// Get client certificates (for current user from Personal store)
        /// </summary>
        /// <returns>certificates</returns>
        public static IEnumerable<X509Certificate2> GetClientCertificates()
        {
            foreach (var cert in CertificateUtility.LoadCertificates(StoreName.My, StoreLocation.CurrentUser))
            {
                //if (string.IsNullOrWhiteSpace(cert.FriendlyName))
                //    continue;

                yield return cert;
            }
        }
    }
}
