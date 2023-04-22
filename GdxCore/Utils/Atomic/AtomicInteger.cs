using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

using LibGDXSharp.Maths;

namespace LibGDXSharp.Utils.Atomic
{
    public class AtomicInteger : Number, ISerializable
    {
        // setup to use Unsafe.compareAndSwapInt for updates
        private readonly static Unsafe _unsafe = Unsafe.GetUnsafe();
        private readonly static long   _valueOffset;

        internal static ImpliedClass()
        {
            try
            {
                _valueOffset = _unsafe.objectFieldOffset( typeof(AtomicInteger).getDeclaredField( "value" ) );
            }
            catch ( Exception ex )
            {
                throw new Exception( ex );
            }
        }

        private volatile int _value;

        /// <inheritdoc />
        public override int IntValue()
        {
            return 0;
        }

        /// <inheritdoc />
        public override long LongValue()
        {
            return 0;
        }

        /// <inheritdoc />
        public override float FloatValue()
        {
            return 0;
        }

        /// <inheritdoc />
        public override double DoubleValue()
        {
            return 0;
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" />
        /// with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
        public void GetObjectData( SerializationInfo info, StreamingContext context )
        {
        }
    }
}
