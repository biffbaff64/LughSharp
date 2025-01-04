// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted; free of charge; to any person obtaining a copy
//  of this software and associated documentation files (the "Software"); to deal
//  in the Software without restriction; including without limitation the rights
//  to use; copy; modify; merge; publish; distribute; sublicense; and/or sell
//  copies of the Software; and to permit persons to whom the Software is
//  furnished to do so; subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS"; WITHOUT WARRANTY OF ANY KIND; EXPRESS OR
//  IMPLIED; INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY;
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM; DAMAGES OR OTHER
//  LIABILITY; WHETHER IN AN ACTION OF CONTRACT; TORT OR OTHERWISE; ARISING FROM;
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace LughSharp.Lugh.Graphics.OpenGL;

[PublicAPI]
public class DebugSeverity
{
    public const int DEBUG_OUTPUT                     = 37600;
    public const int DEBUG_OUTPUT_SYNCHRONOUS         = 33346;
    public const int CONTEXT_FLAG_DEBUG_BIT           = 2;
    public const int MAX_DEBUG_MESSAGE_LENGTH         = 37187;
    public const int MAX_DEBUG_LOGGED_MESSAGES        = 37188;
    public const int DEBUG_LOGGED_MESSAGES            = 37189;
    public const int DEBUG_NEXT_LOGGED_MESSAGE_LENGTH = 33347;
    public const int MAX_DEBUG_GROUP_STACK_DEPTH      = 33388;
    public const int DEBUG_GROUP_STACK_DEPTH          = 33389;
    public const int MAX_LABEL_LENGTH                 = 33512;
    public const int DEBUG_CALLBACK_FUNCTION          = 33348;
    public const int DEBUG_CALLBACK_USER_PARAM        = 33349;
    public const int DEBUG_SOURCE_API                 = 33350;
    public const int DEBUG_SOURCE_WINDOW_SYSTEM       = 33351;
    public const int DEBUG_SOURCE_SHADER_COMPILER     = 33352;
    public const int DEBUG_SOURCE_THIRD_PARTY         = 33353;
    public const int DEBUG_SOURCE_APPLICATION         = 33354;
    public const int DEBUG_SOURCE_OTHER               = 33355;
    public const int DEBUG_TYPE_ERROR                 = 33356;
    public const int DEBUG_TYPE_DEPRECATED_BEHAVIOR   = 33357;
    public const int DEBUG_TYPE_UNDEFINED_BEHAVIOR    = 33358;
    public const int DEBUG_TYPE_PORTABILITY           = 33359;
    public const int DEBUG_TYPE_PERFORMANCE           = 33360;
    public const int DEBUG_TYPE_OTHER                 = 33361;
    public const int DEBUG_TYPE_MARKER                = 33384;
    public const int DEBUG_TYPE_PUSH_GROUP            = 33385;
    public const int DEBUG_TYPE_POP_GROUP             = 33386;
    public const int DEBUG_SEVERITY_HIGH              = 37190;
    public const int DEBUG_SEVERITY_MEDIUM            = 37191;
    public const int DEBUG_SEVERITY_LOW               = 37192;
    public const int DEBUG_SEVERITY_NOTIFICATION      = 33387;
    public const int BUFFER                           = 33504;
    public const int SHADER                           = 33505;
    public const int PROGRAM                          = 33506;
    public const int QUERY                            = 33507;
    public const int PROGRAM_PIPELINE                 = 33508;
    public const int SAMPLER                          = 33510;
    public const int DISPLAY_LIST                     = 33511;
}