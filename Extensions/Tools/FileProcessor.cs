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

using System.Collections;

namespace LibGDXSharp.Extensions.Tools;

/// <summary>
/// Collects files recursively, filtering by file name. Callbacks are provided to
/// process files and the results are collected, either <see cref="ProcessFile(Entry)"/>
/// or <see cref="ProcessDir(Entry, ArrayList)"/> can be overridden, or both. The
/// entries provided to the callbacks have the original file, the output directory,
/// and the output file. If <see cref="SetFlattenOutput(bool)"/> is false, the output
/// will match the directory structure of the input.
/// </summary>
[PublicAPI]
public class FileProcessor
{
}
