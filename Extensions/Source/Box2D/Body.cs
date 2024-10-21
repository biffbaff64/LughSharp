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


namespace Extensions.Source.Box2D;

/// <summary>
/// A rigid body. These are created via World.CreateBody.
/// </summary>
public class Body
{
//    /// <summary>
//    /// the address of the body
//    /// </summary>
//    protected long Addr { get; set; }
//
//    /// <summary>
//    /// temporary float array
//    /// </summary>
//    private readonly float[] _tmp = new float[ 4 ];
//
//    /// <summary>
//    /// The Box2D World
//    /// </summary>
//    private readonly World _world;
//
//    /// <summary>
//    /// Fixtures of this body
//    /// </summary>
//    private List< Fixture > _fixtures = new List< Fixture >( 2 );
//
//    /// <summary>
//    /// Joints of this body
//    /// </summary>
//    protected internal Array< JointEdge > Joints = new Array< JointEdge >( 2 );
//
//    /// <summary>
//    /// user data
//    /// </summary>
//    private object _userData;
//
//    /// <summary>
//    /// Constructs a new body with the given address
//    /// </summary>
//    /// <param name="world"> the world </param>
//    /// <param name="addr"> the address  </param>
//    protected internal Body( World world, long addr )
//    {
//        this._world = world;
//        this.Addr   = addr;
//    }
//
//    /// <summary>
//    ///   Resets this body after fetching it from the <see cref="World.freeBodies"/> Pool.
//    /// </summary>
//    protected internal virtual void Reset( long addr )
//    {
//        this.Addr      = addr;
//        this._userData = null;
//
//        for ( int i = 0; i < _fixtures.size; i++ )
//        {
//            this._world.freeFixtures.free( _fixtures.get( i ) );
//        }
//
//        _fixtures.clear();
//        this.Joints.clear();
//    }
//
//    /// <summary>
//    /// Creates a fixture and attach it to this body. Use this function if you need to set some fixture parameters, like friction.
//    /// Otherwise you can create the fixture directly from a shape. If the density is non-zero, this function automatically updates
//    /// the mass of the body. Contacts are not created until the next time step.
//    /// </summary>
//    /// <param name="def"> the fixture definition.
//    /// @warning This function is locked during callbacks.  </param>
//    public virtual Fixture CreateFixture( FixtureDef def )
//    {
//        long fixtureAddr = jniCreateFixture
//            (
//             addr,
//             def.shape.addr,
//             def.friction,
//             def.restitution,
//             def.density,
//             def.isSensor,
//             def.filter.categoryBits,
//             def.filter.maskBits,
//             def.filter.groupIndex
//            );
//
//        Fixture fixture = this.world.freeFixtures.obtain();
//        fixture.reset( this, fixtureAddr );
//        this.world.fixtures.put( fixture.addr, fixture );
//        this.fixtures.add( fixture );
//
//        return fixture;
//    }
//
////JAVA TO C# CONVERTER TASK: Replace 'unknown' with the appropriate dll name:
//    [DllImport( "unknown" )]
//    private extern long JniCreateFixture( long addr,
//                                          long shapeAddr,
//                                          float friction,
//                                          float restitution,
//                                          float density,
//                                          bool isSensor,
//                                          short filterCategoryBits,
//                                          short filterMaskBits,
//                                          short filterGroupIndex );
//
//    b2Body*      body  = ( b2Body* )addr;
//    b2Shape*     shape = ( b2Shape* )shapeAddr;
//    b2FixtureDef fixtureDef;
//
//    fixtureDef.shape = shape;
//    fixtureDef.friction = friction;
//    fixtureDef.restitution = restitution;
//    fixtureDef.density = density;
//    fixtureDef.isSensor = isSensor;
//    fixtureDef.filter.maskBits = filterMaskBits;
//    fixtureDef.filter.categoryBits = filterCategoryBits;
//    fixtureDef.filter.groupIndex = filterGroupIndex;
//
//    return (jlong) body->CreateFixture( &fixtureDef );
//    */
//
//    /// <summary>
//    /// Creates a fixture from a shape and attach it to this body. This is a convenience function. Use b2FixtureDef if you need to
//    /// set parameters like friction, restitution, user data, or filtering. If the density is non-zero, this function automatically
//    /// updates the mass of the body. </summary>
//    /// <param name="shape"> the shape to be cloned. </param>
//    /// <param name="density"> the shape density (set to zero for static bodies).
//    /// @warning This function is locked during callbacks.  </param>
//    public virtual Fixture CreateFixture( Shape shape, float density )
//    {
//        long    fixtureAddr = jniCreateFixture( addr, shape.addr, density );
//        Fixture fixture     = this.world.freeFixtures.obtain();
//        fixture.reset( this, fixtureAddr );
//        this.world.fixtures.put( fixture.addr, fixture );
//        this.fixtures.add( fixture );
//
//        return fixture;
//    }
//
////JAVA TO C# CONVERTER TASK: Replace 'unknown' with the appropriate dll name:
//    [DllImport( "unknown" )]
//    private extern long JniCreateFixture( long addr, long shapeAddr, float density );
//
//    b2Body* body = ( b2Body* )addr;
//    b2Shape* shape = ( b2Shape* )shapeAddr;
//        return (jlong) body->CreateFixture( shape, density );
//        */
//
//    /// <summary>
//    /// Destroy a fixture. This removes the fixture from the broad-phase and destroys all contacts associated with this fixture.
//    /// This will automatically adjust the mass of the body if the body is dynamic and the fixture has positive density. All
//    /// fixtures attached to a body are implicitly destroyed when the body is destroyed. </summary>
//    /// <param name="fixture"> the fixture to be removed.
//    /// @warning This function is locked during callbacks.  </param>
//    public virtual void DestroyFixture( Fixture fixture )
//    {
//        this.world.destroyFixture( this, fixture );
//        fixture.setUserData( null );
//        this.world.fixtures.remove( fixture.addr );
//        this.fixtures.removeValue( fixture, true );
//        this.world.freeFixtures.free( fixture );
//    }
//
//    /// <summary>
//    /// Set the position of the body's origin and rotation. This breaks any contacts and wakes the other bodies. Manipulating a
//    /// body's transform may cause non-physical behavior. </summary>
//    /// <param name="position"> the world position of the body's local origin. </param>
//    /// <param name="angle"> the world rotation in radians.  </param>
//    public virtual void SetTransform( Vector2 position, float angle )
//    {
//        jniSetTransform( addr, position.x, position.y, angle );
//    }
//
//    /// <summary>
//    /// Set the position of the body's origin and rotation. This breaks any contacts and wakes the other bodies. Manipulating a
//    /// body's transform may cause non-physical behavior. </summary>
//    /// <param name="x"> the world position on the x-axis </param>
//    /// <param name="y"> the world position on the y-axis </param>
//    /// <param name="angle"> the world rotation in radians.  </param>
//    public virtual void SetTransform( float x, float y, float angle )
//    {
//        jniSetTransform( addr, x, y, angle );
//    }
//
////JAVA TO C# CONVERTER TASK: Replace 'unknown' with the appropriate dll name:
//    [DllImport( "unknown" )]
//    private extern void JniSetTransform( long addr, float positionX, float positionY, float angle );
//
//    b2Body* body = ( b2Body* )addr;
//    body->SetTransform( b2Vec2(positionX, positionY), angle);
//    */
//
//    private readonly Transform _transform = new Transform();
//
//    /// <summary>
//    /// Get the body transform for the body's origin. </summary>
//    public virtual Transform Transform
//    {
//        get
//        {
//            jniGetTransform( addr, _transform.vals );
//
//            return _transform;
//        }
//    }
}
