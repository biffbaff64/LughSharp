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

using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// Moves an actor from its current size to a specific size.
/// </summary>
public class SizeToAction : TemporalAction
{
    public float StartWidth  { get; set; }
    public float StartHeight { get; set; }
    public float EndWidth    { get; set; }
    public float EndHeight   { get; set; }

    protected new void Begin()
    {
        if ( Target == null ) throw new GdxRuntimeException( "Cannot Begin with null Target Actor!" );
        
        StartWidth  = Target.Width;
        StartHeight = Target.Height;
    }

    protected override void Update( float percent )
    {
        float width, height;

        if ( percent == 0 )
        {
            width  = StartWidth;
            height = StartHeight;
        }
        else if ( percent is 1.0f )
        {
            width  = EndWidth;
            height = EndHeight;
        }
        else
        {
            width  = StartWidth + ( ( EndWidth - StartWidth ) * percent );
            height = StartHeight + ( ( EndHeight - StartHeight ) * percent );
        }

        Target?.SetSize( width, height );
    }

    public void SetSize( float width, float height )
    {
        EndWidth  = width;
        EndHeight = height;
    }
}