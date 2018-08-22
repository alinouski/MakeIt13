using UnityEngine;
using System.Collections;

[System.Serializable]
public class TileStyle
{
	public int Number;
	public Color32 TileColor;
	public Color32 TextColor;
}

[System.Serializable]
public class TileStyle2
{
    public int[] Number;
    public Color32[] TileColor;
    public Color32[] TextColor;
}


public class TileStyleHolder : MonoBehaviour {

	// SINGLETON
	public static TileStyleHolder Instance;

    
    public TileStyle[] TileStyles;

    public TileStyle2[] TileStylesHolder;

	void Awake()
	{
		Instance = this;
	}
}
