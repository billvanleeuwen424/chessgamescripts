using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn : piece
{
    public override List<Vector2Int> GetAvailableMoves(ref piece[,] board, int squarecountX, int squarecountY){
        List<Vector2Int> r = new List<Vector2Int>();

        // either white or black changes direction
        int direction = (team == 0) ? 1 : -1;

        // Moves:

        // check to make sure we dont overindex the board array
        if(currentY + direction >= squarecountY){
            goto End;   // we dont want to add any moves to the list, since pawn can only move forward
                        // the pawn should be promoted at this point so it shouldnt matter, but this is just for safety :)
        }

        // 1 move forward
        if(board[currentX, currentY + direction] == null){
            r.Add(new Vector2Int(currentX, currentY + direction));
        }

        // 2 places forward
        if(board[currentX, currentY + direction] == null){
            if(team == 0 && currentY == 1 && board[currentX, currentY + (direction * 2)] == null){
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            }
            if(team == 1 && currentY == 6 && board[currentX, currentY + (direction * 2)] == null){
                r.Add(new Vector2Int(currentX, currentY + (direction * 2)));
            }
        }

        //kill move
        if(currentX != squarecountX-1){
            if(board[currentX+1, currentY + direction] != null && board[currentX+1, currentY + direction].team != team){
                r.Add(new Vector2Int(currentX+1, currentY + direction));
            }
        }
        if(currentX != 0){
            if(board[currentX-1, currentY + direction] != null && board[currentX-1, currentY + direction].team != team){
                r.Add(new Vector2Int(currentX-1, currentY + direction));
            }
        }
        End:
            return r;
    }
}
