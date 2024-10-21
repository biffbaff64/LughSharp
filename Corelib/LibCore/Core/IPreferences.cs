// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


namespace Corelib.LibCore.Core;

/// <summary>
/// A Preference instance is a hash map holding different values. It is stored
/// in a "%USERPROFILE%/.prefs" directory. CAUTION: On the desktop platform,
/// all LughSharp applications share the same ".prefs" directory. To avoid collisions
/// use specific names like "game1.settings" instead of "settings".
/// To persist changes made to a preferences instance Flush() has to be invoked.
/// </summary>
[PublicAPI]
public interface IPreferences
{
    /// <summary>
    /// Maps the specified key to the specified value in this preferences liost. Neither the
    /// key nor the value can be null. The value can be retrieved by calling the get method
    /// with a key that is equal to the original key.
    /// </summary>
    /// <param name="key"> A string holding the ID or Key of this preference. </param>
    /// <param name="val"> The state or value of this preference. </param>
    public IPreferences PutEntry( string key, object? val );

    /// <summary>
    /// Calls <see cref="PutEntry"/> for each preference listed in <paramref name="vals"/>.
    /// </summary>
    public IPreferences PutAll( Dictionary< string, object > vals );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a BOOL.
    /// If the preference is not found, the default value of false will
    /// be returned.
    /// </summary>
    public bool GetBool( string key );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as an INT.
    /// If the preference is not found, the default value of zero will
    /// be returned.
    /// </summary>
    public int GetInteger( string key );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a LONG.
    /// If the preference is not found, the default value of zero will
    /// be returned.
    /// </summary>
    public long GetLong( string key );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a FLOAT.
    /// If the preference is not found, the default value of zero will
    /// be returned.
    /// </summary>
    public float GetFloat( string key );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a BOOL.
    /// If the preference is not found, the <paramref name="defValue"/> will
    /// be returned.
    /// </summary>
    public bool GetBool( string key, bool defValue );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as an INT.
    /// If the preference is not found, the <paramref name="defValue"/> will
    /// be returned.
    /// </summary>
    public int GetInteger( string key, int defValue );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a LONG.
    /// If the preference is not found, the <paramref name="defValue"/> will
    /// be returned.
    /// </summary>
    public long GetLong( string key, long defValue );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a FLOAT.
    /// If the preference is not found, the <paramref name="defValue"/> will
    /// be returned.
    /// </summary>
    public float GetFloat( string key, float defValue );

    /// <summary>
    /// Gets the preference specified by <paramref name="key"/> as a STRING.
    /// If the preference is not found, the <paramref name="defValue"/> will
    /// be returned.
    /// </summary>
    public string GetString( string key, string defValue );

    /// <summary>
    /// Returns the preferences list as a Dictionary{string,object}.
    /// </summary>
    public Dictionary< string, object > Get();

    /// <summary>
    /// Checks for the specified key in the preferences list.
    /// </summary>
    /// <param name="key"> The preference key to check for. </param>
    /// <returns> True if the preference exists. </returns>
    public bool Contains( string key );

    /// <summary>
    /// Clears ALL preferences. Does not call <see cref="Flush()"/>.
    /// </summary>
    public void Clear();

    /// <summary>
    /// Removes the preference identified by <paramref name="key"/> from the list.
    /// </summary>
    public void Remove( string key );

    /// <summary>
    /// Writes all preferences to storage location. Any changes made to
    /// preferences must be followed by a call to Flush() or those changes
    /// will not persist.
    /// </summary>
    public void Flush();
}
