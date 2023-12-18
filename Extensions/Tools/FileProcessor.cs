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

using LibGDXSharp.Core.Utils.Collections;
using LibGDXSharp.Extensions.Tools;

namespace LibGDXSharp.Extensions.Tools;

/// <summary>
/// Collects files recursively, filtering by file name. Callbacks are provided to
/// process files and the results are collected, either <see cref="ProcessFile(Entry)"/>
/// or <see cref="ProcessDir(Entry, List)"/> can be overridden, or both. The
/// entries provided to the callbacks have the original file, the output directory,
/// and the output file. If <see cref="SetFlattenOutput(bool)"/> is false, the output
/// will match the directory structure of the input.
/// </summary>
[PublicAPI]
public class FileProcessor
{
    FilenameFilter inputFilter;

//    Comparator< File > comparator = new Comparator< File >()
//    {
//        public int compare (File o1, File o2)
//        {
//            return o1.getName().compareTo(o2.getName());
//        }
//    }

    List< Pattern > inputRegex = new();
    string          outputSuffix;
    List< Entry >   outputFiles = new();
    bool            recursive   = true;
    bool            flattenOutput;

//    Comparator< Entry > entryComparator = new Comparator< Entry >()
//    {
//        public int compare (Entry o1, Entry o2)
//        {
//            return comparator.compare(o1.inputFile, o2.inputFile);
//        }
//    }

    public FileProcessor()
    {
    }

    /** Copy constructor. */
    public FileProcessor( FileProcessor processor )
    {
        inputFilter = processor.inputFilter;
        comparator  = processor.comparator;
        inputRegex.addAll( processor.inputRegex );
        outputSuffix  = processor.outputSuffix;
        recursive     = processor.recursive;
        flattenOutput = processor.flattenOutput;
    }

    public FileProcessor setInputFilter( FilenameFilter inputFilter )
    {
        this.inputFilter = inputFilter;

        return this;
    }

    /** Sets the comparator for {@link #processDir(Entry, List)}. By default the files are sorted by alpha. */
    public FileProcessor setComparator( Comparator< File > comparator )
    {
        this.comparator = comparator;

        return this;
    }

    /** Adds a case insensitive suffix for matching input files. */
    public FileProcessor addInputSuffix( params string[] suffixes )
    {
        foreach ( string suffix in suffixes )
        {
            addInputRegex( "(?i).*" + Pattern.quote( suffix ) );
        }

        return this;
    }

    public FileProcessor addInputRegex( params string[] regexes )
    {
        foreach ( string regex in regexes )
        {
            inputRegex.add( Pattern.compile( regex ) );
        }

        return this;
    }

    /** Sets the suffix for output files, replacing the extension of the input file. */
    public FileProcessor setOutputSuffix( string outputSuffix )
    {
        this.outputSuffix = outputSuffix;

        return this;
    }

    public FileProcessor setFlattenOutput( bool flattenOutput )
    {
        this.flattenOutput = flattenOutput;

        return this;
    }

    /** Default is true. */
    public FileProcessor setRecursive( bool recursive )
    {
        this.recursive = recursive;

        return this;
    }

    /** @param outputRoot May be null.
     * @see #process(File, File) */
    public List< Entry > process( string inputFileOrDir, string outputRoot )
    {
        return process( new File( inputFileOrDir ), outputRoot == null ? null : new File( outputRoot ) );
    }

    /** Processes the specified input file or directory.
     * @param outputRoot May be null if there is no output from processing the files.
     * @return the processed files added with {@link #addProcessedFile(Entry)}. */
    public List< Entry > process( File inputFileOrDir, File outputRoot )
    {
        if ( !inputFileOrDir.exists() )
        {
            throw new ArgumentException( "Input file does not exist: " + inputFileOrDir.getAbsolutePath() );
        }

        if ( inputFileOrDir.isFile() )
        {
            return process( new File[] { inputFileOrDir }, outputRoot );
        }
        else
        {
            return process( inputFileOrDir.listFiles(), outputRoot );
        }
    }

