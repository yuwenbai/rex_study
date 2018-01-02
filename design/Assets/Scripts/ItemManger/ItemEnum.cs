using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ItemEnum
{
    public enum ItemEnumPart
    {
        ItemEnumPart_Base = 0,
        ItemEnumPart_Body = ItemEnumPart_Base,
        ItemEnumPart_Head,
        ItemEnumPart_Hand,
        ItemEnumPart_Foot,
        ItemEnumPart_Weapon,
        ItemEnumPart_Limit
    }
}
