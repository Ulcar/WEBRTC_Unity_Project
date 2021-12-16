using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{

    // Use this for initialization
    [SerializeField]
    CharacterStats stats;

    CharacterMovement movement;




}


[CreateAssetMenu]
public class CharacterStats : ScriptableObject 
{
    public int movespeed;
    public Vector2 TilePos;
}