    /** Processes the specified input files.
     * @param outputRoot May be null if there is no output from processing the files.
     * @return the processed files added with {@link #addProcessedFile(Entry)}. */
    public List< Entry > process( File[] files, File outputRoot )
    {
        if ( outputRoot == null )
        {
            outputRoot = new File( "" );
        }

        outputFiles.clear();

        Dictionary< File, List< Entry > > dirToEntries = new();
        process( files, outputRoot, outputRoot, dirToEntries, 0 );

        List< Entry > allEntries = new();
        for ( Map.Entry < File, List < Entry >> mapEntry :
        {
            dirToEntries.entrySet())
        }

        {
            List< Entry > dirEntries = mapEntry.getValue();
            if ( comparator != null )
            {
                Collections.sort( dirEntries, entryComparator );
            }

            File inputDir     = mapEntry.getKey();
            File newOutputDir = null;

            if ( flattenOutput )
            {
                newOutputDir = outputRoot;
            }
            else if ( !dirEntries.isEmpty() ) //
            {
                newOutputDir = dirEntries.get( 0 ).outputDir;
            }

            string outputName                      = inputDir.getName();
            if ( outputSuffix != null )
            {
                outputName = outputName.replaceAll( "(.*)\\..*", "$1" ) + outputSuffix;
            }

            Entry entry = new Entry();
            entry.inputFile = mapEntry.getKey();
            entry.outputDir = newOutputDir;

            if ( newOutputDir != null )
            {
                entry.outputFile = newOutputDir.length() == 0 ? new File( outputName ) : new File( newOutputDir, outputName );
            }

            try
            {
                processDir( entry, dirEntries );
            }
            catch ( Exception ex )
            {
                throw new Exception( "Error processing directory: " + entry.inputFile.getAbsolutePath(), ex );
            }

            allEntries.addAll( dirEntries );
        }

        if ( comparator != null )
        {
            Collections.sort( allEntries, entryComparator );
        }

        for ( Entry entry :
        allEntries) {
            try
            {
                processFile( entry );
            }
            catch ( Exception ex )
            {
                throw new Exception( "Error processing file: " + entry.inputFile.getAbsolutePath(), ex );
            }
        }

        return outputFiles;
    }

    private void process( File[] files,
                          File outputRoot,
                          File outputDir,
                          LinkedHashMap< File, List< Entry > > dirToEntries,
                          int depth )
    {
        // Store empty entries for every directory.
        for ( File file :
        files) {
            File               dir     = file.getParentFile();
            List< Entry > entries = dirToEntries.get( dir );

            if ( entries == null )
            {
                entries = new List();
                dirToEntries.put( dir, entries );
            }
        }

        for ( File file :
        files) {
            if ( file.isFile() )
            {
                if ( inputRegex.size > 0 )
                {
                    bool found = false;
                    for ( Pattern pattern :
                    inputRegex) {
                        if ( pattern.matcher( file.getName() ).matches() )
                        {
                            found = true;

                            break;
                        }
                    }

                    if ( !found )
                    {
                        continue;
                    }
                }

                File dir = file.getParentFile();

                if ( inputFilter != null && !inputFilter.accept( dir, file.getName() ) )
                {
                    continue;
                }

                string outputName                      = file.getName();
                if ( outputSuffix != null )
                {
                    outputName = outputName.replaceAll( "(.*)\\..*", "$1" ) + outputSuffix;
                }

                Entry entry = new Entry();
                entry.depth     = depth;
                entry.inputFile = file;
                entry.outputDir = outputDir;

                if ( flattenOutput )
                {
                    entry.outputFile = new File( outputRoot, outputName );
                }
                else
                {
                    entry.outputFile = new File( outputDir, outputName );
                }

                dirToEntries.get( dir ).add( entry );
            }

            if ( recursive && file.isDirectory() )
            {
                File subdir = outputDir.getPath().length() == 0 ? new File( file.getName() ) : new File( outputDir, file.getName() );
                process( file.listFiles( inputFilter ), outputRoot, subdir, dirToEntries, depth + 1 );
            }
        }
    }

    /** Called with each input file. */
    protected void processFile( Entry entry )
    {
    }

    /** Called for each input directory. The files will be {@link #setComparator(Comparator) sorted}. The specified files list can
     * be modified to change which files are processed. */
    protected void processDir( Entry entryDir, List< Entry > files )
    {
    }

    /** This method should be called by {@link #processFile(Entry)} or {@link #processDir(Entry, List)} if the return value of
     * {@link #process(File, File)} or {@link #process(File[], File)} should return all the processed files. */
    protected void addProcessedFile( Entry entry )
    {
        outputFiles.add( entry );
    }

    /** @author Nathan Sweet */
    [PublicAPI]
    public class Entry
    {
        public File inputFile;
        /** May be null. */
        public File outputDir;
        public File outputFile;
        public int  depth;

        public Entry()
        {
        }

        public Entry( File inputFile, File outputFile )
        {
            this.inputFile  = inputFile;
            this.outputFile = outputFile;
        }

        public string tostring()
        {
            return inputFile.tostring();
        }
    }

}
