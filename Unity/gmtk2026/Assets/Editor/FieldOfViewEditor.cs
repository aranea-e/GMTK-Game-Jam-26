using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (NPC))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI() {
		NPC fow = (NPC)target;
		Handles.color = Color.white;
		Handles.DrawWireArc (fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);
		Vector3 viewAngleA = fow.DirFromAngle (-fow.transform.eulerAngles.z - fow.viewAngle / 2, true);
		Vector3 viewAngleB = fow.DirFromAngle (-fow.transform.eulerAngles.z + fow.viewAngle / 2, true);

		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
		Handles.DrawLine (fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

		Handles.color = Color.red;
		foreach (Transform visibleTarget in fow.visibleTargets) {
			Handles.DrawLine (fow.transform.position, visibleTarget.position);
		}
	}

}