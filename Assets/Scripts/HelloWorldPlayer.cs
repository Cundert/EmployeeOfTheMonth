using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HelloWorld {
	public class HelloWorldPlayer : NetworkBehaviour {

		public Vector2 dir, adir;
		public int nattacks;
		public float speed;

        public CameraController playerCamera;
		public GameObject cameraFocus;
		public GameObject lastAttacker;

		public List<HelloWorldPlayer> kills = new List<HelloWorldPlayer>();
		
		public float attackDelay = 0.2f;
		public float lastAttack = 0.0f;
		public float Timer = 0.0f;
		
		public GameObject AttackObject;

		public bool isDead;

		public NetworkVariableString PlayerName = new NetworkVariableString(new NetworkVariableSettings {
			WritePermission=NetworkVariablePermission.ServerOnly,
			ReadPermission=NetworkVariablePermission.Everyone
		});

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

		public NetworkVariableInt HP = new NetworkVariableInt(new NetworkVariableSettings {
			WritePermission = NetworkVariablePermission.ServerOnly,
			ReadPermission = NetworkVariablePermission.Everyone
		});

        public NetworkVariableBool HPHasBeenSet = new NetworkVariableBool(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        private void OnEnable() {
			PlayerName.OnValueChanged+=DisplayNames;
			DontDestroyOnLoad(this.gameObject);
		}

		private void OnDisable() {
			PlayerName.OnValueChanged-=DisplayNames;
		}

		public override void NetworkStart() {
			Move();
			InitializeHPServerRpc(20);
		}

		public void Move() {
			if (!IsLocalPlayer) return;
			transform.position+=new Vector3(dir.x, dir.y, 0);
			UpdatePositionServerRpc(transform.position);
		}
		
		public void Attack()
        {
			if(!IsLocalPlayer) return;
			if(adir.x == 0 && adir.y == 0) return;
			if(lastAttack + attackDelay > Timer) return;
			lastAttack = Timer;
			UpdateAttackServerRpc(adir);
		}

		void DisplayNames(string before, string after) {
			string childname = transform.GetChild(0).gameObject.GetComponent<TextMesh>().text;
			if (childname!=after) {
				transform.GetChild(0).gameObject.GetComponent<TextMesh>().text=after;
			}
		}

		[ServerRpc]
		public void SetPlayerNameServerRpc(string name, ServerRpcParams rpcParams = default) {
			PlayerName.Value=name;
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

        [ServerRpc]
        void InitializeHPServerRpc(int InitialHP, ServerRpcParams rpcParams = default)
        {
            HP.Value = InitialHP;
            HPHasBeenSet.Value = true;
        }

        [ServerRpc]
        void UpdateHPServerRpc(int HPDiff, ServerRpcParams rpcParams = default)
        {
            HP.Value += HPDiff;
        }
		
	void generateAttack(){
		// Generates an attack in direction AttackDir
		// todo
		GameObject bullet = Instantiate(AttackObject, transform.position, Quaternion.FromToRotation(new Vector3(1, 0, 0), AttackDir.Value));
		bullet.GetComponent<BulletScript>().source = gameObject;
	}

        Vector2 getMovementVector(float val)
        {
            Vector2 dir = new Vector2(0, 0);
            if (Input.GetKey("s")) dir += new Vector2(0, -1);
            if (Input.GetKey("w")) dir += new Vector2(0, 1);
            if (Input.GetKey("a")) dir += new Vector2(-1, 0);
            if (Input.GetKey("d")) dir += new Vector2(1, 0);
            dir.Normalize();
            return dir * val;
        }

        Vector2 getAttackVector()
        {
            adir = new Vector2(0, 0);
            if (Input.GetKey("down")) adir += new Vector2(0, -1);
            if (Input.GetKey("up")) adir += new Vector2(0, 1);
            if (Input.GetKey("left")) adir += new Vector2(-1, 0);
            if (Input.GetKey("right")) adir += new Vector2(1, 0);
            return adir;
        }

        void MoveCamera()
        {
            Vector3 serverPosition = cameraFocus.GetComponent<HelloWorldPlayer>().Position.Value;
            Vector3 possibleFuturePosition = new Vector3(serverPosition.x, serverPosition.y, -10);
            if (Vector3.Distance(playerCamera.transform.position, possibleFuturePosition) > 0.05)
                playerCamera.transform.position = possibleFuturePosition;
        }

		void ChangeCameraFocus(GameObject focus) {
			cameraFocus=focus;
		}

		void UpdateMyKillsCamera(GameObject focus) {
			foreach (HelloWorldPlayer ded in kills) {
				ded.ChangeCameraFocus(focus);
				ded.UpdateMyKillsCamera(focus);
			}
		}

		void Die() {
			if (IsLocalPlayer) {
				ChangeCameraFocus(lastAttacker);
			}
			UpdateMyKillsCamera(lastAttacker);
			lastAttacker.GetComponent<HelloWorldPlayer>().kills.Add(this);
			gameObject.layer=6;
			GetComponent<SpriteRenderer>().enabled=false;
			transform.GetChild(0).GetComponent<MeshRenderer>().enabled=false;
			isDead=true;
		}

        void Start()
        {
			isDead=false;
            playerCamera = CameraController.instance;
			cameraFocus=gameObject;
        }

		void Update() {
			if (!isDead&&HPHasBeenSet.Value&&HP.Value==0)
				Die();

            while (!isDead && nattacks < NAttacks.Value){
				++nattacks;
				generateAttack();
			}

			if (IsLocalPlayer) {
				if (!isDead) {
					Timer+=Time.deltaTime;

					dir=getMovementVector(speed*Time.deltaTime);
					adir=getAttackVector();

					Move();
					Attack();
				}
				MoveCamera();

            } else {
				if (!isDead) transform.position=Position.Value;
			}
		}

    void OnTriggerEnter2D(Collider2D other)
    {
      // If the Player has collided with a bullet that isn't theirs, and the Player is the local one
      // Then reduce the Player's health
      if (!isDead && other.gameObject.tag == "Bullet" && other.GetComponent<BulletScript>().source != gameObject) {
			lastAttacker=other.GetComponent<BulletScript>().source;
			if (IsLocalPlayer) UpdateHPServerRpc(other.GetComponent<BulletScript>().BulletDamage*-1);
      }
    }

	}
}
