using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DungeonCreator : MonoBehaviour
{
    public int dungeonWidth, dungeonLength;
    public int roomWidthMin, roomLengthMin;
    public int maxIterations;
    public int corridorWidth;
    public int wallSpacing;
    public Material material;
    [Range(0.0f, 0.3f)]
    public float roomBottomCornerModifier;
    [Range(0.7f, 1.0f)]
    public float roomTopCornerMidifier;
    [Range(0, 2)]
    public int roomOffset;
    public GameObject wallVertical, wallHorizontal;
    List<Vector3Int> possibleDoorVerticalPosition;
    List<Vector3Int> possibleDoorHorizontalPosition;
    List<Vector3Int> possibleWallHorizontalPosition;
    List<Vector3Int> possibleWallVerticalPosition;
    // Start is called before the first frame update

    public GameObject Player;
    public GameObject test;
    public GameObject decorationTest;
    public GameObject groundDecorationTest;
    public GameObject characterSpawn;
    

    void Start()
    {
        groundDecorationTest.GetComponent<Rigidbody>();
        CreateDungeon();
        
    }

    public void CreateDungeon()
    {
        DestroyAllChildren();
        DugeonGenerator generator = new DugeonGenerator(dungeonWidth, dungeonLength);
        var listOfRooms = generator.CalculateDungeon(maxIterations,
            roomWidthMin,
            roomLengthMin,
            roomBottomCornerModifier,
            roomTopCornerMidifier,
            roomOffset,
            corridorWidth);
        GameObject wallParent = new GameObject("WallParent");
        wallParent.transform.parent = transform;
        possibleDoorVerticalPosition = new List<Vector3Int>();
        possibleDoorHorizontalPosition = new List<Vector3Int>();
        possibleWallHorizontalPosition = new List<Vector3Int>();
        possibleWallVerticalPosition = new List<Vector3Int>();
        //Testing
        List<Node> realRooms = new List<Node>();
        List<Vector3> roomCenters= new List<Vector3>();
        
        for (int i = 0; i < listOfRooms.Count; i++)
        {

            CreateMesh(listOfRooms[i].BottomLeftAreaCorner, listOfRooms[i].TopRightAreaCorner); //Original stuff

            /*var room=listOfRooms[i];
            bool isRealRoom = room.TopRightAreaCorner.x> room.BottomLeftAreaCorner.x &&
                room.TopRightAreaCorner.y > room.BottomLeftAreaCorner.y;
            if (isRealRoom)
            {
                realRooms.Add(room);
                CreateMesh(room.BottomLeftAreaCorner, room.TopRightAreaCorner);
            }*/
        }
        
        //Test
        /*if (realRooms.Count >= 2 && test != null)
        {
            // First
            Vector2 bl1 = realRooms[0].BottomLeftAreaCorner;
            Vector2 tr1 = realRooms[0].TopRightAreaCorner;
            Vector3 firstCenter = new Vector3((bl1.x + tr1.x) / 2f, 0, (bl1.y + tr1.y) / 2f);

            // Last REAL room
            Vector2 bl2 = realRooms[10].BottomLeftAreaCorner;
            Vector2 tr2 = realRooms[10].TopRightAreaCorner;
            Vector3 lastCenter = new Vector3((bl2.x + tr2.x) / 2f, 0, (bl2.y + tr2.y) / 2f);

            // VERY IMPORTANT — parent to this.transform so markers are destroyed each regen
            Instantiate(test, firstCenter, Quaternion.identity, transform);
            Instantiate(test, lastCenter, Quaternion.identity, transform);
        }*/
        //Testing
        int firstDungeon = UnityEngine.Random.Range(0, 10);
        float Highestdistance = 0f;
        foreach (var room in listOfRooms)
        {
            Vector3 center = new Vector3((room.BottomLeftAreaCorner.x + room.TopRightAreaCorner.x) / 2f, 0, (room.BottomLeftAreaCorner.y + room.TopRightAreaCorner.y) / 2f);
            roomCenters.Add(center);
        }
        int finalDungeonindex = 0;
        for (int i = 0; i < listOfRooms.Count; i++)
        {
            float distance = Vector3.Distance(roomCenters[firstDungeon], roomCenters[i]);
            if (distance > Highestdistance)
            {
                Highestdistance = distance;
                finalDungeonindex = i;
            }
        }
        
        if (listOfRooms.Count >= 2)
        {
            Vector2 bl1 = listOfRooms[firstDungeon].BottomLeftAreaCorner;
            Vector2 tr1= listOfRooms[firstDungeon].TopRightAreaCorner;
            Vector3 firstCenter = new Vector3((bl1.x + tr1.x) / 2f, 5, (bl1.y + tr1.y) / 2f);

            Vector2 bl2 = listOfRooms[finalDungeonindex].BottomLeftAreaCorner;
            Vector2 tr2 = listOfRooms[finalDungeonindex].TopRightAreaCorner;
            Vector3 lastCenter = new Vector3((bl2.x + tr2.x) / 2f, 5, (bl2.y + tr2.y) / 2f);

            //Instantiate(test, firstCenter, Quaternion.identity, transform);
            characterSpawn.transform.position= firstCenter;
            Instantiate(test, lastCenter, Quaternion.identity, transform);
        }
        //END TEST
        CreateWalls(wallParent);
        //NEWTEST Decoration Corners
        for (int i = 0; i < (listOfRooms.Count/2)+1; i++)
        {
            Vector2 bl = listOfRooms[i].BottomLeftAreaCorner;
            Vector2 tr = listOfRooms[i].TopRightAreaCorner;

            float inset = 1.0f;//original value 1
            float y = 0f;

            List<Vector3> decorationPoints = new List<Vector3>()
            {
                new Vector3(bl.x + inset, y, bl.y + inset), // BL
                new Vector3(tr.x - inset, y, bl.y + inset), // BR
                new Vector3(bl.x + inset, y, tr.y - inset), // TL
                new Vector3(tr.x - inset, y, tr.y - inset), // TR

                //new Vector3((bl.x + tr.x)/2f, y, bl.y + inset), // bottom
                //new Vector3((bl.x + tr.x)/2f, y, tr.y - inset), // top
                //new Vector3(bl.x + inset, y, (bl.y + tr.y)/2f), // left
                //new Vector3(tr.x - inset, y, (bl.y + tr.y)/2f)  // right
            };
            foreach (var pos in decorationPoints)
            {
                if (UnityEngine.Random.value > 0.5f) // density control
                    Instantiate(decorationTest, pos, Quaternion.identity, transform);
            }
            //Instantiate(decorationTest, decorationTestVector, Quaternion.identity, transform);

            //Decoration On Ground Randomly

            float groundInset = 1.2f;
            // adding a loop for more decorations
            int numOfDecorations = UnityEngine.Random.Range(8, 12);
            for (int j = 0; j < numOfDecorations; j++)
            {
                float x = UnityEngine.Random.Range(bl.x + groundInset, tr.x - groundInset);
                float z = UnityEngine.Random.Range(bl.y + groundInset, tr.y - groundInset);
                Vector3 spawnPosition = new Vector3(x, 0.0f, z);
                Ray ray = new Ray(spawnPosition + Vector3.up * 5f, Vector3.down);
                LayerMask floorMask = LayerMask.NameToLayer("Floor");
                if (Physics.Raycast(ray, out RaycastHit hit, 10f, floorMask))
                {
                    spawnPosition.y = hit.point.y;
                    Quaternion rotation = Quaternion.Euler(85, UnityEngine.Random.Range(0f, 360f), 0);
                    //Instantiate(groundDecorationTest, hit.point, rotation, transform);
                    spawnPosition.y = spawnPosition.y + 0.5f;
                    Instantiate(groundDecorationTest, spawnPosition, rotation, transform);
                }
            }
            
            

        }
        Player.transform.position = characterSpawn.transform.position;
    }
    private void CreateWalls(GameObject wallParent)
    {
        foreach (var wallPosition in possibleWallHorizontalPosition)
        {
            CreateWall(wallParent, wallPosition, wallHorizontal);
        }
        foreach (var wallPosition in possibleWallVerticalPosition)
        {
            CreateWall(wallParent, wallPosition, wallVertical);
        }
    }


    private void CreateWall(GameObject wallParent, Vector3Int wallPosition, GameObject wallPrefab)
    {
        Instantiate(wallPrefab, wallPosition, Quaternion.identity, wallParent.transform);
    }

    private void CreateMesh(Vector2 bottomLeftCorner, Vector2 topRightCorner)
    {
        Vector3 bottomLeftV = new Vector3(bottomLeftCorner.x, 0, bottomLeftCorner.y);
        Vector3 bottomRightV = new Vector3(topRightCorner.x, 0, bottomLeftCorner.y);
        Vector3 topLeftV = new Vector3(bottomLeftCorner.x, 0, topRightCorner.y);
        Vector3 topRightV = new Vector3(topRightCorner.x, 0, topRightCorner.y);

        Vector3[] vertices = new Vector3[]
        {
            topLeftV,
            topRightV,
            bottomLeftV,
            bottomRightV
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);
        }

        int[] triangles = new int[]
        {
            0,
            1,
            2,
            2,
            1,
            3
        };
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        GameObject dungeonFloor = new GameObject("Mesh" + bottomLeftCorner, typeof(MeshFilter), typeof(MeshRenderer));

        dungeonFloor.transform.position = Vector3.zero;
        dungeonFloor.transform.localScale = Vector3.one;
        dungeonFloor.GetComponent<MeshFilter>().mesh = mesh;
        dungeonFloor.GetComponent<MeshRenderer>().material = material;
        dungeonFloor.transform.parent = transform;

        //Previous code without spacing
        //for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row++)
        //{
        //    var wallPosition = new Vector3(row, 0, bottomLeftV.z);
        //    AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        //}
        //for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row++)
        //{
        //    var wallPosition = new Vector3(row, 0, topRightV.z);
        //    AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        //}
        //for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col++)
        //{
        //    var wallPosition = new Vector3(bottomLeftV.x, 0, col);
        //    AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        //}
        //for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col++)
        //{
        //    var wallPosition = new Vector3(bottomRightV.x, 0, col);
        //    AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        //}

        // First draft

        //for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row += Math.Max(1, wallSpacing))
        //{
        //    var wallPosition = new Vector3(row, 0, bottomLeftV.z);
        //    AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        //}
        //for (int row = (int)topLeftV.x; row < (int)topRightCorner.x; row += Math.Max(1, wallSpacing))
        //{
        //    var wallPosition = new Vector3(row, 0, topRightV.z);
        //    AddWallPositionToList(wallPosition, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        //}
        //for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col += Math.Max(1, wallSpacing))
        //{
        //    var wallPosition = new Vector3(bottomLeftV.x, 0, col);
        //    AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        //}
        //for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col += Math.Max(1, wallSpacing))
        //{
        //    var wallPosition = new Vector3(bottomRightV.x, 0, col);
        //    AddWallPositionToList(wallPosition, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        //}

        //second draft
        //int stride = Math.Max(1, wallSpacing);

        //// bottom horizontal edge -> floor z
        //for (int row = (int)bottomLeftV.x; row < (int)bottomRightV.x; row += stride)
        //{
        //    Vector3Int point = new Vector3Int(row, 0, Mathf.FloorToInt(bottomLeftV.z));
        //    AddWallPositionToList(point, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        //}
        //// top horizontal edge -> ceil z
        //for (int row = (int)topLeftV.x; row < (int)topRightV.x; row += stride)
        //{
        //    Vector3Int point = new Vector3Int(row, 0, Mathf.CeilToInt(topRightV.z));
        //    AddWallPositionToList(point, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
        //}
        //// left vertical edge -> floor x
        //for (int col = (int)bottomLeftV.z; col < (int)topLeftV.z; col += stride)
        //{
        //    Vector3Int point = new Vector3Int(Mathf.FloorToInt(bottomLeftV.x), 0, col);
        //    AddWallPositionToList(point, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        //}
        //// right vertical edge -> ceil x
        //for (int col = (int)bottomRightV.z; col < (int)topRightV.z; col += stride)
        //{
        //    Vector3Int point = new Vector3Int(Mathf.CeilToInt(bottomRightV.x), 0, col);
        //    AddWallPositionToList(point, possibleWallVerticalPosition, possibleDoorVerticalPosition);
        //}

        //third draft
        int stride = Math.Max(1, wallSpacing);

        // compute anchored starts so every room/corridor uses the same stride grid
        int startRow = Mathf.CeilToInt(bottomLeftV.x / (float)stride) * stride;
        int endRow = Mathf.FloorToInt(bottomRightV.x);
        if (startRow < endRow)
        {
            // bottom horizontal edge -> floor z
            for (int row = startRow; row < endRow; row += stride)
            {
                Vector3Int point = new Vector3Int(row, 0, Mathf.FloorToInt(bottomLeftV.z));
                AddWallPositionToList(point, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
            }
        }

        int startRowTop = Mathf.CeilToInt(topLeftV.x / (float)stride) * stride;
        int endRowTop = Mathf.FloorToInt(topRightV.x);
        if (startRowTop < endRowTop)
        {
            // top horizontal edge -> ceil z
            for (int row = startRowTop; row < endRowTop; row += stride)
            {
                Vector3Int point = new Vector3Int(row, 0, Mathf.CeilToInt(topRightV.z));
                AddWallPositionToList(point, possibleWallHorizontalPosition, possibleDoorHorizontalPosition);
            }
        }

        int startCol = Mathf.CeilToInt(bottomLeftV.z / (float)stride) * stride;
        int endCol = Mathf.FloorToInt(topLeftV.z);
        if (startCol < endCol)
        {
            // left vertical edge -> floor x
            for (int col = startCol; col < endCol; col += stride)
            {
                Vector3Int point = new Vector3Int(Mathf.FloorToInt(bottomLeftV.x), 0, col);
                AddWallPositionToList(point, possibleWallVerticalPosition, possibleDoorVerticalPosition);
            }
        }

        int startColRight = Mathf.CeilToInt(bottomRightV.z / (float)stride) * stride;
        int endColRight = Mathf.FloorToInt(topRightV.z);
        if (startColRight < endColRight)
        {
            // right vertical edge -> ceil x
            for (int col = startColRight; col < endColRight; col += stride)
            {
                Vector3Int point = new Vector3Int(Mathf.CeilToInt(bottomRightV.x), 0, col);
                AddWallPositionToList(point, possibleWallVerticalPosition, possibleDoorVerticalPosition);
            }
        }

        //ADDING COLLIDER
        MeshCollider mc= dungeonFloor.GetComponent<MeshCollider>();
        if (mc == null)
        {
            mc = dungeonFloor.AddComponent<MeshCollider>();
        }
        mc.sharedMesh = null;
        mc.sharedMesh = mesh;
        mc.convex = false;

        //dungeonFloor.layer = LayerMask.NameToLayer("Floor");
    }

    //private void AddWallPositionToList(Vector3 wallPosition, List<Vector3Int> wallList, List<Vector3Int> doorList)
    //{
    //    Vector3Int point = Vector3Int.CeilToInt(wallPosition);
    //    if (wallList.Contains(point)){
    //        doorList.Add(point);
    //        wallList.Remove(point);
    //    }
    //    else
    //    {
    //        wallList.Add(point);
    //    }
    //}
    //draft 1
    private void AddWallPositionToList(Vector3Int point, List<Vector3Int> wallList, List<Vector3Int> doorList)
    {
        if (wallList.Contains(point))
        {
            doorList.Add(point);
            wallList.Remove(point);
        }
        else
        {
            wallList.Add(point);
        }
    }

    private void DestroyAllChildren()
    {
        while (transform.childCount != 0)
        {
            foreach (Transform item in transform)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    }
    //private void DestroyAllChildren()
    //{
    //    for (int i = transform.childCount - 1; i >= 0; i--)
    //    {
    //        Destroy(transform.GetChild(i).gameObject);
    //    }
    //}

}
