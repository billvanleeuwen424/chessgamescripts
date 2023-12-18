using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class queen : piece
{
    public override List<Vector2Int> GetAvailableMoves(ref piece[,] board, int squarecountX, int squarecountY){
        List<Vector2Int> r = new List<Vector2Int>();

        // Moves:

        // rook like moves:

        // down
        for (int i = currentY-1; i >= 0; i--){
            if(board[currentX, i] == null){
                r.Add(new Vector2Int(currentX,i));
            }
            if(board[currentX, i] != null){
                if(board[currentX, i].team != team){
                    r.Add(new Vector2Int(currentX, i));
                }
                break;
            }
        }

        // up
        for (int i = currentY+1; i < squarecountY; i++){
            if(board[currentX, i] == null){
                r.Add(new Vector2Int(currentX,i));
            }
            if(board[currentX, i] != null){
                if(board[currentX, i].team != team){
                    r.Add(new Vector2Int(currentX, i));
                }
                break;
            }
        }

        //left 
        for (int i = currentX-1; i >= 0; i--){
            if(board[i, currentY] == null){
                r.Add(new Vector2Int(i,currentY));
            }
            if(board[i, currentY] != null){
                if(board[i, currentY].team != team){
                    r.Add(new Vector2Int(i, currentY));
                }
                break;
            }
        }

        // right
        for (int i = currentX+1; i < squarecountX; i++){
            if(board[i, currentY] == null){
                r.Add(new Vector2Int(i,currentY));
            }
            if(board[i, currentY] != null){
                if(board[i, currentY].team != team){
                    r.Add(new Vector2Int(i, currentY));
                }
                break;
            }
        }




        // bishop like moves:

                for (int x = currentX + 1, y = currentY + 1; x < squarecountX && y < squarecountY ; x++, y++){
            if(board[x,y] == null){
                r.Add(new Vector2Int(x,y));
            }

            else {
                if (board[x,y].team != team ){
                    r.Add(new Vector2Int(x,y));
                }
                break;
            }
        }

        // top left
        for (int x = currentX - 1, y = currentY + 1; x >= 0 && y < squarecountY ; x--, y++){
            if(board[x,y] == null){
                r.Add(new Vector2Int(x,y));
            }

            else {
                if (board[x,y].team != team ){
                    r.Add(new Vector2Int(x,y));
                }
                break;
            }
        }

        // bottom right
        for (int x = currentX + 1, y = currentY - 1; x < squarecountX && y >= 0 ; x++, y--){
            if(board[x,y] == null){
                r.Add(new Vector2Int(x,y));
            }

            else {
                if (board[x,y].team != team ){
                    r.Add(new Vector2Int(x,y));
                }
                break;
            }
        }

        // bottom left
        for (int x = currentX - 1, y = currentY - 1; x >= 0 && y >= 0 ; x--, y--){
            if(board[x,y] == null){
                r.Add(new Vector2Int(x,y));
            }

            else {
                if (board[x,y].team != team ){
                    r.Add(new Vector2Int(x,y));
                }
                break;
            }
        }

        return r;
    }
}
