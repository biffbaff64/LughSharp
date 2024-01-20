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

using LibGDXSharp.Graphics.G3D.Env;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Graphics.G3D.Attributes;

public class SpotLightsAttribute : Attribute
{
    public static string Alias => "spotLights";
    public static long   Type  => Register( Alias );

    public readonly List< SpotLight > lights;

    // ------------------------------------------------------------------------

    public SpotLightsAttribute() : base( Type )
    {
        this.lights = new List< SpotLight >( 1 );
    }

    public SpotLightsAttribute( SpotLightsAttribute copyFrom ) : this()
    {
        lights.AddAll( copyFrom.lights );
    }

    /// <inheritdoc />
    public override Attribute Copy() => this;
}
