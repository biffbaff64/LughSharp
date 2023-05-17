namespace LibGDXSharp.Utils;

/// <summary>
/// A very simple clipboard interface for text content.
/// </summary>
public interface IClipboard
{
    /// <summary>
    /// Check if the clipboard has contents.
    /// </summary>
    /// <returns> true, if the clipboard has contents</returns>
    bool HasContents();

    /// <summary>
    /// The current content of the clipboard if it contains text
    /// </summary>
    /// <returns> the clipboard content or null  </returns>
    string Contents {get; set;}

}