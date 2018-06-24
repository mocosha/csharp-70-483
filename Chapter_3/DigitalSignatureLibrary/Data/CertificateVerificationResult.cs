using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace DigitalSignatureLibrary
{
    /// <summary>
    /// Certificate verification result
    /// </summary>
    public class CertificateVerificationResult
    {
        /// <summary>
        /// Is certificate valid
        /// </summary>
        public bool IsCertificateValid { get; set; }

        /// <summary>
        /// Certificate chain status
        /// </summary>
        public X509ChainStatus[] ChainStatus { get; set; }

        /// <summary>
        /// Certificate chain status as multiline string
        /// </summary>
        public string ChainStatusAsString
        {
            get
            {
                return string.Join(Environment.NewLine, ChainStatus.Select(s => s.StatusInformation).ToArray());
            }
        }
    }
}