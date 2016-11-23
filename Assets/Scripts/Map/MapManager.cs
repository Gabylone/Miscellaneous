using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {

	public static MapManager Instance;

	private MapImage mapImage;

	private UIButton mapButton;

	[SerializeField]
	private Color islandColor;
	[SerializeField]
	private Color discoveredColor;

	private MapGenerator mapGenerator;

	private int posX = 0;
	private int posY = 0;

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {

		mapImage = GetComponent<MapImage> ();
		mapGenerator = GetComponent<MapGenerator> ();
		mapButton = GetComponent<UIButton> ();

			// init boat pos
		posX = Random.Range (0, (int)(mapImage.TextureScale));
		posY = (int)(mapImage.TextureScale / 6);

		mapGenerator.GenerateIslands ();

	}

	public void SetNewPos ( Vector2 v ) {

		posX += (int)v.x;
		posY += (int)v.y;

		int shipRange = NavigationManager.Instance.ShipRange;

		for (int x = -shipRange; x <= shipRange; ++x ) {

			for (int y = -shipRange; y <= shipRange; ++y ) {

				if (x == 0 && y == 0) {
					mapImage.UpdatePixel (posX + x, posY + y, Color.red);
				} else {
					mapImage.UpdatePixel (posX + x, posY + y, mapGenerator.IslandIds [posX + x, posY + y] > -1 ? islandColor : discoveredColor);
				}


			}

		}

	}

	#region properties
	public int PosX {
		get {
			return posX;
		}
		set {
			posX = value;
		}
	}
	public int PosY {
		get {
			return posY;
		}
		set {
			posY = value;
		}
	}
	public int Middle {
		get {
			return mapImage.TextureScale/2;
		}
	}
	public bool NearIsland {
		get {
			return MapGenerator.Instance.IslandIds [posX, posY] > -1;
		}
	}
	public int IslandID {
		get {
			return MapGenerator.Instance.IslandIds [posX, posY]; 
		}
	}
	public IslandData CurrentIsland {
		get {
//			return MapGenerator.Instance.IslandDatas [0];
			return MapGenerator.Instance.IslandDatas [IslandID];
		}
	}
	#endregion

	public UIButton MapButton {
		get {
			return mapButton;
		}
	}
}
