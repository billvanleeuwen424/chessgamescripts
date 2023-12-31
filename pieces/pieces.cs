using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum pieceType{
    pawn = 0,
    rook = 1,
    knight = 2,
    bishop = 3,
    queen = 4,
    king = 5
};

public class piece : MonoBehaviour
{
    public int team;
    public int currentX;
    public int currentY;

    public pieceType type;

    private Vector3 desiredPosition;
    private Vector3 desiredScale = Vector3.one;


    private void Update(){
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * 10);
        transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, Time.deltaTime * 10);
    }
    
    public virtual void SetPosition(Vector3 position, bool force = false){
        desiredPosition = position;
        if(force){
            transform.position = desiredPosition;
        }
    }

    public virtual void setScale(Vector3 scale, bool force = false){
        desiredScale = scale;
        if(force){
            transform.position = desiredPosition;
        }
    }

    public virtual List<Vector2Int> GetAvailableMoves(ref piece[,] board, int squarecountX, int squarecountY){
        List<Vector2Int> r = new List<Vector2Int>();

        r.Add(new Vector2Int(3,3));
        r.Add(new Vector2Int(3,4));
        r.Add(new Vector2Int(4,3));
        r.Add(new Vector2Int(4,4));

        return r;
    }
}
