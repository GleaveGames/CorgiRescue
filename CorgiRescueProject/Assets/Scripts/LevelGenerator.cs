using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelGenerator : MonoBehaviour
{
    /* 0  -  LeftRight
     * 1  -  BackForward
     * 2  -  RightForward
     * 3  -  LeftForward
     * 4  -  BackRight
     * 5  -  BackLeft
     * 6  -  NonPath 
     * 7  -  LeftEntry
     * 8  -  ForwardEntry
     * 9  -  RightEntry
     * 10  -  LeftExit
     * 11  -  BackExit
     * 12  -  RightExit
     * 13  -  Shop
     * 14  -  Locked
     * 15  -  MoleBoss
     */

    //   Filled Room    Instantiate(rooms[6].rooms[0], nodes[i].transform.position, Quaternion.identity);


    [Header("Chance of spawn (1 in X)")]
    [SerializeField]
    private float DiamondChance;
    [SerializeField]
    private float GoldChance;
    [SerializeField]
    private float SilverChance;
    [SerializeField]
    private float SnekChance;
    [SerializeField]
    private float MoleChance;
    [SerializeField]
    private float BatChance;
    [SerializeField]
    private float MongChance;
    [SerializeField]
    private float BombyChance;
    [SerializeField]
    private float PebbleChance;
    [SerializeField]
    private float WoodCrateChance;
    [SerializeField]
    private float MetalCrateChance;
    [SerializeField]
    private float SpikeChance;
    [SerializeField]
    private float ArrowTrapChance;




    [SerializeField]
    private bool LV0;
    [SerializeField]
    private bool MoleBoss;

    public List<GameObject> itemsForPickUp;
    public List<GameObject> livingThings;
    [SerializeField]
    private RoomType[] rooms;
    [SerializeField]
    private GameObject[] nodes;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject creature;
    private Vector3 spawnPos;

    [Header("Tiles")] 
    [SerializeField]
    private TileBase DirtRule;
    [SerializeField]
    private TileBase SnowRule;
    [SerializeField]
    private TileBase RockRule;
    [SerializeField]
    private TileBase ObsidianRule;
    [SerializeField]
    private TileBase Wood;
    [SerializeField]
    private TileBase Diamond;  
    [SerializeField]
    private TileBase DiamondRock;    
    [SerializeField]
    private TileBase Gold;   
    [SerializeField]
    private TileBase Silver;
    [SerializeField]
    private TileBase GoldRock;   
    [SerializeField]
    private TileBase SilverRock;

    [Header("Enemeis")]
    [SerializeField]
    private GameObject snek;
    [SerializeField]
    private bool snekEnabled;
    [SerializeField]
    private GameObject bat;
    [SerializeField]
    private bool batEnabled;
    [SerializeField]
    private GameObject mole;
    [SerializeField]
    private bool moleEnabled;
    [SerializeField]
    private GameObject mong;
    [SerializeField]
    private bool mongEnabled;
    [SerializeField]
    private GameObject bomby;
    [SerializeField]
    private bool bombyEnabled;

    [Header("Other")]
    [SerializeField]
    private GameObject pebble;
    [SerializeField]
    private GameObject metalCrate;
    [SerializeField]
    private GameObject woodCrate;
    [SerializeField]
    private GameObject key;
    [SerializeField]
    private GameObject spikes;
    [SerializeField]
    private bool spikesEnabled;
    [SerializeField]
    private GameObject arrowTrap;
    [SerializeField]
    private bool arrowTrapEnabled;
    Coroutine coroutine;
    private int[] rocks = new int[1452];
    private int[] woods = new int[1452];
    private int[] walls = new int[1452];
    private int[] snows = new int[1452];
    private int[] obsidians = new int[1452];
    private int[] empties = new int[1452];




    void Start()
    {
        if (LV0)
        {
            FindObjectOfType<playerStats>().health = 5;
            int entrypos = SpawnMap();
            SpawnNonPathRooms();
            spawnPos = nodes[entrypos - 1].transform.position;
            spawnPos.x += 5f;
            spawnPos.y -= 5f;
            player = Instantiate(player, spawnPos, Quaternion.identity);
            player.GetComponent<playerMovement>().canMove = true;
            player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            player.GetComponent<Rigidbody2D>().isKinematic = false;
            player.GetComponent<CircleCollider2D>().enabled = true;
            MergeTiles();
        }
        else
        {
            int entrypos = SpawnMap();
            SpawnNonPathRooms();
            spawnPos = nodes[entrypos - 1].transform.position;
            spawnPos.x += 5f;
            spawnPos.y -= 5f;
            if (GameObject.FindGameObjectWithTag("Player") != null)
            {
                SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetActiveScene());
                GameObject.FindGameObjectWithTag("Player").transform.position = spawnPos;
                player = GameObject.FindGameObjectWithTag("Player");
            }
            else
            {
                player = Instantiate(player, spawnPos, Quaternion.identity);
            }
            player.GetComponent<CanPickUp>().lg = this;
            player.GetComponent<CanPickUp>().itemsForPickUp = new List<GameObject>();
            //this good
            foreach (GameObject sk in GameObject.FindGameObjectsWithTag("SK"))
            {
                sk.GetComponent<SKMovement>().livingthings = livingThings;
            }
            GetItemList();
            if (!MoleBoss)
            {
                StartCoroutine("WaitAndMerge");
            }
        }
    }

    private IEnumerator WaitAndMerge()
    {
        MergeTiles();
        yield return new WaitForSeconds(0.4f);
        SetGemsandEms();
        player.GetComponent<playerMovement>().canMove = true;
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void SpawnNonPathRooms()
    {
        if (LV0)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].transform.childCount == 0)
                {
                    GameObject filled = Instantiate(rooms[6].rooms[Random.Range(0, rooms[6].rooms.Length)], nodes[i].transform.position, Quaternion.identity);
                    filled.transform.parent = nodes[i].transform;
                }
            }
        }
        else
        {
            List<int> empties = new List<int>();
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i].transform.childCount == 0)
                {
                    empties.Add(i);
                }
            }
            if (MoleBoss)
            {
                empties.Remove(10);
                empties.Remove(11);
            }
            if (empties.Count > 1)
            {
                //Shop
                int nodeChoice = empties[Random.Range(0, empties.Count)];
                empties.Remove(nodeChoice);
                GameObject Shop = Instantiate(rooms[13].rooms[Random.Range(0, rooms[13].rooms.Length)], nodes[nodeChoice].transform.position, Quaternion.identity);
                Shop.transform.parent = nodes[nodeChoice].transform;
                //LockedRoom
                int nodeChoice2 = empties[Random.Range(0, empties.Count)];
                empties.Remove(nodeChoice2);
                GameObject Locked = Instantiate(rooms[14].rooms[Random.Range(0, rooms[14].rooms.Length)], nodes[nodeChoice2].transform.position, Quaternion.identity);
                Locked.transform.parent = nodes[nodeChoice2].transform;
            }
            else if (empties.Count > 0)
            {
                //Shop
                int nodeChoice = empties[Random.Range(0, empties.Count)];
                empties.Remove(nodeChoice);
                GameObject Shop = Instantiate(rooms[13].rooms[Random.Range(0, rooms[13].rooms.Length)], nodes[nodeChoice].transform.position, Quaternion.identity);
                Shop.transform.parent = nodes[nodeChoice].transform;
            }
            for (int b = 0; b < empties.Count; b++)
            {
                GameObject filled = Instantiate(rooms[6].rooms[Random.Range(0, rooms[6].rooms.Length)], nodes[empties[b]].transform.position, Quaternion.identity);
                filled.transform.parent = nodes[empties[b]].transform;
            }


            //GameObject filled = Instantiate(rooms[6].rooms[Random.Range(0, rooms[6].rooms.Length)], nodes[i].transform.position, Quaternion.identity);
            //filled.transform.parent = nodes[i].transform;
        }
    }


    public int SpawnMap()
    {
        
        int entrypos = Random.Range(1, 4);
        int direction = 0;
        int endPos = Random.Range(9, 12);
        //Debug.Log(endPos);
        if (entrypos == 1)
        {
            //right or forward entry (8 or 9)
            int choice1 = Random.Range(0, 2);
            direction = choice1 + 2;
            GameObject entryRoom = Instantiate(rooms[choice1 + 8].rooms[Random.Range(0, rooms[choice1 + 8].rooms.Length)], nodes[0].transform.position, Quaternion.identity);
            entryRoom.transform.parent = nodes[0].transform;
        }
        else if (entrypos == 2)
        {
            //any entry (7,8 or 9)
            int choice1 = Random.Range(0, 3);
            direction = choice1 + 1;
            GameObject entryRoom = Instantiate(rooms[choice1 + 7].rooms[Random.Range(0, rooms[choice1 + 7].rooms.Length)], nodes[1].transform.position, Quaternion.identity);
            entryRoom.transform.parent = nodes[1].transform;
        }
        else if (entrypos == 3)
        {
            //Left or forward entry (7 or 8)
            int choice1 = Random.Range(0, 2);
            direction = choice1 + 1;
            GameObject entryRoom = Instantiate(rooms[choice1 + 7].rooms[Random.Range(0, rooms[choice1 + 7].rooms.Length)], nodes[2].transform.position, Quaternion.identity);
            entryRoom.transform.parent = nodes[2].transform;
        }
        int nextPos = GetNextPos(direction, entrypos-1);
        int currentPos = entrypos - 1;
        int previousDirection = direction;
        bool routeCompleted = false;
        while (!routeCompleted)
        {
            //Debug.Log(nextPos + " = nextpos");
            if (nextPos < 9)
            {
                currentPos = nextPos;
                previousDirection = direction;
                if (currentPos % 3 == 0)
                {
                    //Left Column
                    direction = Random.Range(2, 4);
                    while (direction - previousDirection == 2 || direction - previousDirection == -2)
                    {
                        direction = Random.Range(2, 4);
                    }
                    //Debug.Log("direction " + direction);
                    //Debug.Log("previous " + previousDirection);
                    nextPos = GetNextPos(direction, currentPos);
                    GetRoom(direction, previousDirection, currentPos);
                }
                else if ((currentPos - 1) % 3 == 0)
                {
                    //Middle Column
                    direction = Random.Range(1, 4);
                    while (direction - previousDirection == 2 || direction - previousDirection == -2)
                    {
                        direction = Random.Range(1, 4);
                    }
                    //Debug.Log("direction " + direction);
                    //Debug.Log("previous " + previousDirection);
                    nextPos = GetNextPos(direction, nextPos);
                    GetRoom(direction, previousDirection, currentPos);
                }
                else
                {
                    //Right Column
                    direction = Random.Range(1, 3);
                    while (direction - previousDirection == 2 || direction - previousDirection == -2)
                    {
                        direction = Random.Range(1, 3);
                    }
                    //Debug.Log("direction " + direction);
                    //Debug.Log("previous " + previousDirection);
                    nextPos = GetNextPos(direction, nextPos);
                    GetRoom(direction, previousDirection, currentPos);
                }
                //Debug.Log(direction + " = direction");
            }
            else
            {                
                //On Final Row
                if (!MoleBoss)
                {
                    if (currentPos == endPos)
                    {
                        //spawn backEnd
                        GameObject room = Instantiate(rooms[11].rooms[Random.Range(0, rooms[11].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
                        room.transform.parent = nodes[currentPos].transform;
                        routeCompleted = true;
                    }
                    else
                    {
                        while (currentPos != endPos)
                        {
                            currentPos = nextPos;
                            previousDirection = direction;
                            if (endPos > currentPos)
                            {
                                if (endPos - currentPos == 1)
                                {
                                    //spawn room and LeftEndRoom
                                    GameObject room = Instantiate(rooms[10].rooms[Random.Range(0, rooms[10].rooms.Length)], nodes[endPos].transform.position, Quaternion.identity);
                                    room.transform.parent = nodes[endPos].transform;
                                    direction = 3;
                                    nextPos = GetNextPos(direction, nextPos);
                                    GetRoom(direction, previousDirection, currentPos);
                                    routeCompleted = true;
                                }
                                else
                                {
                                    direction = 3;
                                    nextPos = GetNextPos(direction, nextPos);
                                    GetRoom(direction, previousDirection, currentPos);
                                }
                            }
                            else if (endPos < currentPos)
                            {
                                if (endPos - currentPos == -1)
                                {
                                    //spawn room and RightEndRoom
                                    GameObject room = Instantiate(rooms[12].rooms[Random.Range(0, rooms[12].rooms.Length)], nodes[endPos].transform.position, Quaternion.identity);
                                    room.transform.parent = nodes[endPos].transform;
                                    direction = 1;
                                    nextPos = GetNextPos(direction, nextPos);
                                    GetRoom(direction, previousDirection, currentPos);
                                    routeCompleted = true;
                                }
                                else
                                {
                                    direction = 1;
                                    nextPos = GetNextPos(direction, nextPos);
                                    GetRoom(direction, previousDirection, currentPos);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //spawn backEnd
                    GameObject room = Instantiate(rooms[15].rooms[Random.Range(0, rooms[15].rooms.Length)], nodes[9].transform.position, Quaternion.identity);
                    room.transform.parent = nodes[9].transform;
                    routeCompleted = true;
                }
                         
            }
            
            //routeCompleted = true;
        }
        return entrypos;
        
    }

    public int GetNextPos(int direction, int currentPos)
    {
        int nextPos;
        if (direction == 2)
        {
            nextPos = currentPos + 3;
        }
        else
        {
            nextPos = currentPos + direction - 2;
        }
        return nextPos;
    }

    
    public void GetRoom(int direction, int previousDirection, int currentPos)
    {
        GameObject room;
        int roomNumber = direction - previousDirection;
        if (roomNumber == 0)
        {
            if (direction == 2)
            {
                room = Instantiate(rooms[1].rooms[Random.Range(0, rooms[1].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
            }
            // LeftRight
            else
            {
                room = Instantiate(rooms[0].rooms[Random.Range(0, rooms[0].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
            }
        }
        else if (roomNumber == -1)
        {
            //LeftForward
            if (direction == 2)
            {
                room = Instantiate(rooms[3].rooms[Random.Range(0, rooms[3].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
            }
            // BackLeft
            else
            {
                room = Instantiate(rooms[5].rooms[Random.Range(0, rooms[5].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
            }
        }
        else
        {
            // RightForward
            if (direction == 2)
            {
                room = Instantiate(rooms[2].rooms[Random.Range(0, rooms[2].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
            }
            //BackRight
            else
            {
                room = Instantiate(rooms[4].rooms[Random.Range(0, rooms[4].rooms.Length)], nodes[currentPos].transform.position, Quaternion.identity);
            }
        }
        room.transform.parent = nodes[currentPos].transform;
    }   

    public void unleashCreature()
    {
        Instantiate(creature, spawnPos, Quaternion.identity);
    }

    public void GetItemList()
    {
        itemsForPickUp.Clear();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("PickupItems"))
        {
            itemsForPickUp.Add(item);
        }
    }
    
    public void GetLivingThings()
    {
        livingThings.Clear();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("Living"))
        {
            livingThings.Add(item);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("SK"))
        {
            livingThings.Add(item);
        }
        livingThings.Add(player);        
    }

    private void SetGemsandEms()
    {
        Tilemap wall = nodes[0].transform.GetChild(0).Find("Walls").gameObject.GetComponent<Tilemap>();
        Tilemap rock = nodes[0].transform.GetChild(0).Find("Rock").gameObject.GetComponent<Tilemap>();
        Tilemap obsidian = nodes[0].transform.GetChild(0).Find("Obsidian").gameObject.GetComponent<Tilemap>();
           
        BoundsInt bounds = wall.cellBounds;
        if (rock.cellBounds.size.x > bounds.size.x)
        {
            bounds = rock.cellBounds;
        }
        if(obsidian.cellBounds.size.x > bounds.size.x)
        {
            bounds = obsidian.cellBounds;
        }

        TileBase[] wallTiles = wall.GetTilesBlock(bounds);         
        TileBase[] rockTiles = rock.GetTilesBlock(bounds);         
        TileBase[] obsidianTiles = obsidian.GetTilesBlock(bounds);

        //quickfixfornew 2 - 1 not working cus of there being no walls
        //replacing bounds.size.x and y with new vec 2 called fakebounds.x andy;

        Vector2Int fakebounds = new Vector2Int(33, 44);


        for (int x = 0; x < fakebounds.x; x++)
        {
            for (int y = 0; y < fakebounds.y; y++)
            {
                TileBase rockTile = rockTiles[x + y * fakebounds.x]; 
                TileBase wallTile = wallTiles[x + y * fakebounds.x];
                TileBase obsidianTile = obsidianTiles[x + y * fakebounds.x];
                //checks if tile there = FALSE
                if (wallTile == null && rockTile == null && obsidianTile == null)
                {
                    if (CheckBool("enemies", x, y))
                    {
                        if (Random.Range(0,SnekChance) < 1)
                        {
                            if (snekEnabled)
                            {
                                Vector3 spawnpoint = wall.transform.position;
                                spawnpoint.x = spawnpoint.x + x - 0.5f;
                                spawnpoint.y = spawnpoint.y + y - 9.5f;
                                Instantiate(snek, spawnpoint, Quaternion.identity);
                            }
                        }
                        else if (Random.Range(0, BatChance) < 1)
                        {
                            if (batEnabled)
                            {
                                Vector3 spawnpoint = wall.transform.position;
                                spawnpoint.x = spawnpoint.x + x - 0.5f;
                                spawnpoint.y = spawnpoint.y + y - 9.5f;
                                Instantiate(bat, spawnpoint, Quaternion.identity);
                            }
                        }
                        else if (Random.Range(0, BombyChance) < 1)
                        {
                            if (bombyEnabled)
                            {
                                Vector3 spawnpoint = wall.transform.position;
                                spawnpoint.x = spawnpoint.x + x - 0.5f;
                                spawnpoint.y = spawnpoint.y + y - 9.5f;
                                Instantiate(bomby, spawnpoint, Quaternion.identity);
                            }
                        }
                    }
                    if (CheckBool("items", x, y))
                    {
                        if (Random.Range(0, PebbleChance) < 1)
                        {
                            Vector3 spawnpoint = wall.transform.position;
                            spawnpoint.x = spawnpoint.x + x - 0.5f;
                            spawnpoint.y = spawnpoint.y + y - 9.5f;
                            Instantiate(pebble, spawnpoint, Quaternion.identity);
                        }
                        else if (Random.Range(0, MetalCrateChance) < 1)
                        {
                            Vector3 spawnpoint = wall.transform.position;
                            spawnpoint.x = spawnpoint.x + x - 0.5f;
                            spawnpoint.y = spawnpoint.y + y - 9.5f;
                            Instantiate(metalCrate, spawnpoint, Quaternion.identity);
                        }
                        else if (Random.Range(0, WoodCrateChance) < 1)
                        {
                            Vector3 spawnpoint = wall.transform.position;
                            spawnpoint.x = spawnpoint.x + x - 0.5f;
                            spawnpoint.y = spawnpoint.y + y - 9.5f;
                            Instantiate(woodCrate, spawnpoint, Quaternion.identity);
                        }
                    }
                    if (CheckBool("spikes", x, y))
                    {
                        if (Random.Range(0, SpikeChance) < 1)
                        {
                            if (spikesEnabled)
                            {
                                Vector3 spawnpoint = wall.transform.position;
                                spawnpoint.x = spawnpoint.x + x - 0.5f;
                                spawnpoint.y = spawnpoint.y + y - 9.5f;
                                Instantiate(spikes, spawnpoint, Quaternion.identity);
                            }
                        }
                        else if (Random.Range(0, ArrowTrapChance) < 1)
                        {
                            if (arrowTrapEnabled)
                            {
                                Vector3 spawnpoint = wall.transform.position;
                                spawnpoint.x = spawnpoint.x + x - 0.5f;
                                spawnpoint.y = spawnpoint.y + y - 9.5f;
                                Instantiate(arrowTrap, spawnpoint, Quaternion.identity);
                            }
                        }
                    }
                }
                else
                {
                    //Debug.Log("x:" + x + " y:" + y + " tile:" + tile.name);
                    if (CheckBool("gems", x, y))
                    {
                        if (wallTile != null)
                        {
                            if (wallTile.name.Contains("Dirt"))
                            {
                                if (Random.Range(0, DiamondChance) < 1) wall.SetTile(new Vector3Int(x - 1, y - 10, 0), Diamond);
                                else if (Random.Range(0, GoldChance) < 1) wall.SetTile(new Vector3Int(x - 1, y - 10, 0), Gold);
                                else if (Random.Range(0, SilverChance) < 1) wall.SetTile(new Vector3Int(x - 1, y - 10, 0), Silver);
                                
                            }
                        }
                        else if (rockTile != null)
                        {
                            if (rockTile.name.Contains("Rock"))
                            {
                                if (Random.Range(0, DiamondChance) < 1) rock.SetTile(new Vector3Int(x - 1, y - 10, 0), DiamondRock);
                                else if (Random.Range(0, GoldChance) < 1) rock.SetTile(new Vector3Int(x - 1, y - 10, 0), GoldRock);
                                else if (Random.Range(0, SilverChance) < 1) rock.SetTile(new Vector3Int(x - 1, y - 10, 0), SilverRock);
                            }
                        }
                    }
                    if (CheckBool("enemies", x, y))
                    {
                        if (wallTile != null)
                        {
                            if (wallTile.name.Contains("Dirt"))
                            {
                                if (Random.Range(0, MongChance) < 1)
                                {
                                    if (mongEnabled)
                                    {
                                        Vector3 spawnpoint = wall.transform.position;
                                        spawnpoint.x = spawnpoint.x + x - 0.5f;
                                        spawnpoint.y = spawnpoint.y + y - 9.5f;
                                        Instantiate(mong, spawnpoint, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                                    }
                                }
                                else if (Random.Range(0, MoleChance) < 1)
                                {
                                    if (moleEnabled)
                                    {
                                        Vector3 spawnpoint = wall.transform.position;
                                        spawnpoint.x = spawnpoint.x + x - 0.5f;
                                        spawnpoint.y = spawnpoint.y + y - 9.5f;
                                        Instantiate(mole, spawnpoint, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            
        }
        GetLivingThings();
    }

    private bool CheckBool(string type, int x, int y)
    {
        bool result = false;
        if (type == "enemies")
        {
            result = nodes[x / 11 + (y / 11) * 3].transform.GetChild(0).GetComponent<RoomInfo>().enemies;
        }
        else if (type == "gems")
        {
            result = nodes[x/11+(y/11)*3].transform.GetChild(0).GetComponent<RoomInfo>().gems;
        }
        else if (type == "spikes")
        {
            result = nodes[x / 11 + (y / 11) * 3].transform.GetChild(0).GetComponent<RoomInfo>().spikes;
        }
        else if (type == "items")
        {
            result = nodes[x / 11 + (y / 11) * 3].transform.GetChild(0).GetComponent<RoomInfo>().items;
        }
        return result;
    }

    public void MergeTiles()
    {
        for (int i = 0; i < nodes.Length - 1; i++)
        {
            if(nodes[i].transform.childCount > 0)
            {                
                for (int children = 0; children < nodes[i].transform.GetChild(0).childCount; children++)
                {
                    if (nodes[i].transform.GetChild(0).GetChild(children).gameObject.TryGetComponent(out Tilemap wall))
                    {
                        BoundsInt bounds = wall.cellBounds;                        
                        TileBase[] allTiles = wall.GetTilesBlock(bounds);
                        //quickfixfornew 2 - 1 not working cus of there being no walls
                        //replacing bounds.size.x and y with new vec 2 called fakebounds.x andy;


                        if (wall.gameObject.name == "Walls")
                        {
                            for (int x = 0; x < bounds.size.x; x++)
                            {
                                for (int y = 0; y < bounds.size.y; y++)
                                {
                                    TileBase tile = allTiles[x + y * bounds.size.x];
                                    if (tile != null)
                                    {
                                        if (tile.name == "SnowRule")
                                        {
                                            snows[i * 121 + x + y * bounds.size.x] = 1;
                                            wall.SetTile(new Vector3Int(x - 1, y - 10, 0), null);
                                        }
                                        else if (tile.name.Contains("Dirt")) 
                                        {
                                            walls[i * 121 + x + y * bounds.size.x] = 1;
                                            wall.SetTile(new Vector3Int(x - 1, y - 10, 0), null);
                                        }
                                    }
                                }
                            }
                        }
                        if (wall.gameObject.name == "Rock")
                        {
                            for (int x = 0; x < bounds.size.x; x++)
                            {
                                for (int y = 0; y < bounds.size.y; y++)
                                {
                                    TileBase tile = allTiles[x + y * bounds.size.x];
                                    if (tile != null)
                                    {                                        
                                        if (tile.name == "WoodWall")
                                        {
                                            woods[i * 121 + x + y * bounds.size.x] = 1;
                                        }                                                         
                                        else
                                        {
                                            rocks[i * 121 + x + y * bounds.size.x] = 1;
                                        }
                                        wall.SetTile(new Vector3Int(x - 1, y - 10, 0), null);                                        
                                    }
                                }
                            }
                        }
                        if (wall.gameObject.name == "Obsidian")
                        {
                            for (int x = 0; x < bounds.size.x; x++)
                            {
                                for (int y = 0; y < bounds.size.y; y++)
                                {
                                    TileBase tile = allTiles[x + y * bounds.size.x];
                                    if (tile != null)
                                    {                                        
                                        obsidians[i * 121 + x + y * bounds.size.x] = 1;
                                        wall.SetTile(new Vector3Int(x - 1, y - 10, 0), null);                                                                               
                                    }
                                }
                            }
                        }
                    }
                }
            }            
        }
        //add them all back in the first node's tilemap
        
        for (int children = 0; children < nodes[0].transform.GetChild(0).childCount; children++)
        {
            if (nodes[0].transform.GetChild(0).GetChild(children).gameObject.TryGetComponent(out Tilemap wall))
            {
                BoundsInt bounds = wall.cellBounds;
                if (wall.gameObject.name == "Walls")
                {
                    for (int i = 0; i < nodes.Length - 1; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        for (int x = 0; x < bounds.size.x; x++)
                        //for (int x = 0; x < 2; x++)
                        {
                            for (int y = 0; y < bounds.size.y; y++)
                            //for (int y = 10; y < 11; y++)
                            {
                                if (walls[i * 121 + x + y * bounds.size.x] == 1)
                                {
                                    wall.SetTile(new Vector3Int((x - 1) + (i % 3) * bounds.size.x, y - 10 + (i / 3) * bounds.size.y, 0), DirtRule);
                                }
                                else if(snows[i*121+x+y*bounds.size.x] == 1)
                                {
                                    wall.SetTile(new Vector3Int((x - 1) + (i % 3) * bounds.size.x, y - 10 + (i / 3) * bounds.size.y, 0), SnowRule);
                                }
                            }
                        }
                    }
                }
                if (wall.gameObject.name == "Rock")
                {
                    for (int i = 0; i < nodes.Length - 1; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        for (int x = 0; x < bounds.size.x; x++)
                        //for (int x = 0; x < 2; x++)
                        {
                            for (int y = 0; y < bounds.size.y; y++)
                            //for (int y = 10; y < 11; y++)
                            {
                                if (rocks[i * 121 + x + y * bounds.size.x] == 1)
                                {
                                    wall.SetTile(new Vector3Int((x - 1) + (i % 3) * bounds.size.x, y - 10 + (i / 3) * bounds.size.y, 0), RockRule);
                                }
                                else if (woods[i * 121 + x + y * bounds.size.x] == 1)
                                {
                                    wall.SetTile(new Vector3Int((x - 1) + (i % 3) * bounds.size.x, y - 10 + (i / 3) * bounds.size.y, 0), Wood);
                                }
                            }
                        }
                    }
                }
                if (wall.gameObject.name == "Obsidian")
                {
                    for (int i = 0; i < nodes.Length - 1; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        for (int x = 0; x < bounds.size.x; x++)
                        //for (int x = 0; x < 2; x++)
                        {
                            for (int y = 0; y < bounds.size.y; y++)
                            //for (int y = 10; y < 11; y++)
                            {
                                if (obsidians[i * 121 + x + y * bounds.size.x] == 1)
                                {
                                    wall.SetTile(new Vector3Int((x - 1) + (i % 3) * bounds.size.x, y - 10 + (i / 3) * bounds.size.y, 0), ObsidianRule);
                                }
                            }
                        }
                    }
                }
            }
        }        
    }


    public void GenerateKey()
    {
        StartCoroutine(KeyGenDelay());
        Debug.Log("Key Spawning");
    }

    private IEnumerator KeyGenDelay() 
    {
        yield return new WaitForSeconds(0.8f);
        //NEW METHOD
        BoundsInt bounds = nodes[0].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Tilemap>().cellBounds;
        int sum = 0;
        for (int z = 0; z < empties.Length; z++) 
        {
            if (walls[z] == 0 && rocks[z] == 0 && snows[z] == 0 && obsidians[z] == 0 && woods[z] == 0) 
            {
                empties[z] = 1;
                sum++;
            }
        }
        
        int spot = Random.Range(0, sum);
        int temp = 0;
        for(int p = 0; p < empties.Length; p++)
        {
            if (empties[p] == 1 ) 
            {
                temp++;
            }
            if (temp == spot)
            {
                int i = p / 121;
                Debug.Log(sum);
                int y = (p % 121) / 11;
                int x = (p % 121) % 11;
                Vector2 spawnpoint = nodes[0].transform.GetChild(0).position;
                spawnpoint.y -= 9.5f;
                spawnpoint.x -= 0.5f;
                spawnpoint.x += x + i % 3 * 11;
                spawnpoint.y += y + i / 3 * 11;
                Instantiate(key, spawnpoint, Quaternion.identity);
                break;
            }
        }
    }
}
