using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

/**
The SunBeam spell class.
This spell creates a beam of light that can damage enemies.
*/
public partial class SunBeam : Spell
{
	[Export] public Color BeamColor = new Color(1, 1, 0, 0.7f); ///< The color of the sun beam
	[Export] public int BeamSegments = 100; ///< The number of segments in the beam for smooth rendering
	[Export] public float BeamAngle = 38.0f; ///< The angle of the beam in degrees
	[Export] public float MaxBeamLength = 200.0f; ///< The maximum length of the beam

	private MeshInstance2D meshInstance; ///< The mesh instance for rendering the beam
	private List<RayCast2D> rayCasts = new List<RayCast2D>(); ///< List of raycasts for collision detection
	private CollisionPolygon2D collisionPolygon; ///< The collision polygon for the beam
	private PointLight2D pointLight; ///< The point light for the beam's origin

	/**
	Creates raycasts for collision detection along the beam's edges.
	*/
	private void CreateRayCasts()
	{
		// Create raycasts for left and right edges of each segment
		for (int i = 0; i <= BeamSegments; i++)
		{
			var ray = new RayCast2D();
			ray.CollisionMask = 1 << (4 - 1);
			AddChild(ray);
			rayCasts.Add(ray);
		}
	}

	public override void Init(Attack attack, Vector2 playerPosition, Vector2 targetPosition) 
	{
		base.Init(attack, playerPosition, targetPosition);
		meshInstance = GetNode<MeshInstance2D>("MeshInstance2D");
		pointLight = GetNode<PointLight2D>("FlashLight");
		CreateRayCasts();
		collisionPolygon = new CollisionPolygon2D();
		AddChild(collisionPolygon);

		GlobalPosition = playerPosition;
		RotateLight(playerPosition, targetPosition);
		CallDeferred(nameof(UpdateBeam), targetPosition);
	}

	/**
	Rotates the point light to face the target position.
	@param playerPosition The position of the player casting the spell
	@param targetPosition The target position for the spell
	*/
	private void RotateLight(Vector2 playerPosition, Vector2 targetPosition)
	{
		Vector2 direction = (targetPosition - playerPosition).Normalized();
		pointLight.Rotation = Mathf.Atan2(direction.Y, direction.X);
	}

	/**
	Updates the beam's shape and collision based on the target position.
	@param targetPosition The target position for the spell
	*/
	private void UpdateBeam(Vector2 targetPosition)
	{
		Vector2 direction = (targetPosition - GlobalPosition).Normalized();
		float baseAngle = direction.Angle();
		float halfBeamAngle = Mathf.DegToRad(BeamAngle) / 2;

		List<Vector2> points = new List<Vector2>
		{
			Vector2.Zero // Add the player's position (origin of the beam)
		};

		for (int i = 0; i <= BeamSegments; i++)
		{
			float t = (float)i / BeamSegments;
			float angle = baseAngle - halfBeamAngle + t * Mathf.DegToRad(BeamAngle);
			Vector2 targetPoint = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * MaxBeamLength;

			var ray = rayCasts[i];
			ray.Position = Vector2.Zero;
			ray.TargetPosition = targetPoint;
			
			ray.ForceRaycastUpdate();

			Vector2 actualPoint = ray.IsColliding() ? ray.GetCollisionPoint() - GlobalPosition : targetPoint;
			points.Add(actualPoint);
		}

		UpdateMesh(points);
		UpdateCollisionPolygon(points);
	}

	/**
	Updates the mesh for rendering the beam.
	@param points The list of points defining the beam's shape
	*/
	private void UpdateMesh(List<Vector2> points)
	{
		var arrays = new Godot.Collections.Array();
		arrays.Resize((int)Mesh.ArrayType.Max);

		var vertices = points.ToArray();
		var colors = new Color[vertices.Length];
		var indices = new int[(vertices.Length - 2) * 3];

		for (int i = 0; i < vertices.Length; i++)
		{
			colors[i] = BeamColor;
		}

		int index = 0;
		for (int i = 1; i < vertices.Length - 1; i++)
		{
			indices[index++] = 0; // The center point (player position)
			indices[index++] = i;
			indices[index++] = i + 1;
		}

		arrays[(int)Mesh.ArrayType.Vertex] = vertices;
		arrays[(int)Mesh.ArrayType.Color] = colors;
		arrays[(int)Mesh.ArrayType.Index] = indices;

		/*var arrayMesh = new ArrayMesh();
		arrayMesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

		meshInstance.Mesh = arrayMesh;*/
	}

	/**
	Updates the collision polygon for the beam.
	@param points The list of points defining the beam's shape
	*/
	private void UpdateCollisionPolygon(List<Vector2> points)
	{
		// Create a list for the polygon points
		var polygonPoints = new List<Vector2>();
		
		// Add the center point (origin of the beam)
		polygonPoints.Add(Vector2.Zero);
		
		// Add all other points
		polygonPoints.AddRange(points.GetRange(1, points.Count - 1));
		
		// Remove any duplicate points
		polygonPoints = polygonPoints.Distinct().ToList();

		// Ensure the polygon is convex by using a convex hull algorithm
		polygonPoints = ConvexHull(polygonPoints);

		collisionPolygon.Polygon = polygonPoints.ToArray();
	}

	/**
	Computes the convex hull of a set of points.
	@param points The list of points to compute the convex hull for
	@return The list of points forming the convex hull
	*/
	private List<Vector2> ConvexHull(List<Vector2> points)
	{
		if (points.Count <= 3)
			return points;

		// Sort points lexicographically
		points.Sort((a, b) => a.X != b.X ? a.X.CompareTo(b.X) : a.Y.CompareTo(b.Y));

		List<Vector2> hull = new List<Vector2>();

		// Build lower hull
		foreach (var point in points)
		{
			while (hull.Count >= 2 && Cross(hull[hull.Count - 2], hull[hull.Count - 1], point) <= 0)
				hull.RemoveAt(hull.Count - 1);
			hull.Add(point);
		}

		// Build upper hull
		int lowerHullSize = hull.Count;
		for (int i = points.Count - 2; i >= 0; i--)
		{
			Vector2 point = points[i];
			while (hull.Count > lowerHullSize && Cross(hull[hull.Count - 2], hull[hull.Count - 1], point) <= 0)
				hull.RemoveAt(hull.Count - 1);
			hull.Add(point);
		}

		hull.RemoveAt(hull.Count - 1);  // Last point is the same as the first one

		return hull;
	}

	/**
	Computes the cross product of three points.
	@param o The origin point
	@param a The first point
	@param b The second point
	@return The cross product value
	*/
	private float Cross(Vector2 o, Vector2 a, Vector2 b)
	{
		return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
	}
}
