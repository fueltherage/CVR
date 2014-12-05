﻿using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class VoxelGeneratorEditorWindow : EditorWindow 
{

	public GameObject SystemObject;
	private VoxelSystemGreedy test;

	private GameObject genObj;
	private Object objField;

	private int XSize, YSize, ZSize;
	private float VoxelSpacing;
	private int XCSize, YCSize, ZCSize;



	[MenuItem("Voxel Generator/Open Editor")]
	private static void showEditor()
	{
		EditorWindow.GetWindow<VoxelGeneratorEditorWindow>("Vox Gen");      
	}

	[MenuItem ("Voxel Generator/Open Editor",true)]
	private static bool showEditorValidator()
	{
		return true;
	}


	void OnGUI()
	{
		EditorGUILayout.LabelField("System Prefab");
		objField = EditorGUILayout.ObjectField(objField, typeof(Object), false);

		SystemObject = objField as GameObject;
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Init"))
		{	
			genObj = Instantiate(SystemObject, new Vector3(0,0,0),Quaternion.identity) as GameObject;
			VoxelSystemGreedy vs = genObj.GetComponent<VoxelSystemGreedy>();
			vs.XSize = XSize;
			vs.YSize = YSize;
			vs.ZSize = ZSize;
			vs.ChunkSizeX = XCSize;
			vs.ChunkSizeY = YCSize;
			vs.ChunkSizeZ = ZCSize;
			vs.VoxelSpacing = VoxelSpacing;
			vs.Init();
			vs.PreMade = true;//Must happen after Init();
		}
		if(GUILayout.Button("Fill")&& genObj!= null)
		{
			Fill ();
		}
		EditorGUILayout.EndHorizontal(); 
	
        VoxelSpacing = EditorGUILayout.IntField("Voxel Spacing", 1);
        EditorGUILayout.LabelField("System Dimensions");
		XSize = EditorGUILayout.IntField("X", XSize);
		YSize = EditorGUILayout.IntField("Y", YSize);
		ZSize = EditorGUILayout.IntField("Z", ZSize);
		
        EditorGUILayout.LabelField("Chunk Dimensions");
		XCSize = EditorGUILayout.IntField("X", XCSize);
		YCSize = EditorGUILayout.IntField("Y", YCSize);
		ZCSize = EditorGUILayout.IntField("Z", ZCSize);
   
	}
	void Fill()
	{
		VoxelSystemGreedy temp = genObj.GetComponent<VoxelSystemGreedy>();
		for (int x = 0; x < temp.XSize; x++){
			for (int y = 0; y < temp.YSize; y++){
				for (int z = 0; z < temp.ZSize; z++){
					for (int xc = 0; xc < temp.ChunkSizeX; xc++){
						for (int yc = 0; yc < temp.ChunkSizeY; yc++){
							for (int zc = 0; zc < temp.ChunkSizeZ; zc++)
							{
								//vsg.AddVoxel(xc + x * vsg.ChunkSizeX,yc + y * vsg.ChunkSizeY, zc + z * vsg.ChunkSizeZ,false,1);
								VoxelFactory.GenerateVoxel(1, ref temp.chunks_vcs[x, y, z].blocks[xc,yc,zc],
														   temp.chunks_vcs[x, y, z].offset, temp.VoxelSpacing);	
							}
						}
					}
				}
			}
		}

		temp.HardUpdateMeshes();
	}
}
