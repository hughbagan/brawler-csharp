using Godot;
using System;

public class Level : Spatial
{
    private NavigationMeshInstance Navmesh;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

        Navmesh = GetNode<NavigationMeshInstance>("NavigationMeshInstance");
        CallDeferred("SetupNavServer");
    }

    private async void SetupNavServer()
    {
        RID map = NavigationServer.MapCreate();
        NavigationServer.MapSetUp(map, Vector3.Up);
        NavigationServer.MapSetCellSize(map, Navmesh.Navmesh.CellSize);
        NavigationServer.MapSetActive(map, true);
        RID region = NavigationServer.RegionCreate();
        NavigationServer.RegionSetTransform(region, new Transform());
        NavigationServer.RegionSetMap(region, map);
        NavigationMesh mesh = Navmesh.Navmesh;
        NavigationServer.RegionSetNavmesh(region, mesh);
        await ToSignal(GetTree(), "physics_frame");
    }
}
