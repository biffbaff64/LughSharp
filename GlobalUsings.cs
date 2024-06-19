// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

// ----------------------------------------------------------------------------
// General System

global using System;
global using System.Collections;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Globalization;
global using System.IO;
global using System.IO.Compression;
global using System.Reflection;
global using System.Runtime.InteropServices;
global using System.Runtime.Serialization;
global using System.Text;
global using System.Text.RegularExpressions;

// ----------------------------------------------------------------------------
// Json
global using System.Text.Json;
global using System.Text.Json.Serialization;

// ----------------------------------------------------------------------------
// XML
global using System.Xml;
global using System.Xml.Linq;
global using System.Xml.Resolvers;
global using System.Xml.Schema;
global using System.Xml.Serialization;
global using System.Xml.XPath;
global using System.Xml.Xsl;

// ----------------------------------------------------------------------------
// Jetbrains
global using JetBrains.Annotations;

// ----------------------------------------------------------------------------
// OpenAL support

// ----------------------------------------------------------------------------
// OpenGL / Glfw support
global using DotGL;
global using DotGLFW;
global using GLFW = DotGLFW;

// ----------------------------------------------------------------------------
// LughSharp
global using LughSharp.LibCore.Assets;
global using LughSharp.LibCore.Audio;
global using LughSharp.LibCore.Audio.OpenAL;
global using LughSharp.LibCore.Audio.Maponus;
global using LughSharp.LibCore.Core;
global using LughSharp.LibCore.Graphics;
global using LughSharp.LibCore.Graphics.Cameras;
global using LughSharp.LibCore.Graphics.FrameBuffers;
global using LughSharp.LibCore.Graphics.G2D;
global using LughSharp.LibCore.Graphics.G3D;
global using LughSharp.LibCore.Graphics.GLUtils;
global using LughSharp.LibCore.Graphics.OpenGL;
global using LughSharp.LibCore.Graphics.Profiling;
global using LughSharp.LibCore.Maps;
global using LughSharp.LibCore.Maths;
global using LughSharp.LibCore.Scenes.Scene2D;
global using LughSharp.LibCore.Utils;
global using LughSharp.LibCore.Utils.Buffers;
global using LughSharp.LibCore.Utils.Collections;
global using LughSharp.LibCore.Utils.Pooling;

// ----------------------------------------------------------------------------
global using Vector2 = LughSharp.LibCore.Maths.Vector2;
global using Vector3 = LughSharp.LibCore.Maths.Vector3;

// ----------------------------------------------------------------------------
