
using MLAPI;
using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI.SceneManagement;

namespace HelloWorld {
	public class HelloWorldManager : MonoBehaviour {

		string playerName="Name";

		private void OnEnable() {
			DontDestroyOnLoad(this.gameObject);
		}

		void OnGUI() {
			GUILayout.BeginArea(new Rect(10, 10, 300, 300));
			if (!NetworkManager.Singleton.IsClient&&!NetworkManager.Singleton.IsServer) {
				StartButtons();
				playerName=GUILayout.TextField(playerName, 10);
			} else if (SceneManager.GetActiveScene().name=="ConnectScene") {
				StatusLabels();
				ShowName(playerName);
				SubmitNewPosition();
			} else {
				ShowName(playerName);
			}

			GUILayout.EndArea();
		}

		void StartButtons() {
			if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
			if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
			if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
		}

		static void StatusLabels() {
			var mode = NetworkManager.Singleton.IsHost ?
				"Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

			GUILayout.Label("Transport: "+
				NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
			GUILayout.Label("Mode: "+mode);
			
		}

		static void ShowName(string playerName) {
			string pN = "";
			if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
					out var networkedClient)) {
				var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
				if (player) {
					pN=playerName;
					player.SetPlayerNameServerRpc(pN);
				}
			}
			GUILayout.Label("Name: "+pN);
		}

		static void SubmitNewPosition() {
			if (GUILayout.Button(NetworkManager.Singleton.IsServer ? "Start" : "Waiting for server")) {
				/*if (NetworkManager.Singleton.ConnectedClients.TryGetValue(NetworkManager.Singleton.LocalClientId,
					out var networkedClient)) {
					var player = networkedClient.PlayerObject.GetComponent<HelloWorldPlayer>();
					if (player) {
						player.Move();
					}
				}*/
				if (NetworkManager.Singleton.IsServer) {
					NetworkSceneManager.SwitchScene("SampleScene");
				}
			}
		}
	}
}