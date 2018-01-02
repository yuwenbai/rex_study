using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class CharacterManager
{
    private Queue<Character> mQueueCharacter = new Queue<Character>();

    public CharacterManager()
    {
        for (int i = 0; i < 10; ++i)
        {
            Character mCharacter = new Character();
            mQueueCharacter.Enqueue(mCharacter);
        }
    }
    public Character GetCharacter()
    {
        Character mCharacter = null;

        if (mQueueCharacter.Count > 0)
        {
            mCharacter = mQueueCharacter.Peek();
        }
        else
        {
            mCharacter = new Character();
        }
        return mCharacter;
    }

    public void ReCollectStack(Character character)
    {
        mQueueCharacter.Enqueue(character);
    }
    public void Clear()
    {
        mQueueCharacter.Clear();
    }
}