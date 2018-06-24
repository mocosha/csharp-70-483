using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace DigitalSignatureLibrary
{
    /// <summary>
    /// Signature verification result
    /// </summary>
    public class SignatureVerificationResult
    {
        /// <summary>
        /// Is signature valid
        /// </summary>
        public bool IsSignatureValid { get; set; }
        /// <summary>
        /// Certificate
        /// </summary>
        public X509Certificate2 Certificate { get; set; }
        /// <summary>
        /// Error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}