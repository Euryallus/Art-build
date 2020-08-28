using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialSwitch : MonoBehaviour
{
    private starStoneManager stoneManager;
    [SerializeField]
    private starStoneManager.starStones currentStone;

    [Tooltip("Blue, pink, orange, purple, crystal")]
    [SerializeField]
    public List<Material> materials = new List<Material>();

    [SerializeField]
    private Material hurt;

    public int crystalRendererIndex;
    [SerializeField]
    private MeshRenderer renderer;

    [SerializeField]
    private Enemy parent;
    void Start()
    {
        stoneManager = GameObject.FindGameObjectWithTag("GeneratorManager").GetComponent<starStoneManager>();
        renderer = gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        currentStone = stoneManager.returnActive();

        if(parent.hasHurt == false)
        {
            Material[] tempList = renderer.materials;
            tempList[crystalRendererIndex] = materials[(int)currentStone];
            tempList[1 - crystalRendererIndex] = renderer.materials[1 - crystalRendererIndex];
            renderer.materials = tempList;
        }
        else
        {
            Material[] tempList = renderer.materials;
            tempList[crystalRendererIndex] = hurt;
            tempList[1 - crystalRendererIndex] = renderer.materials[1 - crystalRendererIndex];
            renderer.materials = tempList;
        }
    }
}
