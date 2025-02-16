

public class NewId 
{
    public int GetNewLeftId(int[] idState, int dataSize)
    {
        idState[2] = idState[1];
        idState[1] = idState[0];
        if (idState[0] == 0)//これ以上左にオブジェクトが存在しない場合
        {
            idState[0] = dataSize - 1;//最後尾に戻る

        }
        else
        {
            idState[0] = idState[0] - 1;
        }

        return idState[0];
    }

    public int GetNewRightId(int[] idState, int dataSize)
    {
        idState[0] = idState[1];
        idState[1] = idState[2];
        if (idState[2] == dataSize - 1)
        {
            idState[2] = 0;//先頭に戻る
        }
        else
        {
            idState[2] = idState[2] + 1;
        }
        return idState[2];
    }
}
