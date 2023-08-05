// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils.Annotations;

// This attribute is attached to members that are still in development and must be used with care.
// Message is some human readable explanation of what to use
// Error indicates if the compiler should treat usage of such a method as an
//   error. (this would be used if the actual implementation of the obsolete
//   method's implementation had changed).
// DiagnosticId. Represents the ID the compiler will use when reporting a use of the API.
// UrlFormat.The URL that should be used by an IDE for navigating to corresponding documentation. Instead of taking the URL directly,
//   the API takes a format string. This allows having a generic URL that includes the diagnostic ID.
//
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum |
                AttributeTargets.Interface | AttributeTargets.Constructor | AttributeTargets.Method
                | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event
                | AttributeTargets.Delegate,
                Inherited = false)]
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class IncompleteAttribute : Attribute
{
    public IncompleteAttribute()
    {
    }

    public IncompleteAttribute(string? message)
    {
        Message = message;
    }

    public IncompleteAttribute(string? message, bool error)
    {
        Message = message;
        IsError = error;
    }

    public string? Message { get; }

    public bool IsError { get; }

    public string? DiagnosticId { get; set; }

    public string? UrlFormat { get; set; }
}

