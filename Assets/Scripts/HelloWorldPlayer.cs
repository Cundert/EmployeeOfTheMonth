
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace HelloWorld {
	public class HelloWorldPlayer : NetworkBehaviour {

		public Vector2 dir;
		public float speed;

		public NetworkVariableBool namesDisplayed = new NetworkVariableBool(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.Everyone,
			ReadPermission=NetworkVariablePermission.Everyone
		});

		public NetworkVariableString PlayerName = new NetworkVariableString(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});

		public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});

		private void OnEnable() {
			namesDisplayed.OnValueChanged+=DisplayNames;
		}

		private void OnDisable() {
			namesDisplayed.OnValueChanged-=DisplayNames;
		}

		public override void NetworkStart() {
			Move();
		}

		public void Move() {
			if (!IsLocalPlayer) return;
			transform.position+=new Vector3(dir.x, dir.y, 0);
			SubmitPositionRequestServerRpc(new Vector2(transform.position.x,transform.position.y));
		}

		void DisplayNames(bool before, bool after) {
			if (!after) {
				string childname = transform.GetChild(0).gameObject.GetComponent<TextMesh>().text;
				if (childname!=PlayerName.Value) {
					transform.GetChild(0).gameObject.GetComponent<TextMesh>().text=PlayerName.Value;
				}
				namesDisplayed.Value=true;
			}
		}

		[ServerRpc]
		public void SetPlayerNameServerRpc(string name, ServerRpcParams rpcParams = default) {
			PlayerName.Value=name;
			namesDisplayed.Value=false;
		}

		[ServerRpc]
		void SubmitPositionRequestServerRpc(Vector2 d, ServerRpcParams rpcParams = default) {
			Position.Value=new Vector3(d.x, d.y, 0);
		}

		Vector3 GetRandomPositionOnPlane() {
			return new Vector3(dir.x, dir.y, 0);
		}

		void Update() {
			float val = speed*Time.deltaTime;
            dir = new Vector2(0, 0);
            if (Input.GetKey("s")) dir+=new Vector2(0, -1);
			if (Input.GetKey("w")) dir+=new Vector2(0, 1);
			if (Input.GetKey("a")) dir+=new Vector2(-1, 0);
			if (Input.GetKey("d")) dir+=new Vector2(1, 0);
            dir.Normalize();
            dir = dir * val;
			Move();
			if (!IsLocalPlayer) transform.position=Position.Value;
		}
	}
}