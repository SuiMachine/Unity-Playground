using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackInspector : MonoBehaviour
{
    bool drawHackInspector = false;
    bool refresh = true;
    Vector2 scroll;
    Transform element;
    uint rootObjectId = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
		{
            drawHackInspector = !drawHackInspector;
            if(drawHackInspector)
			{
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
			{
                //Modify it
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = false;
            }
        }
    }

	private void OnGUI()
	{
		if(drawHackInspector)
		{
            try
			{
                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUI.skin.box);
                scroll = GUILayout.BeginScrollView(scroll);
                if (GUILayout.Button("Restart"))
                {
                    element = this.gameObject.scene.GetRootGameObjects()[0].transform;
                }
				if (GUILayout.Button("Move camera"))
				{
					Camera.current.transform.position = this.element.transform.position;
					Camera.current.transform.rotation = this.element.transform.rotation;
				}

                if (GUILayout.Button("Up"))
                {
                    if (element.parent != null)
                    {
                        element = element.parent;
                    }
                    else
                    {
                        rootObjectId++;
                        if (rootObjectId < this.gameObject.scene.GetRootGameObjects().Length)
                            element = this.gameObject.scene.GetRootGameObjects()[rootObjectId].transform;
                        else
                        {
                            rootObjectId = 0;
                            element = this.gameObject.scene.GetRootGameObjects()[rootObjectId].transform;
                        }
                    }
                }
				
				GUILayout.Label("Current object:" + element.name);
                GUILayout.Label("CHILDREN:");
                for (int i = 0; i < element.childCount; i++)
                {
                    var child = element.GetChild(i);
                    if (GUILayout.Button(child.name))
                    {
                        element = child.transform;
                    }
                }

                GUILayout.Label("--------------");
                GUILayout.Label("MONO BEHAVIOURS:");
                var monoBehaviours = this.GetComponents<MonoBehaviour>();
                for (int i = 0; i < monoBehaviours.Length; i++)
                {
                    GUILayout.Label(monoBehaviours[i].GetType().Name);
                    GUILayout.Label("Fields (" + monoBehaviours[i].GetType().Name + "):");
                    var properties = monoBehaviours[i].GetType().GetProperties();

                    try
                    {
                        for (int k = 0; k < properties.Length; k++)
                        {
                            var value = properties[k].GetValue(monoBehaviours[i], new object[] { });
                            if(value.GetType() == typeof(Transform))
							{
                                var pos = transform.position;
                                var rot = transform.rotation;
                                GUILayout.Label(properties[k] + "(position): " + pos.ToString());
                                GUILayout.Label(properties[k] + "(rotation): " + rot.ToString());
                            }
                            else
							{
                                GUILayout.Label(properties[k] + ": " + value.ToString());
                            }
                        }
                    }
                    catch (Exception e2)
                    {
                        GUILayout.Label("Error" + e2);
                    }

                    GUILayout.Label("---------");
                }
                GUILayout.EndVertical();
                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
            }
            catch(Exception e)
			{
                element = this.gameObject.scene.GetRootGameObjects()[rootObjectId].transform;
            }            
		}
	}
}
