using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GeneratorScript : MonoBehaviour
{
    public enum CharEncoding{
        BLOC_DOWN = 'v',
        BLOC_RIGHT = '>',
        BLOC_LEFT = '<',
        BLOC_UP = '^',
        BLOC_POINT = 'o',
        MINE = '#',
        OBSTACLE = '$',
        NO_BLOC = 'x',
        SEPARATOR = ';'
    }
    
    public string mapPath;

    private double blocSpeed;
    private string soundPath;
    private double blocsPerSecond;
    private double secBetweenBlocs;
    private List<string> allBlocs= new List<string>();
    private double timeSinceLastBloc = 0;
    private int idBloc = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(File.Exists(mapPath)){
            StreamReader sr = new StreamReader(File.OpenRead(mapPath));
            string[] firstLine= sr.ReadLine().Split((char)CharEncoding.SEPARATOR);

            blocSpeed = Convert.ToDouble(firstLine[0]);
            soundPath = firstLine[1];
            blocsPerSecond = Convert.ToDouble(firstLine[2]);
            secBetweenBlocs = 1.0/blocsPerSecond;
            string str = null;
            while((str = sr.ReadLine()) != null){
                allBlocs.Add(str);
            }
        }
    }
    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        timeSinceLastBloc += Time.deltaTime;
        if(timeSinceLastBloc >= secBetweenBlocs){
            timeSinceLastBloc -= secBetweenBlocs;
            if(allBlocs.Count < idBloc){
                //get blocs to create
                string currentBlocs = allBlocs[idBloc];
                idBloc++;
                
                ReadBlocLine(currentBlocs);
            }
        }
    }
    public enum BLOCLINE_SEPARATION {
        BLUE_DOWN,BLUE_MIDDLE,BLUE_UP,
        RED_DOWN,RED_MIDDLE,RED_UP,
        BOMB_DOWN,BOMB_MIDDLE,BOMB_UP,
        OBSTACLE_VERTICAL,OBSTACLE_HORIZONTAL}
    public void ReadBlocLine(string blocLine){
        int sep = 0;
        string[] blocs = blocLine.Split((char)CharEncoding.SEPARATOR);
        while(sep <= 10){
            if(blocs[sep].Length != 0){
                Position aPosition = (Position)(4-blocs[sep].Length);
                Direction aDirection = Direction.ANY;
                for (int i = 0; i < blocs[sep].Length; i++)
                {
                    switch((CharEncoding)blocs[sep][i]){
                        case CharEncoding.BLOC_DOWN:
                            aDirection = Direction.DOWN;
                            break;
                        case CharEncoding.BLOC_LEFT:
                            aDirection = Direction.LEFT;
                            break;
                        case CharEncoding.BLOC_RIGHT:
                            aDirection = Direction.RIGHT;
                            break;
                        case CharEncoding.BLOC_UP:
                            aDirection = Direction.UP;
                            break;
                        case CharEncoding.MINE:
                        case CharEncoding.BLOC_POINT:
                        case CharEncoding.NO_BLOC:    
                        case CharEncoding.OBSTACLE:
                        default:
                            break;

                    }
                switch((BLOCLINE_SEPARATION)sep){
                    case BLOCLINE_SEPARATION.BLUE_DOWN:
                        CreateBloc(BlocColor.BLUE,aDirection,Height.BOTTOM,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.BLUE_MIDDLE:
                        CreateBloc(BlocColor.BLUE,aDirection,Height.MIDDLE,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.BLUE_UP:
                        CreateBloc(BlocColor.BLUE,aDirection,Height.TOP,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.RED_DOWN:
                        CreateBloc(BlocColor.RED,aDirection,Height.BOTTOM,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.RED_MIDDLE:
                        CreateBloc(BlocColor.RED,aDirection,Height.MIDDLE,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.RED_UP:
                        CreateBloc(BlocColor.RED,aDirection,Height.TOP,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.BOMB_DOWN:
                        CreateBomb(Height.BOTTOM,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.BOMB_MIDDLE:
                        CreateBomb(Height.MIDDLE,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.BOMB_UP:
                        CreateBomb(Height.TOP,aPosition);
                        break;
                    case BLOCLINE_SEPARATION.OBSTACLE_VERTICAL:
                        CreateVerticalObstacle(aPosition);
                        break;
                    case BLOCLINE_SEPARATION.OBSTACLE_HORIZONTAL:
                        CreateHorizontalObstacle();
                        break;
                    default:
                        break;

                }     
                }
            }
            sep++;
        }
    }
    public enum BlocColor{BLUE,RED};
    public enum Direction{UP,DOWN,LEFT,RIGHT,ANY}
    public enum Height{BOTTOM,TOP,MIDDLE}
    public enum Position{FAR_LEFT,MIDDLE_LEFT,MIDDLE_RIGHT,FAR_RIGHT}
    public void CreateBloc(BlocColor aBlocColor,Direction aDirection,Height anHeight,Position aPosition){
        switch(aDirection){
            case Direction.UP:
            case Direction.DOWN:
            case Direction.LEFT:
            case Direction.RIGHT:
                break;
            case Direction.ANY:
                break;
            default:
                break;
        }
    }
    public void CreateBomb(Height anHeight,Position aPosition){

    }
    public void CreateVerticalObstacle(Position aPosition){

    }
    public void CreateHorizontalObstacle(){

    }
}
