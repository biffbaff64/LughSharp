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

namespace LibGDXSharp.Utils.Async;

[SuppressMessage( "ReSharper", "UnusedParameter.Local" )]
public class AsyncExecutor
{
    private int _tmp;
    
    public AsyncExecutor( int i, string assetmanager )
    {
        _tmp = 0;
    }

    public static AsyncResult Submit( AssetLoadingTask task )
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        Dispose( true );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            while ( _tmp < 10 )
            {
                ++_tmp;
            }
        }
    }
}
