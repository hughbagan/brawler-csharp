using Godot;
using System;

public partial class Level : Node3D
{
	private NavigationRegion3D NavigationMesh;

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;

		NavigationMesh = GetNode<NavigationRegion3D>("NavigationRegion3D");
		// CallDeferred("SetupNavServer");
	}

	// private async void SetupNavServer()
	// {
	// 	RID map = NavigationServer3D.MapCreate();
	// 	NavigationServer3D.MapSetUp(map, Vector3.Up);
	// 	NavigationServer3D.MapSetCellSize(map, NavigationMesh.NavigationMesh.CellSize);
	// 	NavigationServer3D.MapSetActive(map, true);
	// 	RID region = NavigationServer3D.RegionCreate();
	// 	NavigationServer3D.RegionSetTransform(region, new Transform3D());
	// 	NavigationServer3D.RegionSetMap(region, map);
	// 	NavigationMesh mesh = NavigationMesh.NavigationMesh;
	// 	NavigationServer3D.RegionSetNavigationMesh(region, mesh);
	// 	await ToSignal(GetTree(), "physics_frame");
	// }
}
