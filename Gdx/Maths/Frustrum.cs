namespace LibGDXSharp.Maths
{
    /// <summary>
    /// A truncated rectangular pyramid. Used to define the viewable
    /// region and its projection onto the screen.
    /// </summary>
    public class Frustrum
    {
        protected static readonly Vector3[] ClipSpacePlanePoints = new Vector3[]
        {
            new Vector3(-1, -1, -1),
            new Vector3(1, -1, -1),
            new Vector3(1, 1, -1),
            new Vector3(-1, 1, -1),
            new Vector3(-1, -1, 1),
            new Vector3(1, -1, 1),
            new Vector3(1, 1, 1),
            new Vector3(-1, 1, 1)
        };

        protected static readonly float[] ClipSpacePlanePointsArray = new float[8 * 3];

        static Frustrum()
        {
            int j = 0;
            
            foreach (Vector3 v in ClipSpacePlanePoints )
            {
                ClipSpacePlanePointsArray[ j++ ] = v.X;
                ClipSpacePlanePointsArray[ j++ ] = v.Y;
                ClipSpacePlanePointsArray[ j++ ] = v.Z;
            }
        }
	
        private readonly static Vector3 tmpV = new Vector3();

        /// <system>
        /// the six clipping planes, near, far, left, right, top, bottom
        /// </system>
        public Plane[] Planes { get; set; } = new Plane[6];

        /// <system>
        /// eight points making up the near and far clipping "rectangles".
        /// Order is counter clockwise, starting at bottom left.
        /// </system>
        public readonly Vector3[] planePoints =
        {
            new Vector3(), new Vector3(), new Vector3(), new Vector3(),
            new Vector3(), new Vector3(), new Vector3(), new Vector3()
        };

        protected readonly float[] planePointsArray = new float[ 8 * 3 ];

        public Frustrum()
        {
            for ( int i = 0; i < 6; i++ )
            {
                this.Planes[i] = new Plane( new Vector3(), 0 );
            }
        }

        /// <summary>
        /// Updates the clipping plane's based on the given inverse combined
        /// projection and view matrix, e.g. from an <see cref="OrthographicCamera"/>
        /// or <see cref="PerspectiveCamera"/>.
        /// </summary>
        /// <param name="inverseProjectionView">The combined projection and view matrices.</param>
        public virtual void Update(Matrix4 inverseProjectionView)
        {
            Array.Copy( ClipSpacePlanePointsArray, 0, planePointsArray, 0, ClipSpacePlanePointsArray.Length );

            Matrix4.Prj(inverseProjectionView.val, planePointsArray, 0, 8, 3);
            
            for (int i = 0, j = 0; i < 8; i++)
            {
                Vector3 v = planePoints[ i ];
            
                v.X = planePointsArray[ j++ ];
                v.Y = planePointsArray[ j++ ];
                v.Z = planePointsArray[ j++ ];
            }

            Planes[0].Set( planePoints[ 1 ], planePoints[ 0 ], planePoints[ 2 ] );
            Planes[1].Set( planePoints[ 4 ], planePoints[ 5 ], planePoints[ 7 ] );
            Planes[2].Set( planePoints[ 0 ], planePoints[ 4 ], planePoints[ 3 ] );
            Planes[3].Set( planePoints[ 5 ], planePoints[ 1 ], planePoints[ 6 ] );
            Planes[4].Set( planePoints[ 2 ], planePoints[ 3 ], planePoints[ 6 ] );
            Planes[5].Set( planePoints[ 4 ], planePoints[ 0 ], planePoints[ 1 ] );
        }

        /// <summary>
        /// Returns whether the point is in the frustum.
        /// </summary>
        /// <param name="point"> The point </param>
        /// <returns> Whether the point is in the frustum.  </returns>
        public virtual bool PointInFrustum(Vector3 point)
        {
            for (int i = 0; i < planes.length; i++)
            {
                PlaneSide result = planes[i].testPoint(point);
                if (result == PlaneSide.Back)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


