using DigitalSignatureLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Cryptography
{
    class Program
    {
        static string SignXml(Certificate certificate, string sourcePath, string signedPath)
        {
            // Create a new XML document.
            XmlDocument xmlDoc = new XmlDocument();

            // Load an XML file into the XmlDocument object.
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(sourcePath);

            var signatureType = SignatureType.Enveloped;

            // Sign the XML document.
            xmlDoc = XmlDsig.Sign(xmlDoc, certificate.X509Certificate2, signatureType);

            // Save the document.
            xmlDoc.Save(signedPath);
            return $"XML file signed, path:{signedPath}";
        }

        static string VerifyXmlSignature(string signedPath)
        {
            var status = new StringBuilder();

            // Create a new XML document.
            XmlDocument xmlDoc = new XmlDocument();

            // Load an XML file into the XmlDocument object.
            xmlDoc.PreserveWhitespace = true;
            xmlDoc.Load(signedPath);

            status.AppendLine("Verifying signature...");

            // Verify the signature of the signed XML.
            var signatureVerificationResult = XmlDsig.Verify(xmlDoc);

            // Verify the certificate
            var certificateVerificationResult = CertificateUtility.Verify(signatureVerificationResult.Certificate);

            // Display the results of the signature verification to the console.
            if (signatureVerificationResult.IsSignatureValid)
            {
                status.AppendLine("The XML signature is valid.");
                status.AppendFormat("Is certificate valid: \t{0}{1}", certificateVerificationResult.IsCertificateValid, Environment.NewLine);

                var certificate = signatureVerificationResult.Certificate;
                status.AppendFormat("Name: \t{0}{1}", certificate.FriendlyName, Environment.NewLine);
                status.AppendFormat("Subject: \t{0}{1}", certificate.Subject, Environment.NewLine);
                status.AppendFormat("Version: \t{0}{1}", certificate.Version, Environment.NewLine);
                status.AppendFormat("Serial Number: \t{0}{1}", certificate.SerialNumber, Environment.NewLine);

                status.AppendFormat("Not Before: \t{0}{1}", certificate.NotBefore, Environment.NewLine);
                status.AppendFormat("Not After: \t{0}{1}", certificate.NotAfter, Environment.NewLine);
            }
            else
            {
                status.AppendLine("The XML signature is not valid.");
                status.AppendFormat("Message: \t{0}{1}", signatureVerificationResult.ErrorMessage, Environment.NewLine);
            }

            return status.ToString();
        }

        static void SigningSample()
        {
            var certificate = new Certificate();
            certificate.InitializeFromFile("self_signed_cert.pfx", "passw0rd!");
            Console.WriteLine($"Certificat thumbprint: {certificate.Thumbprint}");

            var signingResult = SignXml(certificate, "test.xml", "signed.xml");
            Console.WriteLine(signingResult);

            var verifyingResult = VerifyXmlSignature("signed.xml");
            Console.WriteLine(verifyingResult);
        }

        static void EncryptDecryptSample()
        {
            var certificate = new Certificate();
            certificate.InitializeFromFile("self_signed_exchange.pfx", "passw0rd!");

            // Encrypt / Decrypt
            var message = "secret message";
            Console.WriteLine($"Original message: {message}");
            Console.WriteLine();

            var cryptedMessage = certificate.CryptoServiceProvider.Encrypt(Encoding.ASCII.GetBytes(message), fOAEP: true);
            var cryptedMessageAsString = Encoding.Default.GetString(cryptedMessage);
            Console.WriteLine($"Crypted mesage: {cryptedMessageAsString}");
            Console.WriteLine();

            var decryptedMessage = certificate.CryptoServiceProvider.Decrypt(cryptedMessage, true);
            var decryptedMessageAsString = Encoding.Default.GetString(decryptedMessage);
            Console.WriteLine($"Decrypted mesage: {decryptedMessageAsString}");
        }

        static void Main(string[] args)
        {
            //SigningSample();

            EncryptDecryptSample();


            Console.ReadKey();
        }
    }
}
