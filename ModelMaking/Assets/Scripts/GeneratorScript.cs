using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class GeneratorScript : MonoBehaviour
{
    #region enumConst
        public enum BlocColor{BLUE,RED};
        public enum Direction{UP,DOWN,LEFT,RIGHT,ANY}
        public enum Height{BOTTOM,TOP,MIDDLE}
        public enum Position{FAR_LEFT,MIDDLE_LEFT,MIDDLE_RIGHT,FAR_RIGHT}
        public enum BLOCLINE_SEPARATION {
            BLUE_DOWN,BLUE_MIDDLE,BLUE_UP,
            RED_DOWN,RED_MIDDLE,RED_UP,
            BOMB_DOWN,BOMB_MIDDLE,BOMB_UP,
            OBSTACLE_VERTICAL,OBSTACLE_HORIZONTAL}

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
    #endregion

    #region varsToDefine
        public GameObject blueSlice;
        public GameObject bluePoint;
        public GameObject redSlice;
        public GameObject redPoint;
        public GameObject bomb;
        public GameObject obstacle;
        
        public string mapPath;
    #endregion

    #region privateVars
        private float blocSpeed;
        private string soundPath;
        private double blocsPerSecond;
        private double secBetweenBlocs;
        private List<string> allBlocs= new List<string>();
        private double timeSinceLastBloc = 0;
        private int idBloc = 0;
        private List<GameObject> instantiedBlocs = new List<GameObject>();
        const float X_OFFSET_POWER = 1.0f;
        const float Y_OFFSET_POWER = 0.5f;
        const float SEC_BEFORE_SONG_START = 10.0f; 
        private AudioSource audioSource;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        if(File.Exists(mapPath)){
            //ReadFile
            StreamReader sr = new StreamReader(File.OpenRead(mapPath));
            string[] firstLine= sr.ReadLine().Split((char)CharEncoding.SEPARATOR);

            blocSpeed = (float)Convert.ToDouble(firstLine[0]);
            soundPath = firstLine[1];
            this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z+blocSpeed*SEC_BEFORE_SONG_START);
            blocsPerSecond = Convert.ToDouble(firstLine[2]);
            secBetweenBlocs = 1.0/blocsPerSecond;
            string str = null;
            while((str = sr.ReadLine()) != null){
                allBlocs.Add(str);
            }
            sr.Close();

            //Load music
            audioSource = GetComponent<AudioSource>() ;
            StartCoroutine(LoadSong());

            idBloc = 0;

        }else{
            Debug.Log("Didn't find the map");
        }
    }

    IEnumerator LoadSong(){
        using(UnityWebRequest webRequest = UnityWebRequestMultimedia.GetAudioClip("file:///"+soundPath,AudioType.WAV)){
            yield return webRequest.SendWebRequest();
            if(webRequest.isNetworkError){
                Debug.Log(webRequest.error);
            }else{
                
                audioSource.clip = DownloadHandlerAudioClip.GetContent(webRequest);
                audioSource.PlayDelayed(SEC_BEFORE_SONG_START);
            }
        }
    }

    /// <summary>
    /// This function is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        timeSinceLastBloc += Time.fixedDeltaTime;
        if(timeSinceLastBloc >= secBetweenBlocs){
            timeSinceLastBloc -= secBetweenBlocs;
            if(allBlocs.Count > idBloc){
                //get blocs to create
                string currentBlocs = allBlocs[idBloc];
                    Debug.Log(idBloc);
                idBloc++;
                ReadBlocLine(currentBlocs);
            }
        }
    }   
    
    public void ReadBlocLine(string blocLine){
        int sep = 0;
        string[] blocs = blocLine.Split((char)CharEncoding.SEPARATOR);
        while(sep <= 10){
            if(blocs[sep].Length != 0){
                for (int i = 0; i < blocs[sep].Length; i++)
                    {
                        //Set Direction
                        Direction aDirection = Direction.ANY;
                        //Set Position
                        Position aPosition = (Position)(4-blocs[sep].Length-i);
                        //Change Direction
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

                        //Create the Bloc
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
                }//end for
            }//end if
            sep++;
        }//end while(sep)
    }



    public void CreateBloc(BlocColor aBlocColor,Direction aDirection,Height anHeight,Position aPosition){
        //Calculate orientation
        int orientation = 0;
        switch(aDirection){
            case Direction.UP:
                orientation = 180;
                break;
            case Direction.DOWN:
                break;
            case Direction.LEFT:
                orientation = 90;
                break;
            case Direction.RIGHT:
                orientation = 270;
                break;
            case Direction.ANY:
            default:
                break;
        }
        //Calculate Height
        float yOffset = 0f;
        switch (anHeight)
        {
            case Height.BOTTOM:
                yOffset = -1.0f * Y_OFFSET_POWER;
                break;
            case Height.TOP:
                yOffset = 1.0f * Y_OFFSET_POWER;
                break;
            case Height.MIDDLE:
            default:
                break;
        }
        //Calculate Position
        float xOffset = 0f;
        switch (aPosition)
        {
            case Position.FAR_LEFT:
                xOffset = -1.0f * X_OFFSET_POWER;
                break;
            case Position.MIDDLE_LEFT:
                xOffset = -0.33f * X_OFFSET_POWER;
                break;
            case Position.MIDDLE_RIGHT:
                xOffset = 0.33f * X_OFFSET_POWER;
                break;
            case Position.FAR_RIGHT:
                xOffset = 1.0f * X_OFFSET_POWER;
                break;
            default:
                break;
        }
        //calculate bloc to use
        GameObject toInstantiate;
        if(aBlocColor == BlocColor.BLUE){
            if(aDirection == Direction.ANY){
                toInstantiate = bluePoint;
            }else{
                toInstantiate = blueSlice;
            }
        }else{
            if(aDirection == Direction.ANY){
                toInstantiate = redPoint;
            }else{
                toInstantiate = redSlice;
            }
        }
        //Define variables for the object
        Vector3 startPosition = new Vector3(this.transform.position.x+xOffset,this.transform.position.y+yOffset,this.transform.position.z);
        Quaternion startRotation = Quaternion.Euler(orientation,90,0);
        
        GameObject gmObjct = Instantiate(toInstantiate,startPosition,startRotation);
        gmObjct.GetComponent<BlocScript>().speed = blocSpeed;
        //Debug.Log("Bloc created");
        instantiedBlocs.Add(gmObjct);
    }
    public void CreateBomb(Height anHeight,Position aPosition){
        //Calculate Height
        float yOffset = 0f;
        switch (anHeight)
        {
            case Height.BOTTOM:
                yOffset = -1.0f * Y_OFFSET_POWER;
                break;
            case Height.TOP:
                yOffset = 1.0f * Y_OFFSET_POWER;
                break;
            case Height.MIDDLE:
            default:
                break;
        }
        //Calculate Position
        float xOffset = 0f;
        switch (aPosition)
        {
            case Position.FAR_LEFT:
                xOffset = -1.0f * X_OFFSET_POWER;
                break;
            case Position.MIDDLE_LEFT:
                xOffset = -0.33f * X_OFFSET_POWER;
                break;
            case Position.MIDDLE_RIGHT:
                xOffset = 0.33f * X_OFFSET_POWER;
                break;
            case Position.FAR_RIGHT:
                xOffset = 1.0f * X_OFFSET_POWER;
                break;
            default:
                break;
        }
        
        //Define variables for the object
        Vector3 startPosition = new Vector3(this.transform.position.x+xOffset,this.transform.position.y+yOffset,this.transform.position.z);
        Quaternion startRotation = new Quaternion(bomb.transform.rotation.x,bomb.transform.rotation.y,+bomb.transform.rotation.z,1);;

        GameObject gmObjct = Instantiate(bomb,startPosition,startRotation);
        
        Debug.Log("Bomb created");
        
        instantiedBlocs.Add(gmObjct);
    }
    public void CreateVerticalObstacle(Position aPosition){        
        //Calculate Position
        float xOffset = 0f;
        switch (aPosition)
        {
            case Position.FAR_LEFT:
                xOffset = -1.0f * X_OFFSET_POWER;
                break;
            case Position.MIDDLE_LEFT:
                xOffset = -0.5f * X_OFFSET_POWER;
                break;
            case Position.MIDDLE_RIGHT:
                xOffset = 0.5f * X_OFFSET_POWER;
                break;
            case Position.FAR_RIGHT:
                xOffset = 1.0f * X_OFFSET_POWER;
                break;
            default:
                break;
        }
        //Define variables for the object
        Vector3 startPosition = new Vector3(this.transform.position.x+xOffset,this.transform.position.y,this.transform.position.z);
        Quaternion startRotation = new Quaternion(0,0,0,1);
        GameObject gmObjct = Instantiate(obstacle,startPosition,startRotation);

        Debug.Log("Vertical Obstacle created");
        instantiedBlocs.Add(gmObjct);
    }
    public void CreateHorizontalObstacle(){
        //Define variables for the object
        Vector3 startPosition = new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z);
        Quaternion startRotation = new Quaternion(0,0,90,1);

        GameObject gmObjct = Instantiate(obstacle,startPosition,startRotation);

        Debug.Log("Horizontal Obstacle created");
        
        instantiedBlocs.Add(gmObjct);
    }
}
