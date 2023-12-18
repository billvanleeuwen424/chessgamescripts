using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    [Header("Art")]
    [SerializeField] private Material squareMaterial;
    [SerializeField] private float squareSize = 1.0f;
    [SerializeField] private float yOffset = 0.2f;
    [SerializeField] private Vector3 boardCenter = Vector3.zero;
    [SerializeField] private float deathSize = 0.3f;
    [SerializeField] private float deathSpacing = 0.3f;
    [SerializeField] private float dragOffset = 1.0f;

    [Header("Prefabs and Materials")]
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private Material[] teamMaterials;


    private piece[,] chessPieces;
    private piece currentlyDragging;
    private List<Vector2Int> availableMoves = new List<Vector2Int>();
    private List<piece> deadWhites = new List<piece>();
    private List<piece> deadBlacks = new List<piece>();
    private const int SQUARE_COUNT = 8;  //8x8 chess board

    private GameObject[,] squares;
    private Camera currentCamera;
    private Vector2Int currentHover;
    private Vector3 bounds;

    private void Awake(){
        GenerateGrid();
        SpawnAllPieces();
        PositionAllPieces();
    }

    private void Update(){
        if(!currentCamera){
            currentCamera = Camera.main;
            return;
        }

        RaycastHit info;
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Square", "Hover", "Highlight"))){
            // get index of hit square
            Vector2Int hitPosition = LookupSquareIndex(info.transform.gameObject);

            // hovering over the square after not hovering over anything
            if (currentHover == -Vector2Int.one){
                currentHover = hitPosition;
                squares[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            // if already hovering a tile, change previous
            if (currentHover != hitPosition){
                squares[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Square");
                currentHover = hitPosition;
                squares[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            // on press down of mouse
            if(Input.GetMouseButtonDown(0)){
                if(chessPieces[hitPosition.x, hitPosition.y] != null){
                    // is it our turn?
                    if (true){
                        currentlyDragging = chessPieces[hitPosition.x,hitPosition.y];


                        // get a list of where I can go
                        availableMoves = currentlyDragging.GetAvailableMoves(ref chessPieces, SQUARE_COUNT, SQUARE_COUNT);
                        HiglightSquares();

                    }
                }
            }

            // releasing mouse button
            if(currentlyDragging != null && Input.GetMouseButtonUp(0)){
                Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);


                bool validMove = MoveTo(currentlyDragging, hitPosition.x, hitPosition.y);
                if(!validMove){
                    currentlyDragging.SetPosition(GetTileCenter(previousPosition.x, previousPosition.y));
                }

                RemoveHiglightSquares();
                currentlyDragging = null;
            }   
        }
        else{
            if(currentHover != -Vector2Int.one){
                squares[currentHover.x, currentHover.y].layer = (ContainsValidMove(ref availableMoves, currentHover)) ? LayerMask.NameToLayer("Highlight") : LayerMask.NameToLayer("Square");
                currentHover = -Vector2Int.one;
            }

            if(currentlyDragging && Input.GetMouseButtonUp(0)){
                currentlyDragging.SetPosition(GetTileCenter(currentlyDragging.currentX, currentlyDragging.currentY));
                currentlyDragging = null;
                RemoveHiglightSquares();
            }
        }

        // if we're dragging and
        if(currentlyDragging){
            Plane horizontalPlane = new Plane(Vector3.up, Vector3.up * yOffset);
            float distance = 0.0f;
            if(horizontalPlane.Raycast(ray, out distance)){
                currentlyDragging.SetPosition(ray.GetPoint(distance) + Vector3.up * dragOffset);
            }
        }
    }

    private void HiglightSquares()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            squares[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Highlight"); 
        }
    }

    private void RemoveHiglightSquares()
    {
        for (int i = 0; i < availableMoves.Count; i++)
        {
            squares[availableMoves[i].x, availableMoves[i].y].layer = LayerMask.NameToLayer("Square"); 
        }

        availableMoves.Clear();
    }

    private bool MoveTo(piece currentlyDragging, int x, int y)
    {
        if(!ContainsValidMove(ref availableMoves, new Vector2(x,y))){
            return false;
        }
        Vector2Int previousPosition = new Vector2Int(currentlyDragging.currentX, currentlyDragging.currentY);

        //is there another piece on the target position?
        if(chessPieces[x,y] != null){
            piece ocp = chessPieces[x,y];

            // if our team
            if(currentlyDragging.team == ocp.team){
                return false;
            }

            //if enemy team
            if(ocp.team == 0){
                deadWhites.Add(ocp);
                ocp.setScale(Vector3.one * deathSize);
                ocp.SetPosition(
                    new Vector3(8 * squareSize, yOffset, -1 * squareSize)
                    - bounds 
                    + new Vector3(squareSize / 2, 0, squareSize / 2)
                    + Vector3.forward * deathSpacing * deadWhites.Count);

            }
            else{
                deadBlacks.Add(ocp);
                ocp.setScale(Vector3.one * deathSize);
                ocp.SetPosition(
                    new Vector3(-1 * squareSize, yOffset, 8 * squareSize)
                    - bounds 
                    + new Vector3(squareSize / 2, 0, squareSize / 2)
                    + Vector3.back * deathSpacing * deadBlacks.Count);
            }

        }


        chessPieces[x,y] = currentlyDragging;
        chessPieces[previousPosition.x, previousPosition.y] = null;

        PositionSinglePiece(x,y);

        return true;
    }

    /// <summary>
    /// loops over GenerateSinglesquare to generate the board
    /// </summary>
    private void GenerateGrid(){
    
        yOffset += transform.position.y;
        bounds = new Vector3(SQUARE_COUNT / 2 * squareSize, 0, SQUARE_COUNT / 2 * squareSize) + boardCenter;

        squares = new GameObject[SQUARE_COUNT, SQUARE_COUNT];
        for(int x = 0; x < SQUARE_COUNT; x++){
            for(int y = 0; y < SQUARE_COUNT; y++){
                squares[x,y] = GenerateSinglesquare(x,y);
            }
        }
    }

    /// <summary>
    /// generates a single square
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private GameObject GenerateSinglesquare(int x, int y){
        GameObject squareObject = new GameObject(string.Format("X:{0}, Y:{1}",x,y));
        squareObject.transform.parent = transform;

        // create mesh
        Mesh mesh = new Mesh();
        squareObject.AddComponent<MeshFilter>().mesh = mesh;
        squareObject.AddComponent<MeshRenderer>().material = squareMaterial;

        // define vertices and order
        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * squareSize, yOffset, y * squareSize) - bounds;
        vertices[1] = new Vector3(x * squareSize, yOffset, (y + 1) * squareSize) - bounds;
        vertices[2] = new Vector3((x + 1) * squareSize, yOffset, y * squareSize) - bounds;
        vertices[3] = new Vector3((x + 1) * squareSize, yOffset, (y + 1) * squareSize) - bounds;

        int[] triangles = new int[] {0,1,2,1,3,2};


        // set the mesh and normals
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();


        squareObject.layer = LayerMask.NameToLayer("Square");

        squareObject.AddComponent<BoxCollider>();

        return squareObject; 
    }
    

    private void SpawnAllPieces(){
        chessPieces = new piece[SQUARE_COUNT, SQUARE_COUNT];

        int whiteTeam = 0,blackTeam = 1;

        //spawn all the pieces iteratively
        chessPieces[0,0] = SpawnSinglePiece(pieceType.rook, whiteTeam);
        chessPieces[1,0] = SpawnSinglePiece(pieceType.knight, whiteTeam);
        chessPieces[2,0] = SpawnSinglePiece(pieceType.bishop, whiteTeam);
        chessPieces[3,0] = SpawnSinglePiece(pieceType.queen, whiteTeam);
        chessPieces[4,0] = SpawnSinglePiece(pieceType.king, whiteTeam);
        chessPieces[5,0] = SpawnSinglePiece(pieceType.bishop, whiteTeam);
        chessPieces[6,0] = SpawnSinglePiece(pieceType.knight, whiteTeam);
        chessPieces[7,0] = SpawnSinglePiece(pieceType.rook, whiteTeam);
        for (int i = 0; i < SQUARE_COUNT; i++)
        {
            chessPieces[i,1] = SpawnSinglePiece(pieceType.pawn, whiteTeam);
        }

        chessPieces[0,7] = SpawnSinglePiece(pieceType.rook, blackTeam);
        chessPieces[1,7] = SpawnSinglePiece(pieceType.knight, blackTeam);
        chessPieces[2,7] = SpawnSinglePiece(pieceType.bishop, blackTeam);
        chessPieces[3,7] = SpawnSinglePiece(pieceType.queen, blackTeam);
        chessPieces[4,7] = SpawnSinglePiece(pieceType.king, blackTeam);
        chessPieces[5,7] = SpawnSinglePiece(pieceType.bishop, blackTeam);
        chessPieces[6,7] = SpawnSinglePiece(pieceType.knight, blackTeam);
        chessPieces[7,7] = SpawnSinglePiece(pieceType.rook, blackTeam);
        for (int i = 0; i < SQUARE_COUNT; i++)
        {
            chessPieces[i,6] = SpawnSinglePiece(pieceType.pawn, blackTeam);
        }

    }
    

    private piece SpawnSinglePiece(pieceType type, int team){
        piece p = Instantiate(prefabs[(int)type], transform).GetComponent<piece>();

        p.type = type;
        p.team = team;
        p.GetComponent<MeshRenderer>().material = teamMaterials[team];
        return p;
    }
    
    
    private void PositionAllPieces(){
        for (int x = 0; x < SQUARE_COUNT; x++)
        {
            for (int y = 0; y < SQUARE_COUNT; y++)
            {
                if(chessPieces[x,y] != null){
                    PositionSinglePiece(x,y, true);
                }
            }
        }
    }

    private void PositionSinglePiece(int x, int y, bool force = false){

        chessPieces[x,y].currentX = x;
        chessPieces[x,y].currentY = y;
        chessPieces[x,y].SetPosition(GetTileCenter(x,y), force);
    }

    private Vector3 GetTileCenter(int x, int y){
        return new Vector3(x * squareSize, yOffset, y *squareSize) - bounds + new Vector3(squareSize / 2, 0, squareSize / 2);
    }

    private Vector2Int LookupSquareIndex(GameObject hitInfo){
        Vector2Int returnVec = -Vector2Int.one; // base case, -1, -1 this will crash the game
        
        for (int x = 0; x < SQUARE_COUNT; x++)
        {
            for (int y = 0; y < SQUARE_COUNT; y++)
            {
                if(squares[x,y] == hitInfo){
                    returnVec.Set(x,y);
                    goto LoopEnd;   // break out
                }
            }
        }
        LoopEnd:
            return returnVec; 
    }

    private bool ContainsValidMove(ref List<Vector2Int> moves, Vector2 pos){
        for (int i = 0; i < moves.Count; i++){
            if(moves[i].x == pos.x && moves[i].y == pos.y){
                return true;
            }
        }
        return false;
    }
}
