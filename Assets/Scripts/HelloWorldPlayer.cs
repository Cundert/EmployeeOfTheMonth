
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;

namespace HelloWorld {
	public class HelloWorldPlayer : NetworkBehaviour {

		public Vector2 dir, adir;
		public int nattacks;
		public float speed;
    public CameraController playerCamera;
		
		public float attackDelay = 0.2f;
		public float lastAttack = 0.0f;
		public float Timer = 0.0f;
		
		public GameObject AttackObject;
		
		public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});
		
		public NetworkVariableVector3 AttackDir = new NetworkVariableVector3(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});
		
		public NetworkVariableInt NAttacks = new NetworkVariableInt(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});

		public override void NetworkStart() {
			Move();
		}

		public void Move() {
			if (!IsLocalPlayer) return;
			transform.position+=new Vector3(dir.x, dir.y, 0);
			UpdatePositionServerRpc(transform.position);
		}
		
		public void Attack(){
			if(!IsLocalPlayer) return;
			if(adir.x == 0 && adir.y == 0) return;
			if(lastAttack + attackDelay > Timer) return;
			lastAttack = Timer;
			UpdateAttackServerRpc(adir);
		}

		[ServerRpc]
		void UpdatePositionServerRpc(Vector3 d, ServerRpcParams rpcParams = default) {
			Position.Value=d;
		}
		
		[ServerRpc]
		void UpdateAttackServerRpc(Vector3 a, ServerRpcParams rpcParams = default) {
			AttackDir.Value=a;
			NAttacks.Value++;
		}
		
		void generateAttack(){
			// Generates an attack in direction AttackDir
			// todo
			GameObject bullet = Instantiate(AttackObject, transform.position, Quaternion.FromToRotation(new Vector3(1, 0, 0), AttackDir.Value));
			
		}

        void Start()
        {
            playerCamera = CameraController.instance;
        }

		void Update() {
			while(nattacks < NAttacks.Value){
				++nattacks;
				generateAttack();
			}
			if (IsLocalPlayer) {
				Timer += Time.deltaTime;
				float val = speed*Time.deltaTime;
				dir = new Vector2(0, 0);
				if (Input.GetKey("s")) dir+=new Vector2( 0, -1);
				if (Input.GetKey("w")) dir+=new Vector2( 0,  1);
				if (Input.GetKey("a")) dir+=new Vector2(-1,  0);
				if (Input.GetKey("d")) dir+=new Vector2( 1,  0);
				dir.Normalize();
				dir = dir * val;
				
				adir = new Vector2(0, 0);
				if (Input.GetKey("down"))  adir+=new Vector2( 0, -1);
				if (Input.GetKey("up"))    adir+=new Vector2( 0,  1);
				if (Input.GetKey("left"))  adir+=new Vector2(-1,  0);
				if (Input.GetKey("right")) adir+=new Vector2( 1,  0);
				
				Move();
				Attack();
        
        Vector3 serverPosition = Position.Value;
        Vector3 possibleFuturePosition = new Vector3(serverPosition.x, serverPosition.y, -10);
        if (Vector3.Distance(playerCamera.transform.position, possibleFuturePosition) > 0.05)
            playerCamera.transform.position = possibleFuturePosition;
			} else {
				transform.position=Position.Value;
			}
		}
	}
}
