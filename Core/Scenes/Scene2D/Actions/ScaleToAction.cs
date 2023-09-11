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

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[PublicAPI]
public class ScaleToAction : TemporalAction
{
    public float EndX { get; set; }
    public float EndY { get; set; }
    
    private float _startX;
    private float _startY;

    protected new void Begin()
    {
        _startX = Target!.ScaleX;
        _startY = Target.ScaleY;
    }

    protected override void Update( float percent )
    {
        float x, y;

        if ( percent == 0 )
        {
            x = _startX;
            y = _startY;
        }
        else if ( percent is 1.0f )
        {
            x = EndX;
            y = EndY;
        }
        else
        {
            x = _startX + ( ( EndX - _startX ) * percent );
            y = _startY + ( ( EndY - _startY ) * percent );
        }

        Target?.SetScale( x, y );
    }

    public void SetScale( float x, float y )
    {
        EndX = x;
        EndY = y;
    }

    public void SetScale( float scale )
    {
        EndX = scale;
        EndY = scale;
    }
}