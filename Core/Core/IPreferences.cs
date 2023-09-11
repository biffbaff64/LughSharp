// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Core;

/// <summary>
/// A Preference instance is a hash map holding different values. It is stored
/// in a "%USERPROFILE%/.prefs" directory. CAUTION: On the desktop platform,
/// all libgdx applications share the same ".prefs" directory. To avoid collisions
/// use specific names like "game1.settings" instead of "settings".
/// To persist changes made to a preferences instance Flush() has to be invoked.
/// </summary>
[PublicAPI]
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