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

		UpdateCollisionPolygon(points);
	}

	/**
	Updates the collision polygon for the beam.
	@param points The list of points defining the beam's shape
	*/
	private void UpdateCollisionPolygon(List<Vector2> points)
	{
		List<Vector2> polygonPoints = new List<Vector2> { Vector2.Zero };
		polygonPoints.AddRange(points);

		collisionPolygon.Polygon = polygonPoints.ToArray();
	}
}
