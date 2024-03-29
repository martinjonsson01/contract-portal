﻿namespace Infrastructure;

/// <summary>
/// Verifies streams before allowing them to pass through.
/// </summary>
internal interface IVerifier
{
    /// <summary>
    /// Verifies if a binary data stream is a valid file that can safely be stored.
    /// </summary>
    /// <param name="stream">The data stream to verify.</param>
    /// <returns>Whether the stream is a valid image.</returns>
    bool IsValid(Stream stream);

    /// <summary>
    /// Returns the file extension matching the type of data stream.
    /// </summary>
    /// <param name="stream">The stream to find the file type of.</param>
    /// <returns>The file extension of the stream file type.</returns>
    public string GetFileExtensionOf(Stream stream);
}
