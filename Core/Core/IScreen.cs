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
/// Represents one of many application screens, such as a main menu,
/// a settings menu, the game screen and so on.
/// Note that Dispose() is not called automatically.
/// </summary>
[PublicAPI]
public interface IScreen
{
    void Show();

    void Render( float delta );

    void Resize( int width, int height );

    void Pause();

    void Resume();

    void Hide();

    void Dispose();
}