using Godot;
using System;

public class Actor : RigidBody
{
    public Player PlayerInstance;
    public Area Hitbox;
    public bool Hit = false;

    private const float MoveSpeed = 2.0f;
    private NavigationAgent Nav;
    private SpotLight Eye;

    public override void _Ready()
    {
        PlayerInstance = GetNode<Player>("/root/Level/Player");
        Nav = GetNode<NavigationAgent>("NavigationAgent");
        Hitbox = GetNode<Area>("Hitbox");
        Eye = GetNode<SpotLight>("SpotLight");
    }

    public override void _PhysicsProcess(float delta)
    {
        // Eye.LookAt(GlobalTransform.origin + LinearVelocity, Vector3.Up);

        if (PlayerInstance != null && !Hit)
        {
            SetMovementTarget(PlayerInstance.GlobalTranslation);
            if (Nav.IsNavigationFinished())
                return;
            Vector3 newVelocity = (Nav.GetNextLocation() - GlobalTranslation).Normalized() * MoveSpeed;
            if (Nav.AvoidanceEnabled)
                Nav.SetVelocity(newVelocity);
            else
                _OnVelocityComputed(newVelocity);
        }
    }

    private void SetMovementTarget(Vector3 movementTarget)
    {
        Nav.SetTargetLocation(movementTarget);
        Eye.LookAt(movementTarget, Vector3.Up);
    }

    private void _OnVelocityComputed(Vector3 safeVelocity)
    {
        LinearVelocity = safeVelocity;
    }
}
