using Godot;
using System;

public partial class Player : CharacterBody3D
{
    private const float MoveSpeed = 7.0f;
    private const float MouseSens = 0.3f;

    private Sprite2D PunchSprite;
    private Area3D PunchArea;
    private RayCast3D Raycast;

    public override void _Ready()
    {
        PunchSprite = GetNode<Sprite2D>("CanvasLayer/Control/Punch");
        PunchArea = GetNode<Area3D>("PunchArea");
        Raycast = GetNode<RayCast3D>("RayCast3D");
    }

    public override void _Input(InputEvent ev)
    {
        if (ev is InputEventMouseMotion m) // casting event type
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
                RotationDegrees = new Vector3(
                    Mathf.Clamp(RotationDegrees.X - m.Relative.Y * MouseSens, -90, 90),
                    RotationDegrees.Y - MouseSens * m.Relative.X,
                    RotationDegrees.Z
                );
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 moveVec = new Vector3();
        if (Input.IsActionPressed("move_forwards"))
            moveVec.Z -= 1;
        if (Input.IsActionPressed("move_backwards"))
            moveVec.Z += 1;
        if (Input.IsActionPressed("move_left"))
            moveVec.X -= 1;
        if (Input.IsActionPressed("move_right"))
            moveVec.X += 1;
        Velocity = moveVec.Normalized().Rotated(new Vector3(0,1,0), Rotation.Y) * MoveSpeed;
        MoveAndSlide();

        bool punched = Input.IsActionJustPressed("punch_left") || Input.IsActionJustPressed("punch_right");
        if (punched)
        {
            PunchSprite.Show();
            Object col = Raycast.GetCollider();
            if (Raycast.IsColliding() && col is Actor actor)
            {
                if (PunchArea.OverlapsArea(actor.Hitbox))
                {
                    Vector3 to = Raycast.ToGlobal(new Vector3(
                        Raycast.TargetPosition.X,
                        Raycast.TargetPosition.Y+2000,
                        Raycast.TargetPosition.Z
                    ));
                    Vector3 dir = GlobalTransform.Origin.DirectionTo(to);
                    actor.ApplyCentralImpulse(dir.Normalized()*5);
                    actor.Hit = true;
                }
            }
        }
        else
        {
            PunchSprite.Hide();
        }
    }
}
