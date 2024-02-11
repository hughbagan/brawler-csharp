using Godot;
using System;

public partial class Actor : RigidBody3D
{
    public Player PlayerInstance;
    public Area3D Hitbox;
    public bool Hit = false;

    private const float MoveSpeed = 2.0f;
    private NavigationAgent3D Nav;
    private SpotLight3D Eye;

    public override void _Ready()
    {
        base._Ready();
        PlayerInstance = GetNode<Player>("/root/Level/Player");
        Nav = GetNode<NavigationAgent3D>("NavigationAgent3D");
        Hitbox = GetNode<Area3D>("Hitbox");
        Eye = GetNode<SpotLight3D>("SpotLight3D");

        // can't await during _Ready
        Callable.From(ActorSetup).CallDeferred();
    }

    private async void ActorSetup()
    {
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        // Eye.LookAt(GlobalTransform.origin + LinearVelocity, Vector3.Up);
        if (PlayerInstance != null && !Hit)
        {
            SetMovementTarget(PlayerInstance.GlobalPosition);
            if (Nav.IsNavigationFinished())
                return;
            Vector3 newVelocity = GlobalPosition.DirectionTo(Nav.GetNextPathPosition()) * MoveSpeed;
            if (Nav.AvoidanceEnabled)
                Nav.Velocity = newVelocity;
            else
                _OnVelocityComputed(newVelocity);
        }
    }

    private void SetMovementTarget(Vector3 movementTarget)
    {
        Nav.TargetPosition = movementTarget;
        Eye.LookAt(movementTarget, Vector3.Up);
    }

    private void _OnVelocityComputed(Vector3 safeVelocity)
    {
        LinearVelocity = safeVelocity;
    }
}
