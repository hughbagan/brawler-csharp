using Godot;
using System;

public class Player : KinematicBody
{
    private const float MoveSpeed = 7.0f;
    private const float MouseSens = 0.3f;

    private Sprite PunchSprite;
    private Area PunchArea;
    private RayCast Raycast;

    public override void _Ready()
    {
        PunchSprite = GetNode<Sprite>("CanvasLayer/Control/Punch");
        PunchArea = GetNode<Area>("PunchArea");
        Raycast = GetNode<RayCast>("RayCast");
    }

    public override void _Input(InputEvent ev)
    {
        if (ev is InputEventMouseMotion m) // casting event type
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
                RotationDegrees = new Vector3(
                    Mathf.Clamp(RotationDegrees.x - m.Relative.y * MouseSens, -90, 90),
                    RotationDegrees.y - MouseSens * m.Relative.x,
                    RotationDegrees.z
                );
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector3 moveVec = new Vector3();
        if (Input.IsActionPressed("move_forwards"))
            moveVec.z -= 1;
        if (Input.IsActionPressed("move_backwards"))
            moveVec.z += 1;
        if (Input.IsActionPressed("move_left"))
            moveVec.x -= 1;
        if (Input.IsActionPressed("move_right"))
            moveVec.x += 1;
        Vector3 velocity = new Vector3();
        velocity = moveVec.Normalized().Rotated(new Vector3(0,1,0), Rotation.y) * MoveSpeed;
        MoveAndSlide(velocity);

        bool punched = Input.IsActionJustPressed("punch_left") || Input.IsActionJustPressed("punch_right");
        if (punched)
        {
            PunchSprite.Show();
            Godot.Object col = Raycast.GetCollider();
            if (Raycast.IsColliding() && col is Actor actor)
            {
                if (PunchArea.OverlapsArea(actor.Hitbox))
                {
                    Vector3 to = Raycast.ToGlobal(new Vector3(
                        Raycast.CastTo.x,
                        Raycast.CastTo.y+2000,
                        Raycast.CastTo.z
                    ));
                    Vector3 dir = GlobalTransform.origin.DirectionTo(to);
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
