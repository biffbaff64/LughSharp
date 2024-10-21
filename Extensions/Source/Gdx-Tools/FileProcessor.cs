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

using System.Diagnostics;
using JetBrains.Annotations;

namespace Extensions.Source.Gdx_Tools;

///// <summary>
/////     Collects files recursively, filtering by file name. Callbacks are provided to
/////     process files and the results are collected, either <see cref="ProcessFile(Entry)" />
/////     or <see cref="ProcessDir(Entry, List)" /> can be overridden, or both. The
/////     entries provided to the callbacks have the original file, the output directory,
/////     and the output file. If <see cref="SetFlattenOutput(bool)" /> is false, the output
/////     will match the directory structure of the input.
///// </summary>
[PublicAPI]
public class FileProcessor
{
//    FilenameFilter inputFilter;
//
////    Comparator< File > comparator = new Comparator< File >()
////    {
////        public int compare (File o1, File o2)
////        {
////            return o1.getName().compareTo(o2.getName());
////        }
////    }

//    private List< Pattern > inputRegex = new();
    private readonly List< Entry > _outputFiles = new();
    private          bool          _flattenOutput;
    private          string        _outputSuffix;
    private          bool          _recursive;

////    Comparator< Entry > entryComparator = new Comparator< Entry >()
////    {
////        public int compare (Entry o1, Entry o2)
////        {
////            return comparator.compare(o1.inputFile, o2.inputFile);
////        }
////    }

    public FileProcessor()
    {
        _outputSuffix  = string.Empty;
        _flattenOutput = false;

        SetRecursive();
    }

//    /** Copy constructor. */
    public FileProcessor( FileProcessor processor )
    {
//        _inputFilter = processor.inputFilter;
//        _comparator  = processor.comparator;
//        _inputRegex.addAll( processor.inputRegex );
        _outputSuffix  = processor._outputSuffix;
        _recursive     = processor._recursive;
        _flattenOutput = processor._flattenOutput;
    }

//
//    public FileProcessor setInputFilter( FilenameFilter inputFilter )
//    {
//        this.inputFilter = inputFilter;
//
//        return this;
//    }
//
//    /** Sets the comparator for {@link #processDir(Entry, List)}. By default the files are sorted by alpha. */
//    public FileProcessor setComparator( Comparator< File > comparator )
//    {
//        this.comparator = comparator;
//
//        return this;
//    }
//
//    /** Adds a case insensitive suffix for matching input files. */
//    public FileProcessor addInputSuffix( params string[] suffixes )
//    {
//        foreach ( string suffix in suffixes )
//        {
//            addInputRegex( "(?i).*" + Pattern.quote( suffix ) );
//        }
//
//        return this;
//    }
//
//    public FileProcessor addInputRegex( params string[] regexes )
//    {
//        foreach ( string regex in regexes )
//        {
//            inputRegex.add( Pattern.compile( regex ) );
//        }
//
//        return this;
//    }
//
//    /** Sets the suffix for output files, replacing the extension of the input file. */
//    public FileProcessor setOutputSuffix( string outputSuffix )
//    {
//        this.outputSuffix = outputSuffix;
//
//        return this;
//    }
//
//    public FileProcessor setFlattenOutput( bool flattenOutput )
//    {
//        this.flattenOutput = flattenOutput;
//
//        return this;
//    }

