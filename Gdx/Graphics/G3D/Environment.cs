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

using LibGDXSharp.Graphics.G3D.Attributes;
using LibGDXSharp.Graphics.G3D.Env;

namespace LibGDXSharp.Graphics.G3D;

public class Environment : AttributesGroup
{
    /// <summary>
    /// Shadow map used to render shadows
    /// </summary>
    public IShadowMap? ShadowMap { get; set; }

    public Environment()
    {
    }

    /// <summary>
    /// Adds one or more Lights, extended from <see cref="BaseLight"/>, to the
    /// attributes list in <see cref="AttributesGroup"/> .
    /// </summary>
    /// <param name="lights"></param>
    /// <returns></returns>
    public Environment Add( params BaseLight[] lights )
    {
        foreach ( BaseLight light in lights )
        {
            Add( light );
        }

        return this;
    }

    public Environment Add( List< BaseLight > lights )
    {
        foreach ( BaseLight light in lights )
        {
            Add( light );
        }

        return this;
    }

    public Environment Add( BaseLight light )
    {
        if ( light is DirectionalLight directionalLight )
        {
            Add( directionalLight );
        }
        else if ( light is PointLight pointLight )
        {
            Add( pointLight );
        }
        else if ( light is SpotLight spotLight )
        {
            Add( spotLight );
        }
        else
        {
            throw new GdxRuntimeException( "Unknown light type" );
        }

        return this;
    }

    public Environment Add( DirectionalLight light )
    {
        var dirLights = ( ( DirectionalLightsAttribute? )Get( DirectionalLightsAttribute.Type ) );

        if ( dirLights == null )
        {
            Set( dirLights = new DirectionalLightsAttribute() );
        }

        dirLights.lights.Add( light );

        return this;
    }

    public Environment Add( PointLight light )
    {
        var pointLights = ( ( PointLightsAttribute? )Get( PointLightsAttribute.Type ) );

        if ( pointLights == null )
        {
            Set( pointLights = new PointLightsAttribute() );
        }

        pointLights.lights.Add( light );

        return this;
    }

    public Environment Add( SpotLight light )
    {
        var spotLights = ( ( SpotLightsAttribute? )Get( SpotLightsAttribute.Type ) );

        if ( spotLights == null )
        {
            Set( spotLights = new SpotLightsAttribute() );
        }

        spotLights.lights.Add( light );

        return this;
    }

    public Environment Remove( params BaseLight[] lights )
    {
        foreach ( BaseLight light in lights )
        {
            Remove( light );
        }

        return this;
    }

    public Environment Remove( List< BaseLight > lights )
    {
        foreach ( BaseLight light in lights )
        {
            Remove( light );
        }

        return this;
    }

    public Environment Remove( BaseLight light )
    {
        if ( light is DirectionalLight directionalLight )
        {
            Remove( directionalLight );
        }
        else if ( light is PointLight pointLight )
        {
            Remove( pointLight );
        }
        else if ( light is SpotLight spotLight )
        {
            Remove( spotLight );
        }
        else
        {
            throw new GdxRuntimeException( "Unknown light type" );
        }

        return this;
    }

    public Environment Remove( DirectionalLight light )
    {
        if ( Has( DirectionalLightsAttribute.Type ) )
        {
            var dirLights = ( ( DirectionalLightsAttribute? )Get( DirectionalLightsAttribute.Type ) );

            dirLights?.lights.Remove( light );

            if ( dirLights?.lights.Count == 0 )
            {
                Remove( DirectionalLightsAttribute.Type );
            }
        }

        return this;
    }

    public Environment Remove( PointLight light )
    {
        if ( Has( PointLightsAttribute.Type ) )
        {
            var pointLights = ( ( PointLightsAttribute? )Get( PointLightsAttribute.Type ) );

            pointLights?.lights.Remove( light );

            if ( pointLights?.lights.Count == 0 )
            {
                Remove( PointLightsAttribute.Type );
            }
        }

        return this;
    }

    public Environment Remove( SpotLight light )
    {
        if ( Has( SpotLightsAttribute.Type ) )
        {
            var spotLights = ( ( SpotLightsAttribute? )Get( SpotLightsAttribute.Type ) );

            spotLights?.lights.Remove( light );

            if ( spotLights?.lights.Count == 0 )
            {
                Remove( SpotLightsAttribute.Type );
            }
        }

        return this;
    }
}
