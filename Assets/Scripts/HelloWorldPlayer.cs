
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace HelloWorld {
	public class HelloWorldPlayer : NetworkBehaviour {

		public Vector2 dir;
		public float speed;

		public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});

		public override void NetworkStart() {
			Move();
		}

		public void Move() {
<<<<<<< HEAD
			if( !isLocalPlayer ) return;
			/*if (NetworkManager.Singleton.IsServer) {
				var randomPosition = GetRandomPositionOnPlane();
				//Position.Value+=randomPosition;
			} else {*/
				SubmitPositionRequestServerRpc(dir);
			//}
=======
			if (!IsLocalPlayer) return;
			SubmitPositionRequestServerRpc(dir);
>>>>>>> 4d6f49433540fcdcf1eb2541919d34288e87f004
		}

		[ServerRpc]
		void SubmitPositionRequestServerRpc(Vector2 d, ServerRpcParams rpcParams = default) {
			Position.Value+=new Vector3(d.x, d.y, 0);
		}

		Vector3 GetRandomPositionOnPlane() {
			return new Vector3(dir.x, dir.y, 0);
		}

		void Update() {
			float val = speed*Time.deltaTime;
			if (Input.GetKey("s")) dir=new Vector2(0, -val);
			else if (Input.GetKey("w")) dir=new Vector2(0, val);
			else if (Input.GetKey("a")) dir=new Vector2(-val, 0);
			else if (Input.GetKey("d")) dir=new Vector2(val, 0);
			else dir=new Vector2(0, 0);
			Move();
			transform.position=Position.Value;
		}
	}
}
