using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MultiInput : MonoBehaviour {

    //storage for keys
	[SerializeField]
	public List<keyInfo> keys = new List<keyInfo>();
	[SerializeField]
	public List<bool> visible = new List<bool>();

    // Update is called once per frame
    void Update()
	{
		//loop through keys and test input
		if (Input.anyKey) {
			foreach (keyInfo info in keys) {
				int accepted = 0;
				if (!info.axisInput) {
					foreach (KeyCode key in info.key) {
						//check if inputs check is less than max
						if (accepted < info.keyInputsAccepted) {
							if (Input.GetButton (key.ToString ())) {
								if (info.keyEvent.GetPersistentEventCount () > 0) {
									for (int a = 0; a < info.keyEvent.GetPersistentEventCount (); a++) {
										activate (info, info.keyEvent.GetPersistentTarget (a), info.keyEvent.GetPersistentMethodName (a));
									}
								} else {
									activate (info, null, null);
								}
								accepted++;
							}
						}
					}
				} else {
					//check if axis has been used
					if (Input.GetAxis (info.axis) != 0) {

						//set variable to axis value
						info.variable = Input.GetAxis (info.axis);
						activate (info, null, null);

					}
				}
			}
		}
	}

	void activate(keyInfo info, Object temp, string name) {
			
		//check if variable input or private function
		if (info.variableInput || info.privateFunction) {

			//check if function object exists
			if (info.functionObject) {

				//check if variable input or not
				if (info.variableInput) {
					//invoke method based on function object
					customObjectVariableInvoke (info);
				} else {
					//invoke method based on function object
					customObjectInvoke (info);
				}
			} else if (!info.functionObject) {

				//check if variable input or not
				if (info.variableInput) {
					//invoke method based on event systems
					customVariableInvoke (temp, name, info.variable);
				} else {
					//invoke method based on event systems
					customInvoke (temp, name);
				}
			}
		} else {
			//if basic invoke, invoke event
			info.keyEvent.Invoke ();
		}
	}

	void debug() {
		transform.Translate (Vector3.up);
	}

	void debug1(float output) {
		transform.Translate (Vector3.up * output);
	}

	void customInvoke(object info, string name) {
		((MonoBehaviour)info).SendMessage (name);
	}

	void customVariableInvoke(object info, string name, float variable) {
		((MonoBehaviour)info).SendMessage (name, variable);
	}

	void customObjectInvoke(keyInfo info) {
		Debug.Log (info.privateFunctionName);
		info.functionObject.SendMessage (info.privateFunctionName);
	}

	void customObjectVariableInvoke(keyInfo info) {
		info.functionObject.SendMessage (info.privateFunctionName, info.variable);
	}
}


//default class for key input data
[System.Serializable]
public class keyInfo
{
	//basic variables for input names should explain it
	public int keyInputsAccepted;
	public bool axisInput;
	public string axis;
    public List<KeyCode> key;
	public UnityEvent keyEvent;
	public GameObject functionObject;
	public string privateFunctionName;
	public bool privateFunction;
	public bool variableInput;
	public float variable;
}