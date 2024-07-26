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
	private float StartZoom;

	private Vector2 targetPosition => CalculateTargetPosition();

	/**
	 * @brief Called when the node enters the scene tree for the first time.
	 * 
	 * Initializes the player and room handler, and sets the initial camera position.
	 */
	public override void _Ready()
	{
		StartZoom = Zoom.X;
		Player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		RoomHandler = GetTree().GetFirstNodeInGroup("room_handler") as RoomHandler;
		JumpToTarget();
		System.Diagnostics.Debug.Assert(Player is not null);
		System.Diagnostics.Debug.Assert(RoomHandler is not null);
		RoomHandler.RoomInitialized += JumpToTarget;
	}

	/**
	 * @brief Called every frame. 'delta' is the elapsed time since the previous frame.
	 * 
	 * Updates the camera position based on the player's position and the current roomtype.
	 * 
	 * @param delta The elapsed time since the previous frame.
	 */
	public override void _Process(double delta)
	{
		System.Diagnostics.Debug.Assert(RoomHandler is not null && RoomHandler.CurrentRoom is not null);
		switch(RoomHandler.CurrentRoom.Type) {
			case RoomType.Normal:
				ProcessNormalRoom(delta);
				break;
			case RoomType.Boss:
				ProcessBossRoom(delta);
				break;
		}
	}

	/**
	 * @brief Processes the camera movement in a normal room.
	 * 
	 * @param delta The elapsed time since the previous frame.
	 */
	private void ProcessNormalRoom(double delta)
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
	 * @brief Processes the camera movement in a boss room.
	 * 
	 * @param delta The elapsed time since the previous frame.
	 */
	private void ProcessBossRoom(double delta)
	{
		Vector2 roomCenter = RoomHandler.GetCurrentRoomBounds().GetCenter();
		Position = Position.Lerp(roomCenter, (float)delta * SmoothingFactor);
		//Zoom = Vector2.One * GetBossRoomZoom();
	}

	/**
	 * @brief Calculates the zoom level for the camera in a boss room based on the room bounds.
	 * 
	 * @return The zoom level for the camera in a boss room.
	 */
	private float GetBossRoomZoom()
	{
		Rect2 roomBounds = RoomHandler.GetCurrentRoomBounds();
		Vector2 roomSize = roomBounds.Size;
		Vector2 viewportSize = GetViewportRect().Size;
		float zoomX = viewportSize.X / roomSize.X;
		float zoomY = viewportSize.Y / roomSize.Y;
		return Mathf.Min(zoomX, zoomY);
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

		// Calculate the maximum allowed camera positions
		float minX = roomBounds.Position.X + cameraRect.Size.X / 2;
		float maxX = roomBounds.End.X - cameraRect.Size.X / 2;
		float minY = roomBounds.Position.Y + cameraRect.Size.Y / 2;
		float maxY = roomBounds.End.Y - cameraRect.Size.Y / 2;

		// Ensure that minX is always less than maxX, and minY is always less than maxY
		if (minX > maxX)
		{
			float centerX = (roomBounds.Position.X + roomBounds.End.X) / 2;
			minX = maxX = centerX;
		}
		if (minY > maxY)
		{
			float centerY = (roomBounds.Position.Y + roomBounds.End.Y) / 2;
			minY = maxY = centerY;
		}

		targetPosition.X = Mathf.Clamp(targetPosition.X, minX, maxX);
		targetPosition.Y = Mathf.Clamp(targetPosition.Y, minY, maxY);

		return targetPosition;
	}
	
	/**
	 * @brief Jumps the camera to the target position.
	 */
	private void JumpToTarget()
	{
		switch(RoomHandler.CurrentRoom.Type) {
			case RoomType.Normal:
				Zoom = Vector2.One * StartZoom;
				Position = CalculateTargetPosition();
				break;
			case RoomType.Boss:
				Zoom = Vector2.One * GetBossRoomZoom();
				Position = RoomHandler.GetCurrentRoomBounds().GetCenter();
				break;
		}
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
