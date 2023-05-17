using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Files.PlaceHolders;

namespace LibGDXSharp.Files;

/// <summary>
/// An abstract representation of file and directory pathnames.
///
/// <para>
/// User interfaces and operating systems use system-dependent <i>pathname strings</i>
/// to name files and directories. This class presents an abstract, system-independent
/// view of hierarchical pathnames. An <i>abstract pathname</i> has two components:
///
/// <para>
/// 1.   An optional system-dependent <tt>prefix</tt> string, such as a disk-drive
///      specifier, <tt>"/"</tt> for the UNIX root directory, or <tt>"\\\\"</tt> for
///      a Microsoft Windows UNC pathname, and...
/// </para>
/// <para>
/// 2.   A sequence of zero or more string <tt>names</tt>.
/// </para>
/// <para>
/// The first name in an abstract pathname may be a directory name or, in the
/// case of Microsoft Windows UNC pathnames, a hostname.  Each subsequent name
/// in an abstract pathname denotes a directory; the last name may denote
/// either a directory or a file.  The <tt>empty</tt> abstract pathname has no
/// prefix and an empty name sequence.
/// </para>
/// </para>
/// <para>
/// The conversion of a pathname string to or from an abstract pathname is
/// inherently system-dependent.  When an abstract pathname is converted into a
/// pathname string, each name is separated from the next by a single copy of
/// the default <tt>separator character</tt>.  The default name-separator
/// character is defined by the system property <tt>file.separator</tt>, and
/// is made available in the public static fields <tt><see cref="Separator"/></tt>
/// and <tt><see cref="SeparatorChar"/></tt> of this class.
/// </para>
/// <para>
/// When a pathname string is converted into an abstract pathname, the names
/// within it may be separated by the default name-separator character or by any
/// other name-separator character that is supported by the underlying system.
/// </para>
/// <para>
/// A pathname, whether abstract or in string form, may be either <tt>absolute</tt>
/// or <tt>relative</tt>.  An absolute pathname is complete in that no other
/// information is required in order to locate the file that it denotes.  A
/// relative pathname, in contrast, must be interpreted in terms of
/// information taken from some other pathname.  By default the classes in the
/// <tt>java.io</tt> package always resolve relative pathnames against the
/// current user directory.  This directory is named by the system property
/// <tt>user.dir</tt>, and is typically the directory in which the Java
/// virtual machine was invoked.
/// </para>
///
/// <para>
/// The <tt>parent</tt> of an abstract pathname may be obtained by invoking
/// the <see cref="GetParent()"/> method of this class and consists of the pathname's
/// prefix and each name in the pathname's name sequence except for the last.
/// </para>
/// <para>
/// Each directory's absolute pathname is an ancestor of any <tt>File</tt>
/// object with an absolute abstract pathname which begins with the directory's
/// absolute pathname.  For example, the directory denoted by the abstract
/// pathname <tt>"/usr"</tt> is an ancestor of the directory denoted by the
/// pathname <tt>"/usr/local/bin"</tt>.
/// </para>
///
/// <para>
/// The prefix concept is used to handle root directories on UNIX platforms,
/// and drive specifiers, root directories and UNC pathnames on Microsoft Windows
/// platforms, as follows:
/// </para>
///
/// <para>
/// For UNIX platforms, the prefix of an absolute pathname is always <tt>"/"</tt>.
/// Relative pathnames have no prefix.  The abstract pathname denoting the root
/// directory has the prefix <tt>"/"</tt> and an empty name sequence.
/// </para>
///
/// <para>
/// For Microsoft Windows platforms, the prefix of a pathname that contains a drive
/// specifier consists of the drive letter followed by <tt>":"</tt> and
/// possibly followed by <tt>"\\"</tt> if the pathname is absolute.  The
/// prefix of a UNC pathname is <tt>"\\\\"</tt>; the hostname and the share
/// name are the first two names in the name sequence.  A relative pathname that
/// does not specify a drive has no prefix.
/// </para>
///
/// <para>
/// Instances of this class may or may not denote an actual file-system object
/// such as a file or a directory.  If it does denote such an object then that
/// object resides in a <i>partition</i>.  A partition is an operating system-
/// specific portion of storage for a file system.  A single storage device (e.g.
/// a physical disk-drive, flash memory, CD-ROM) may contain multiple partitions.
/// The object, if any, will reside on the partition named by some ancestor of
/// the absolute form of this pathname.
/// </para>
///
/// <para>
/// A file system may implement restrictions to certain operations on the
/// actual file-system object, such as reading, writing, and executing.  These
/// restrictions are collectively known as <i>access permissions</i>.  The file
/// system may have multiple sets of access permissions on a single object.
/// For example, one set may apply to the object's <i>owner</i>, and another
/// may apply to all other users.  The access permissions on an object may
/// cause some methods in this class to fail.
/// </para>
///
/// <para>
/// Instances of the <tt>File</tt> class are immutable; that is, once created,
/// the abstract pathname represented by a <tt>File</tt> object will never change.
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class File
{
    /// <summary>
    /// The FileSystem object representing the platform's local file system.
    /// </summary>
    private readonly static FileSystem? fs = DefaultFileSystem.GetFileSystem();

    /// <summary>
    /// This abstract pathname's normalized pathname string. A normalized
    /// pathname string uses the default name-separator character and does not
    /// contain any duplicate or redundant separators.
    /// </summary>
    private readonly string _path;

    /// <summary>
    /// Enum type that indicates the status of a file path.
    /// </summary>
    private enum PathStatus
    {
        Invalid,
        Checked,
        Not_Set
    }

    /// <summary>
    /// The flag indicating whether the file path is invalid.
    /// </summary>
    [NonSerialized] private PathStatus _status = PathStatus.Not_Set;

    /// <summary>
    /// Check if the file has an invalid path. Currently, the inspection of
    /// a file path is very limited, and it only covers Nul character check.
    /// Returning true means the path is definitely invalid/garbage. But
    /// returning false does not guarantee that the path is valid.
    /// </summary>
    /// <returns> true if the file path is invalid. </returns>
    public bool Invalid
    {
        get
        {
            if ( _status == PathStatus.Not_Set )
            {
                _status = ( this._path.IndexOf( '\u0000' ) < 0 ) ? PathStatus.Checked : PathStatus.Invalid;
            }

            return _status == PathStatus.Invalid;
        }
    }

    /// <summary>
    /// Returns the length of this abstract pathname's prefix. For use by FileSystem classes.
    /// </summary>
    [field: NonSerialized] public int PrefixLength { get; }

    /// <summary>
    /// The system-dependent default name-separator character.  This field is
    /// initialized to contain the first character of the value of the system
    /// property <tt>file.separator</tt>.  On UNIX systems the value of this
    /// field is <tt>'/'</tt>; on Microsoft Windows systems it is <tt>'\\'</tt>.
    /// </summary>
    public readonly static char SeparatorChar = fs.GetSeparator();

    /// <summary>
    /// The system-dependent default name-separator character, represented as a
    /// string for convenience.  This string contains a single character, namely
    /// <tt><see cref="SeparatorChar"/></tt>.
    /// </summary>
    public readonly static string Separator = "" + SeparatorChar;

    /// <summary>
    /// The system-dependent path-separator character.  This field is
    /// initialized to contain the first character of the value of the system
    /// property <tt>path.separator</tt>.  This character is used to
    /// separate filenames in a sequence of files given as a <tt>path list</tt>.
    /// On UNIX systems, this character is <tt>':'</tt>; on Microsoft Windows systems it
    /// is <tt>';'</tt>.
    /// </summary>
    /// <see cref="java.lang.System.getProperty(java.lang.String)"/>
    public readonly static char PathSeparatorChar = fs.getPathSeparator();

    /// <summary>
    /// The system-dependent path-separator character, represented as a string
    /// for convenience.  This string contains a single character, namely
    /// <tt><see cref="pathSeparatorChar"/></tt>.
    /// </summary>
    public readonly static string PathSeparator = "" + PathSeparatorChar;

    /// <summary>
    /// Internal constructor for already-normalized pathname strings.
    /// </summary>
    private File( string pathname, int prefixLength )
    {
        this._path         = pathname;
        this.PrefixLength = prefixLength;
    }
}
