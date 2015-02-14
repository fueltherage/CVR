using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VoxelSystemChunk))]
public class SystemChunkEditor : Editor {


	public override void OnInspectorGUI()
	{
		VoxelSystemChunk myTarget = (VoxelSystemChunk)target;

		VoxelPos v = new VoxelPos(EditorGUILayout.IntField("XSize", myTarget.XSize),
		                          EditorGUILayout.IntField("YSize", myTarget.YSize),
		                          EditorGUILayout.IntField("ZSize", myTarget.ZSize));
		myTarget.XSize = v.x;
		myTarget.YSize = v.y;
		myTarget.ZSize = v.z;

        myTarget.VoxelSpacing = EditorGUILayout.FloatField("VoxelSpacing", myTarget.VoxelSpacing);

        EditorGUILayout.LabelField("Voxel Pos :"+myTarget.voxPos.ToString());
	}
}