    public FileProcessor SetRecursive( bool recursive = true )
    {
        _recursive = recursive;

        return this;
    }

//    /** @param outputRoot May be null.
//     * @see #process(File, File) */
//    public List< Entry > process( string inputFileOrDir, string outputRoot )
//    {
//        return process( new File( inputFileOrDir ), outputRoot == null ? null : new File( outputRoot ) );
//    }
//
//    /** Processes the specified input file or directory.
//     * @param outputRoot May be null if there is no output from processing the files.
//     * @return the processed files added with {@link #addProcessedFile(Entry)}. */
//    public List< Entry > process( File inputFileOrDir, File outputRoot )
//    {
//        if ( !inputFileOrDir.exists() )
//        {
//            throw new ArgumentException( "Input file does not exist: " + inputFileOrDir.getAbsolutePath() );
//        }
//
//        if ( inputFileOrDir.isFile() )
//        {
//            return process( new File[] { inputFileOrDir }, outputRoot );
//        }
//        else
//        {
//            return process( inputFileOrDir.listFiles(), outputRoot );
//        }
//    }
//
//    /** Processes the specified input files.
//     * @param outputRoot May be null if there is no output from processing the files.
//     * @return the processed files added with {@link #addProcessedFile(Entry)}. */
//    public List< Entry > process( File[] files, File outputRoot )
//    {
//        if ( outputRoot == null )
//        {
//            outputRoot = new File( "" );
//        }
//
//        outputFiles.clear();
//
//        Dictionary< File, List< Entry > > dirToEntries = new();
//        process( files, outputRoot, outputRoot, dirToEntries, 0 );
//
//        List< Entry > allEntries = new();
//        for ( Map.Entry < File, List < Entry >> mapEntry :
//        {
//            dirToEntries.entrySet())
//        }
//
//        {
//            List< Entry > dirEntries = mapEntry.getValue();
//            if ( comparator != null )
//            {
//                Collections.sort( dirEntries, entryComparator );
//            }
//
//            File inputDir     = mapEntry.getKey();
//            File newOutputDir = null;
//
//            if ( flattenOutput )
//            {
//                newOutputDir = outputRoot;
//            }
//            else if ( !dirEntries.isEmpty() ) //
//            {
//                newOutputDir = dirEntries.get( 0 ).outputDir;
//            }
//
//            string outputName                      = inputDir.getName();
//            if ( outputSuffix != null )
//            {
//                outputName = outputName.replaceAll( "(.*)\\..*", "$1" ) + outputSuffix;
//            }
//
//            Entry entry = new Entry();
//            entry.inputFile = mapEntry.getKey();
//            entry.outputDir = newOutputDir;
//
//            if ( newOutputDir != null )
//            {
//                entry.outputFile = newOutputDir.length() == 0 ? new File( outputName ) : new File( newOutputDir, outputName );
//            }
//
//            try
//            {
//                processDir( entry, dirEntries );
//            }
//            catch ( Exception ex )
//            {
//                throw new Exception( "Error processing directory: " + entry.inputFile.getAbsolutePath(), ex );
//            }
//
//            allEntries.addAll( dirEntries );
//        }
//
//        if ( comparator != null )
//        {
//            Collections.sort( allEntries, entryComparator );
//        }
//
//        for ( Entry entry :
//        allEntries) {
//            try
//            {
//                processFile( entry );
//            }
//            catch ( Exception ex )
//            {
//                throw new Exception( "Error processing file: " + entry.inputFile.getAbsolutePath(), ex );
//            }
//        }
//
//        return outputFiles;
//    }
//
//    private void process( File[] files,
//                          File outputRoot,
//                          File outputDir,
//                          LinkedHashMap< File, List< Entry > > dirToEntries,
//                          int depth )
//    {
//        // Store empty entries for every directory.
//        for ( File file :
//        files) {
//            File               dir     = file.getParentFile();
//            List< Entry > entries = dirToEntries.get( dir );
//
//            if ( entries == null )
//            {
//                entries = new List();
//                dirToEntries.put( dir, entries );
//            }
//        }
//
//        for ( File file :
//        files) {
//            if ( file.isFile() )
//            {
//                if ( inputRegex.size > 0 )
//                {
//                    bool found = false;
//                    for ( Pattern pattern :
//                    inputRegex) {
//                        if ( pattern.matcher( file.getName() ).matches() )
//                        {
//                            found = true;
//
//                            break;
//                        }
//                    }
//
//                    if ( !found )
//                    {
//                        continue;
//                    }
//                }
//
//                File dir = file.getParentFile();
//
//                if ( inputFilter != null && !inputFilter.accept( dir, file.getName() ) )
//                {
//                    continue;
//                }
//
//                string outputName                      = file.getName();
//                if ( outputSuffix != null )
//                {
//                    outputName = outputName.replaceAll( "(.*)\\..*", "$1" ) + outputSuffix;
//                }
//
//                Entry entry = new Entry();
//                entry.depth     = depth;
//                entry.inputFile = file;
//                entry.outputDir = outputDir;
//
//                if ( flattenOutput )
//                {
//                    entry.outputFile = new File( outputRoot, outputName );
//                }
//                else
//                {
//                    entry.outputFile = new File( outputDir, outputName );
//                }
//
//                dirToEntries.get( dir ).add( entry );
//            }
//
//            if ( recursive && file.isDirectory() )
//            {
//                File subdir = outputDir.getPath().length() == 0 ? new File( file.getName() ) : new File( outputDir, file.getName() );
//                process( file.listFiles( inputFilter ), outputRoot, subdir, dirToEntries, depth + 1 );
//            }
//        }
//    }

    /// <summary>
    /// Called with each input file.
    /// </summary>
    protected void ProcessFile( Entry entry )
    {
    }

    /// <summary>
    /// Called for each input directory. The files will be <see cref="SetComparator(Comparator)"/>
    /// sorted. The specified files list can be modified to change which files are processed.
    /// </summary>
    protected void ProcessDir( Entry entryDir, List< Entry > files )
    {
    }

    /// <summary>
    /// This method should be called by <see cref="ProcessFile(Entry)"/> or <see cref="ProcessDir(Entry, List{})"/>
    /// if the return value of <see cref="Process"/> or <see cref="Process(File[], File)"/> should return
    /// all the processed files.
    /// </summary>
    protected void AddProcessedFile( Entry entry )
    {
        _outputFiles.Add( entry );
    }

    public class Entry
    {
//        public File inputFile;
//        public File outputDir;
//        public File outputFile;
//        public int depth;

//        public Entry( File inputFile, File outputFile )
//        {
//            this.inputFile  = inputFile;
//            this.outputFile = outputFile;
//        }

//        public string tostring()
//        {
//            return inputFile.tostring();
//        }
    }
}
