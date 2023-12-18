using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king : piece
{
    public override List<Vector2Int> GetAvailableMoves(ref piece[,] board, int squarecountX, int squarecountY){
        List<Vector2Int> r = new List<Vector2Int>();

        // Moves:

        // right side moves
        if (currentX + 1 < squarecountX){
            //right
            if (board[currentX + 1, currentY] == null){
                r.Add(new Vector2Int(currentX+1, currentY));
            }
            else if(board[currentX+1, currentY].team != team){
                r.Add(new Vector2Int(currentX + 1, currentY));
            }

            //top right
            if(currentY + 1 < squarecountY){
                if (board[currentX + 1, currentY + 1] == null){
                    r.Add(new Vector2Int(currentX+1, currentY + 1));
                }
                else if(board[currentX+1, currentY + 1].team != team){
                    r.Add(new Vector2Int(currentX + 1, currentY + 1));
                }
            }

            //bottom right
            if(currentY - 1 >= 0){
                if (board[currentX + 1, currentY - 1] == null){
                    r.Add(new Vector2Int(currentX+1, currentY - 1));
                }
                else if(board[currentX+1, currentY - 1].team != team){
                    r.Add(new Vector2Int(currentX + 1, currentY - 1));
                }
            }
        }


        // left side moves
        if (currentX - 1 >= 0){
            // left
            if (board[currentX - 1, currentY] == null){
                r.Add(new Vector2Int(currentX - 1, currentY));
            }
            else if(board[currentX - 1, currentY].team != team){
                r.Add(new Vector2Int(currentX - 1, currentY));
            }

            //top left
            if(currentY + 1 < squarecountY){
                if (board[currentX - 1, currentY + 1] == null){
                    r.Add(new Vector2Int(currentX - 1, currentY + 1));
                }
                else if(board[currentX - 1, currentY + 1].team != team){
                    r.Add(new Vector2Int(currentX - 1, currentY + 1));
                }
            }

            //bottom left
            if(currentY - 1 >= 0){
                if (board[currentX - 1, currentY - 1] == null){
                    r.Add(new Vector2Int(currentX - 1, currentY - 1));
                }
                else if(board[currentX-1, currentY - 1].team != team){
                    r.Add(new Vector2Int(currentX - 1, currentY - 1));
                }
            }
        }

        // down
        if(currentY - 1 >= 0){
            if (board[currentX, currentY - 1] == null){
                r.Add(new Vector2Int(currentX, currentY - 1));
            }
            else if(board[currentX, currentY - 1].team != team){
                r.Add(new Vector2Int(currentX, currentY - 1));
            }
        }
        

        // up
        if(currentY + 1 < squarecountY){
            if (board[currentX, currentY + 1] == null){
                r.Add(new Vector2Int(currentX, currentY + 1));
            }
            else if(board[currentX, currentY + 1].team != team){
                r.Add(new Vector2Int(currentX, currentY + 1));
            }
        }

        return r;
    }
}
