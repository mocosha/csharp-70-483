using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace DigitalSignatureLibrary
{
    /// <summary>
    /// XML Signature class, based on W3C Recommendation 10 June 2008
    /// http://www.w3.org/TR/xmldsig-core/
    /// Online verification tool
    /// https://www.signatur.rtr.at/en/elsi/Pruefung.html
    /// </summary>    
    public class XmlDsig
    {
        /// <summary>
        /// Sign an XML file. 
        /// This document cannot be verified unless the verifying code has the key with which it was signed.
        /// </summary>
        /// <param name="doc">Xml document</param>
        /// <param name="certificate">Certificate</param>
        /// <param name="signatureType">Signature type</param>
        /// <returns>signed document</returns>
        public static XmlDocument Sign(XmlDocument doc, X509Certificate2 certificate, SignatureType signatureType = SignatureType.Enveloped)
        {
            // Check arguments.
            if (doc == null)
                throw new ArgumentException("doc");
            if (certificate == null)
                throw new ArgumentException("certificate");            

            // Create a SignedXml object.
            SignedXml signedXml = new SignedXml(doc);
            // Add the key to the SignedXml document.
            signedXml.SigningKey = certificate.PrivateKey;

            // Create a reference to be signed.
            Reference reference = new Reference();
            reference.Uri = "";

            // Add an enveloped transformation to the reference.
            XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
            reference.AddTransform(env);

            //if (c14)
            //{
            //    XmlDsigC14NTransform c14t = new XmlDsigC14NTransform();
            //    reference.AddTransform(c14t);
            //}

            //include key details
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
            keyInfo.AddClause(keyInfoData);
            signedXml.KeyInfo = keyInfo;          
            
            // Add the reference to the SignedXml object.
            signedXml.AddReference(reference);

            // Compute the signature.
            signedXml.ComputeSignature();

            // Get the XML representation of the signature and save
            // it to an XmlElement object.
            XmlElement xmlDigitalSignature = signedXml.GetXml();

            if (signatureType == SignatureType.Detached)
            {
                var detachedSignature = new XmlDocument();
                detachedSignature.LoadXml(xmlDigitalSignature.OuterXml);
                return detachedSignature;
            }

            // Append the element to the XML document.
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));
            return doc;
        }

        /// <summary>
        /// Verify the signature of an Xml
        /// </summary>
        /// <param name="doc">signed XML document</param>
        /// <returns>signature verification result</returns>
        public static SignatureVerificationResult Verify(XmlDocument doc)
        {            
            if (doc == null)
                throw new ArgumentNullException("doc");

            SignedXml signedXml = new SignedXml(doc);            

            // load the first <signature> node and load the signature  
            XmlNode MessageSignatureNode = doc.GetElementsByTagName("Signature")[0];

            signedXml.LoadXml((XmlElement)MessageSignatureNode);
            
            // get the cert from the signature
            X509Certificate2 certificate = null;
            foreach (KeyInfoClause clause in signedXml.KeyInfo)
            {
                if (clause is KeyInfoX509Data)
                {
                    if (((KeyInfoX509Data)clause).Certificates.Count > 0)                    
                        certificate = (X509Certificate2)((KeyInfoX509Data)clause).Certificates[0];
                }
            }

            var result = new SignatureVerificationResult();

            if (certificate == null)
            {
                result.IsSignatureValid = false;
                result.ErrorMessage = "Certificate not found";
                return result;
            }
            
            result.IsSignatureValid = Verify(doc, certificate);
            result.Certificate = certificate;
            return result;
        }

        /// <summary>
        /// Verify the signature of an XML file against an asymmetric algorithm and return the result.
        /// </summary>
        /// <param name="doc">signed XML document</param>
        /// <param name="certificate">certificate</param>
        /// <returns>boolean, true if signature is valid, false otherwise</returns>
        public static bool Verify(XmlDocument doc, X509Certificate2 certificate)
        {
            // Check arguments.
            if (doc == null)
                throw new ArgumentNullException("doc");
            if (certificate == null)
                throw new ArgumentNullException("key");

            // Create a new SignedXml object and pass it the XML document class.
            SignedXml signedXml = new SignedXml(doc);

            // Find the "Signature" node and create a new XmlNodeList object.
            XmlNodeList nodeList = doc.GetElementsByTagName("Signature");

            // Throw an exception if no signature was found.
            if (nodeList.Count <= 0)
            {
                throw new CryptographicException("Verification failed: No Signature was found in the document.");
            }

            // This example only supports one signature for the entire XML document.  
            // Throw an exception if more than one signature was found.
            if (nodeList.Count >= 2)
            {
                throw new CryptographicException("Verification failed: More that one signature was found for the document.");
            }

            // Load the first <signature> node.  
            signedXml.LoadXml((XmlElement)nodeList[0]);

            // Check the signature and return the result.                  
            return signedXml.CheckSignature(certificate, true);
        }
    }
}
