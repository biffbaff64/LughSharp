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

public class ScaleByAction : RelativeTemporalAction
{
    public float AmountX { get; set; }
    public float AmountY { get; set; }

    protected override void UpdateRelative( float percentDelta )
    {
        Target?.ScaleBy( AmountX * percentDelta, AmountY * percentDelta );
    }

    public void SetAmount( float x, float y )
    {
        AmountX = x;
        AmountY = y;
    }

    public void SetAmount( float scale )
    {
        AmountX = scale;
        AmountY = scale;
    }
}