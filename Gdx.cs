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

namespace LibGDXSharp;

/// <summary>
/// Environment class holding references to the Application,
/// Graphics, Audio, Files and Input instances.
/// </summary>
[PublicAPI]
public static class Gdx
{
    public static bool DevMode { get; set; } = false;
    public static bool GodMode { get; set; } = false;

    public static IApplication App      { get; set; }
    public static IGraphics    Graphics { get; set; }
    public static IAudio       Audio    { get; set; }
    public static IInput       Input    { get; set; }
    public static IFiles       Files    { get; set; }
    public static INet         Net      { get; set; }
    public static IGL20        GL       { get; set; }
    public static IGL20        GL20     { get; set; }
    public static IGL30        GL30     { get; set; }

    static Gdx()
    {
        App      = null!;
        Graphics = null!;
        Audio    = null!;
        Input    = null!;
        Files    = null!;
        Net      = null!;
        GL       = null!;
        GL20     = null!;
        GL30     = null!;
    }
}
