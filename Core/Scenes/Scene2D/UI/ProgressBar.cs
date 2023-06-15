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

using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

public class ProgressBar
{
    /// <summary>
    /// The style for a progress bar, see <see cref="ProgressBar"/>.
    /// </summary>
    public class ProgressBarStyle
    {
        // The progress bar background, stretched only in one direction.
        public IDrawable Background         { get; set; }
        public IDrawable DisabledBackground { get; set; }
        public IDrawable Knob               { get; set; }
        public IDrawable DisabledKnob       { get; set; }
        public IDrawable KnobBefore         { get; set; }
        public IDrawable DisabledKnobBefore { get; set; }
        public IDrawable KnobAfter          { get; set; }
        public IDrawable DisabledKnobAfter  { get; set; }

        public ProgressBarStyle()
        {
        }

        public ProgressBarStyle( IDrawable background, IDrawable knob )
        {
            this.Background = background;
            this.Knob       = knob;
        }

        public ProgressBarStyle( ProgressBarStyle style )
        {
            Background         = style.Background;
            DisabledBackground = style.DisabledBackground;
            Knob               = style.Knob;
            DisabledKnob       = style.DisabledKnob;
            KnobBefore         = style.KnobBefore;
            DisabledKnobBefore = style.DisabledKnobBefore;
            KnobAfter          = style.KnobAfter;
            DisabledKnobAfter  = style.DisabledKnobAfter;
        }
    }
}
