namespace DigitalSignatureLibrary
{
    /// <summary>
    /// Types of XML digital signatures
    /// </summary>
    public enum SignatureType
    {
        /// <summary>
        /// The signature is contained within the document it is signing
        /// </summary>
        Enveloped,
        /// <summary>
        /// The signature is in a seperate document from the signed data
        /// </summary>
        Detached
        // <summary>
        // The signed XML is contained within the signature element
        // Not implemented
        // </summary>
        //Enveloping,
    }
}
