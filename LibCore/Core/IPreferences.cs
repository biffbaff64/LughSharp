// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Core;

/// <summary>
///     A Preference instance is a hash map holding different values. It is stored
///     in a "%USERPROFILE%/.prefs" directory. CAUTION: On the desktop platform,
///     all libgdx applications share the same ".prefs" directory. To avoid collisions
///     use specific names like "game1.settings" instead of "settings".
///     To persist changes made to a preferences instance Flush() has to be invoked.
/// </summary>
public interface IPreferences
{
    public IPreferences PutEntry( string key, object? val );

    public IPreferences PutAll( Dictionary< string, object > vals );

    public bool Getbool( string key );

    public int GetInteger( string key );

    public long GetLong( string key );

    public float GetFloat( string key );

    public bool Getbool( string key, bool defValue );

    public int GetInteger( string key, int defValue );

    public long GetLong( string key, long defValue );

    public float GetFloat( string key, float defValue );

    public string GetString( string key, string defValue );

    public Dictionary< string, object > Get();

    public bool Contains( string key );

    public void Clear();

    public void Remove( string key );

    public void Flush();
}
