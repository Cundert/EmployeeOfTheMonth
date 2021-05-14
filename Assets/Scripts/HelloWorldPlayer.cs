
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace HelloWorld {
	public class HelloWorldPlayer : NetworkBehaviour {

		public Vector2 dir;
		public float speed;
        public CameraController playerCamera;

        public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});

		public override void NetworkStart() {
			Move();
		}

		public void Move() {
			if (!IsLocalPlayer) return;
			transform.position+=new Vector3(dir.x, dir.y, 0);
			SubmitPositionRequestServerRpc(new Vector2(transform.position.x,transform.position.y));
		}

		[ServerRpc]
		void SubmitPositionRequestServerRpc(Vector2 d, ServerRpcParams rpcParams = default) {
			Position.Value=new Vector3(d.x, d.y, 0);
		}

		Vector3 GetRandomPositionOnPlane() {
			return new Vector3(dir.x, dir.y, 0);
		}

        void Start()
        {
            playerCamera = CameraController.instance;
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
            Vector3 serverPosition = Position.Value;
            if (!IsLocalPlayer) transform.position = serverPosition;
            else
            {
                Vector3 possibleFuturePosition = new Vector3(serverPosition.x, serverPosition.y, -10);
                if (Vector3.Distance(playerCamera.transform.position, possibleFuturePosition) > 0.05)
                    playerCamera.transform.position = possibleFuturePosition;
            }
		}
	}
}