using System.Text;

namespace LibGDXSharp.Utils.Regex
{
    public class PatternSyntaxException : ArgumentException
    {
        public string  Desc    { get; set; }
        public string? Pattern { get; set; }
        public int     Index   { get; set; }

        public PatternSyntaxException( String desc, String regex, int index )
        {
            this.Desc    = desc;
            this.Pattern = regex;
            this.Index   = index;
        }

        public string GetMessage()
        {
            StringBuilder sb = new();

            sb.Append( Desc );

            if ( Index >= 0 )
            {
                sb.Append( " near index " );
                sb.Append( Index );
            }

            sb.Append( '\n' );
            sb.Append( Pattern );

            if ( ( Index >= 0 ) && ( Pattern != null ) && ( Index < Pattern.Length ) )
            {
                sb.Append( '\n' );

                for ( var i = 0; i < Index; i++ )
                {
                    sb.Append( ' ' );
                }

                sb.Append( '^' );
            }

            return sb.ToString();
        }
    }
}
