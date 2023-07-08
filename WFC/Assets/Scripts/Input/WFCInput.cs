using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using WaveFunctionCollapse;

public class WFCInput : MonoBehaviour
{
    [Header("Tilemaps"),Tooltip("Füge Referenzen aus der Hierarchie ein")]
    public Tilemap inputTilemap;
    public Tilemap outputTilemap;
    [Header("Generation Settings"), Tooltip("Stelle hier Werte für die Generierung ein")]
    public int patternSize=1;
    public int maxIterations = 500;
    public int outputWidth;
    public int outputHeight;
    public bool equalWeigths = false;
    private ValueManager<TileBase> valueManager;
    private WFCCore core;
    private TileMapOutput output;
    private PatternManager manager;
    private InputReader reader;

    public void CreateWFC()
    {
        reader = new InputReader(inputTilemap);
        var grid = reader.ReadInputToGrid();
        valueManager = new ValueManager<TileBase>(grid);
        manager = new PatternManager(2);
        manager.ProcessGrid(valueManager, equalWeigths);
        core = new WFCCore(outputWidth, outputHeight, maxIterations, manager);
    }
    public void CreateTilemap()
    {
        output = new TileMapOutput(valueManager, outputTilemap);
        var result = core.CreateOutputGrid();
        output.CreateOutput(manager, result, outputWidth, outputHeight);
    }
    public void SaveTileMap()
    {
        if (output.OutputImage != null)
        {
            outputTilemap = output.OutputImage;
            GameObject objectToSave = outputTilemap.gameObject;
            PrefabUtility.SaveAsPrefabAsset(objectToSave, "Assets/Gespeichertes/output.prefab");
        }
    }
}
