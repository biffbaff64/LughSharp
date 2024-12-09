// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Maths;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// Keeps track of an application's tooltips.
/// </summary>
[PublicAPI]
public class TooltipManager< T > where T : Actor
{
    private readonly List< Tooltip< T > > _activeTooltips = [ ];

    private readonly CancellationToken       _showTaskCancellationToken;
    private readonly CancellationTokenSource _showTaskCancellationTokenSource;
    private          Task                    _showTask = null!;

    private Tooltip< T > _showTooltip = null!;
    private float        _time;

    public TooltipManager()
    {
        _time                            = InitialTime;
        _showTaskCancellationTokenSource = new CancellationTokenSource();
        _showTaskCancellationToken       = _showTaskCancellationTokenSource.Token;

        Create();
    }

    // ========================================================================

    private void Create()
    {
        //@formatter:off
        _showTask = new Task( () =>
        {
            var stage = _showTooltip.TargetActor?.Stage;

            if ( stage == null )
            {
                return;
            }

            stage.AddActor( _showTooltip.Container );

            _showTooltip.Container.ToFront();
            _activeTooltips.Add( _showTooltip );
            _showTooltip.Container.ClearActions();

            ShowAction( _showTooltip );

            if ( !_showTooltip.Instant )
            {
                _time = SubsequentTime;
            }

            if ( _showTaskCancellationToken.IsCancellationRequested )
            {
                _showTaskCancellationToken.ThrowIfCancellationRequested();
            }
        },_showTaskCancellationToken );
        //@formatter:on
    }

    public void TouchDown( Tooltip< T > tooltip )
    {
        CancelTask();

        tooltip.Container.Remove();

        _time = InitialTime;

        if ( Enabled || tooltip.Always )
        {
            _showTooltip = tooltip;

//            Timer.schedule( showTask, time );
        }
    }

    public void Enter( Tooltip< T > tooltip )
    {
        _showTooltip = tooltip;

        CancelTask();

        if ( Enabled || tooltip.Always )
        {
            if ( ( _time == 0 ) || tooltip.Instant )
            {
                _showTask.Start();
            }
        }
    }

    public void Hide( Tooltip< T > tooltip )
    {
        _showTooltip = null!;
        CancelTask();

        if ( tooltip.Container.HasParent() )
        {
            _activeTooltips.Remove( tooltip );
            HideAction( tooltip );
        }
    }

    /// <summary>
    /// Called when tooltip is shown. Default implementation sets actions to animate showing.
    /// </summary>
    protected void ShowAction( Tooltip< T > tooltip )
    {
        var actionTime = Animations ? _time > 0 ? 0.5f : 0.15f : 0.1f;

        tooltip.Container.Transform = true;
        tooltip.Container.Color.A   = 0.2f;
        tooltip.Container.SetScale( 0.05f );

        tooltip.Container.AddAction
            (
             Actions.Actions.Parallel
                 (
                  Actions.Actions.FadeIn( actionTime, Interpolation.Fade ),
                  Actions.Actions.ScaleTo( 1, 1, actionTime, Interpolation.Fade )
                 )
            );
    }

    /// <summary>
    /// Called when tooltip is hidden. Default implementation sets actions to animate hiding
    /// and to remove the actor from the stage when the actions are complete. A subclass must
    /// at least remove the actor.
    /// </summary>
    protected static void HideAction( Tooltip< T > tooltip )
    {
        tooltip.Container.AddAction
            (
             Actions.Actions.Sequence
                 (
                  Actions.Actions.Parallel
                      (
                       Actions.Actions.Alpha( 0.2f, 0.2f, Interpolation.Fade ),
                       Actions.Actions.ScaleTo( 0.05f, 0.05f, 0.2f, Interpolation.Fade )
                      ),
                  Actions.Actions.RemoveActor()
                 )
            );
    }

    public void HideAll()
    {
        CancelTask();

        _time        = InitialTime;
        _showTooltip = null!;

        foreach ( Tooltip< T > tooltip in _activeTooltips )
        {
            tooltip.Hide();
        }

        _activeTooltips.Clear();
    }

    /// <summary>
    /// Shows all tooltips on hover without a delay for <see cref="ResetTime"/> seconds.
    /// </summary>
    public void ShowInstantly()
    {
        _time = 0;
        _showTask.Start();
        CancelTask();
    }

    /// <summary>
    /// </summary>
    private void CancelTask()
    {
        if ( _showTask is { Status: TaskStatus.Running } )
        {
            _showTaskCancellationTokenSource.Cancel();
        }
    }

    // ========================================================================

    #region properties

    /// <summary>
    /// The X distance from the mouse position to offset the tooltip actor. Default is 15.
    /// </summary>
    public float OffsetX { get; set; } = 15;

    /// <summary>
    /// The Y distance from the mouse position to offset the tooltip actor. Default is 19.
    /// </summary>
    public float OffsetY { get; set; } = 19;

    /// <summary>
    /// The maximum width of a <see cref="TextTooltip"/>. The label will wrap if needed.
    /// Default is <see cref="int.MaxValue"/>.
    /// </summary>
    public float MaxWidth { get; set; } = int.MaxValue;

    /// <summary>
    /// The distance from the tooltip actor position to the edge of the screen where the
    /// actor will be shown on the other side of the mouse cursor. Default is 7.
    /// </summary>
    public float EdgeDistance { get; set; } = 7;

    /// <summary>
    /// Seconds from when an actor is hovered to when the tooltip is shown. Default is 2.
    /// Call <see cref="HideAll()"/> after changing to reset internal state.
    /// </summary>
    public float InitialTime { get; set; } = 2;

    /// <summary>
    /// Once a tooltip is shown, this is used instead of <see cref="InitialTime"/>.
    /// Default is 0.
    /// </summary>
    public float SubsequentTime { get; set; } = 0;

    /// <summary>
    /// Seconds to use <see cref="SubsequentTime"/>. Default is 1.5f.
    /// </summary>
    public float ResetTime { get; set; } = 1.5f;

    /// <summary>
    /// If false, tooltips will not be shown. Default is true.
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// If false, tooltips will be shown without animations. Default is true.
    /// </summary>
    public bool Animations { get; set; } = true;

    #endregion properties

    // ========================================================================

//    private static TooltipManager _instance = null!;
// 
//    public static TooltipManager Instance()
//    {
//        if ( ( _files == null ) || ( _files != Gdx.Files ) )
//        {
//            _files    = Gdx.Files;
//            _instance = new TooltipManager();
//        }
// 
//        return _instance;
//    }
}
