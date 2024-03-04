using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eActivableColor
{
    RED,
    GREEN,
    BLUE
}

// ���� �÷��� ��� �Ŵ���
public class PlayManager : SingletonBehavior<PlayManager>
{
    public static readonly string PLAYER_TAG = "Player"; 


    private List<eActivableColor> activationColors = new List<eActivableColor>();
    public bool ContainsActivationColors(eActivableColor color) => activationColors.Contains(color);

    public eActivableColor ActivationColors
    {
        set
        {
            if (!activationColors.Contains(value))
            {
                activationColors.Add(value);
            }
        }
    }

    protected override void OnAwake()
    {

    }
}
