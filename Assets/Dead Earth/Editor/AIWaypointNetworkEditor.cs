using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.AI;

[CustomEditor(typeof(AIWaypointNetwork))]
public class AIWaypointNetworkEditor : Editor {
	
	//this function implements integar slidebar on inspector
	public override void OnInspectorGUI(){
		AIWaypointNetwork network = (AIWaypointNetwork)target;
		//did not implement Range() in AIWaypointNetwork.cs because cannot access list count
		if(network.DisplayMode==PathDisplayMode.Paths)
		{
			network.UIStart = EditorGUILayout.IntSlider("Waypoint Start", network.UIStart, 0, network.Waypoints.Count-1);
			network.UIEnd = EditorGUILayout.IntSlider("Waypoint End", network.UIEnd, 0, network.Waypoints.Count-1);
		}
		DrawDefaultInspector();
		SceneView.RepaintAll();
	}

	//This function allows user to select scene display modes among none, connections and paths
	void OnSceneGUI(){
		AIWaypointNetwork network = (AIWaypointNetwork)target;
		LabelWaypoints(network);

		if(network.DisplayMode == PathDisplayMode.Connections)
			DrawWaypointLines(network);	
		else if(network.DisplayMode == PathDisplayMode.Paths)
			DrawPaths(network);	
	}

	//this function lables wayPoints
	void LabelWaypoints(AIWaypointNetwork network){
		for (int i=0; i<network.Waypoints.Count; i++){
			if(network.Waypoints[i]!=null)
			{
				Handles.Label(network.Waypoints[i].position, "Waypoint " + i.ToString());
			}
		}
	}

	//this function links wayPoints
	void DrawWaypointLines(AIWaypointNetwork network){
		Vector3[] linePoints = new Vector3[network.Waypoints.Count + 1];
		
		for (int i=0; i<=network.Waypoints.Count; i++){
			int index = i<network.Waypoints.Count ? i : 0; //tenary conditional operator
			linePoints[i] = network.Waypoints[index].position;
		}
		//linePoints[linePoints.Length - 1] = linePoints[0];
		Handles.color = Color.magenta;
		Handles.DrawPolyLine(linePoints);
	}

	//this function draws navMesh paths (least distance route)
	void DrawPaths(AIWaypointNetwork network){
		NavMeshPath navMeshPath = new NavMeshPath();
		Vector3 start = network.Waypoints[network.UIStart].position;
		Vector3 end = network.Waypoints[network.UIEnd].position;
		NavMesh.CalculatePath(start, end, NavMesh.AllAreas, navMeshPath);
		Handles.color = Color.yellow;
		Handles.DrawPolyLine(navMeshPath.corners);
	}
}
