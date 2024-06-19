using GdUnit4.Asserts;
using Godot;
using System;

/**
 * @class CameraController
 * @brief This class controls the camera in the game.
 */
public partial class CameraController : Camera2D
{
	[Export]
	/**
	 * @brief Smoothing factor for camera movement.
	 */
	public float SmoothingFactor { get; set; } = 1f;

	[Export]
	/**
	 * @brief Minimum delta for camera movement.
	 */
	public float MinDelta { get; set; } = 0.01f;

	private Node2D Player;
	private RoomHandler RoomHandler;

	private Vector2 targetPosition => CalculateTargetPosition();

	/**
	 * @brief Called when the node enters the scene tree for the first time.
	 * 
	 * Initializes the player and room handler, and sets the initial camera position.
	 */
	public override void _Ready()
	{
		Player = GetTree().GetNodesInGroup("player")[0] as Node2D;
		RoomHandler = GetTree().GetNodesInGroup("room_handler")[0] as RoomHandler;
		JumpToTarget();
		System.Diagnostics.Debug.Assert(Player is not null);
		RoomHandler.RoomInitialized += JumpToTarget;
	}

	/**
	 * @brief Called every frame. 'delta' is the elapsed time since the previous frame.
	 * 
	 * Updates the camera position based on the player's position and the room bounds.
	 * 
	 * @param delta The elapsed time since the previous frame.
	 */
	public override void _Process(double delta)
	{
		Vector2 difference = targetPosition - Position;

		if (difference.Length() < MinDelta)
		{
			Position = targetPosition;
		}
		else
		{
			Position = new Vector2(
				Mathf.Lerp(Position.X, targetPosition.X, (float)delta * SmoothingFactor),
				Mathf.Lerp(Position.Y, targetPosition.Y, (float)delta * SmoothingFactor)
			);
		}
	}

	/**
	 * @brief Calculates the target position for the camera.
	 * 
	 * @return The target position for the camera.
	 */
	private Vector2 CalculateTargetPosition()
	{
		Rect2 roomBounds = RoomHandler.GetCurrentRoomBounds();
		Rect2 cameraRect = GetCameraRect();

		Vector2 targetPosition = Player.Position;

		targetPosition.X = Mathf.Clamp(targetPosition.X, roomBounds.Position.X + cameraRect.Size.X / 2, roomBounds.End.X - cameraRect.Size.X / 2);
		targetPosition.Y = Mathf.Clamp(targetPosition.Y, roomBounds.Position.Y + cameraRect.Size.Y / 2, roomBounds.End.Y - cameraRect.Size.Y / 2);

		return targetPosition;
	}
	
	/**
	 * @brief Jumps the camera to the target position.
	 */
	private void JumpToTarget()
	{
		Position = CalculateTargetPosition();
	}

	/**
	 * @brief Gets the camera rectangle, i.e. the rectangle of the camera in the world space.
	 * 
	 * @return The camera rectangle.
	 */
	private Rect2 GetCameraRect()
	{
		Transform2D t = GetCanvasTransform();
		Rect2 viewportRect = GetViewportRect();
		return t.AffineInverse() * viewportRect;
	}
}
